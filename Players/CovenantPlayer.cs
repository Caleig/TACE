using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.Healer.EternalBulwark;
using ThoriumMod;
using ThoriumMod.Buffs.Healer;
using ThoriumMod.Projectiles.Healer;

namespace ThoriumAccessoryExpansion.Players
{
    public class CovenantPlayer : ModPlayer
    {
        
        public const int GlobalMaxStacks = 60; 

        
        public bool BoneHasCovenant = false;
        public bool CursedHasCovenant = false;
        public bool FallenHasCovenant = false;
        public bool KarmaHasCovenant = false;
        public bool HeresyHasCovenant = false;
        public bool EternalHasCovenant = false;
        public bool MichaelasHasCovenant = false;

        
        public int FallenRadianceStacks = 0;
        public int FallenRadianceTimer = 0;

        
        public int KarmaHealAccumulator = 0;
        private int _karmaPreviousLife = 0;
        public int HeresyLifeRegenTimer = 0;
        public int EternalProtectionCooldown = 0;
        public bool EternalIsProtected = false;

        private bool _boneHasAppliedHealDebuff = false;
        private int _boneDebugTimer = 0;
        private int _boneLastDisplayedHealBonus = int.MinValue;

        
        public override void Initialize()
        {
            _karmaPreviousLife = Player.statLife;
        }

        
        public override void ResetEffects()
        {
            bool anyEquipped = BoneHasCovenant || CursedHasCovenant || FallenHasCovenant || KarmaHasCovenant;

            BoneHasCovenant = false;
            CursedHasCovenant = false;
            FallenHasCovenant = false;
            KarmaHasCovenant = false;
            HeresyHasCovenant = false;
            EternalHasCovenant = false;
            MichaelasHasCovenant = false;

            if (!anyEquipped)
            {
                FallenRadianceStacks = 0;
                FallenRadianceTimer = 0;
            }

            if (!HeresyHasCovenant) HeresyLifeRegenTimer = 0;
            if (!KarmaHasCovenant) KarmaHealAccumulator = 0;
        }

        public int GetMaxStacks() => GlobalMaxStacks;

        
        public override void UpdateDead()
        {
            BoneHasCovenant = false;
            CursedHasCovenant = false;
            FallenHasCovenant = false;
            KarmaHasCovenant = false;
            HeresyHasCovenant = false;
            EternalHasCovenant = false;
            MichaelasHasCovenant = false;

            FallenRadianceStacks = 0;
            FallenRadianceTimer = 0;
            KarmaHealAccumulator = 0;
            HeresyLifeRegenTimer = 0;
            EternalProtectionCooldown = 0;
            EternalIsProtected = false;

            _karmaPreviousLife = Player.statLife;
            _boneDebugTimer = 0;
            _boneLastDisplayedHealBonus = int.MinValue;
        }

        
        public override void UpdateEquips()
        {
            if (BoneHasCovenant)
            {
                if (!_boneHasAppliedHealDebuff)
                    _boneHasAppliedHealDebuff = true;
            }
            else
            {
                if (_boneHasAppliedHealDebuff)
                    _boneHasAppliedHealDebuff = false;
            }
        }

        
        public override void PostUpdate()
        {
            if (FallenRadianceTimer > 0)
            {
                FallenRadianceTimer--;
                if (FallenRadianceTimer == 0)
                    FallenRadianceStacks = 0;
            }

            
            if (BoneHasCovenant && Main.netMode == NetmodeID.SinglePlayer)
            {
                Item heldItem = Player.HeldItem;
                int currentHealBonus = 0;
                bool isHealingItem = heldItem != null && heldItem.healLife > 0;
                if (isHealingItem && ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod))
                    currentHealBonus = thoriumMod.Call("GetHealerHealBonus", Player) as int? ?? 0;

                _boneDebugTimer--;
                if (_boneDebugTimer <= 0)
                {
                    _boneDebugTimer = 60;
                    if (currentHealBonus != _boneLastDisplayedHealBonus)
                    {
                        _boneLastDisplayedHealBonus = currentHealBonus;
                    }
                }
            }
            else
            {
                _boneDebugTimer = 0;
                _boneLastDisplayedHealBonus = int.MinValue;
            }

            
            if (KarmaHasCovenant)
            {
                int currentLife = Player.statLife;
                int lifeIncrease = currentLife - _karmaPreviousLife;
                if (lifeIncrease > 0)
                {
                    KarmaHealAccumulator += lifeIncrease;

                    while (KarmaHealAccumulator >= 30)
                    {
                        KarmaHealAccumulator -= 30;
                        Projectile proj = Projectile.NewProjectileDirect(
                            new EntitySource_ItemUse(Player, null),
                            Player.Center,
                            GetShootVelocity(Player),
                            ModContent.ProjectileType<DarkHeartPro>(),
                            0, 2f, Player.whoAmI
                        );
                        if (proj != null)
                        {
                            float baseDamage = 80f;
                            float finalDamage = baseDamage * Player.GetDamage(ModContent.GetInstance<HealerDamage>()).ApplyTo(1f);
                            proj.damage = (int)finalDamage;
                            proj.DamageType = ModContent.GetInstance<HealerDamage>();
                            if (Main.netMode == NetmodeID.Server)
                                NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
                        }
                    }
                }
                _karmaPreviousLife = currentLife;
            }
            else
            {
                _karmaPreviousLife = Player.statLife;
            }

            
            if (HeresyHasCovenant && HeresyLifeRegenTimer > 0)
            {
                HeresyLifeRegenTimer--;
                if (Main.GameUpdateCount % 12 == 0)
                {
                    Player.statLife++;
                    if (Player.statLife > Player.statLifeMax2)
                        Player.statLife = Player.statLifeMax2;
                    CombatText.NewText(Player.Hitbox, CombatText.HealLife, 1);
                }
            }

            
            if (EternalHasCovenant)
            {
                if (EternalProtectionCooldown > 0)
                    EternalProtectionCooldown--;

                if (EternalIsProtected)
                {
                    bool hasProj = false;
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        Projectile p = Main.projectile[i];
                        if (p.active && p.type == ModContent.ProjectileType<BubbleBulwarkWandPro>() && p.owner == Player.whoAmI)
                        {
                            hasProj = true;
                            break;
                        }
                    }
                    if (!hasProj)
                        EternalIsProtected = false;
                }
            }
        }

        
        public override void ModifyHurt(ref Player.HurtModifiers modifiers)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;
            if (Player.dead || Player.HasBuff(ModContent.BuffType<EternalBulwarkAbsorbBuff>())) return;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player other = Main.player[i];
                if (other == Player || !other.active || other.dead) continue;

                CovenantPlayer eb = other.GetModPlayer<CovenantPlayer>();
                if (!eb.EternalHasCovenant) continue;
                if (other.statLife < other.statLifeMax2 * 0.25f) continue;

                int absorbed = (int)(modifiers.FinalDamage.Flat * 0.25f);
                if (absorbed <= 0) continue;

                modifiers.FinalDamage.Flat -= absorbed;
                other.AddBuff(ModContent.BuffType<EternalBulwarkAbsorbBuff>(), 1);
                other.Hurt(PlayerDeathReason.ByCustomReason(other.name + " 被永恒壁垒吸收伤害"), absorbed, 0, false, false);
                other.ClearBuff(ModContent.BuffType<EternalBulwarkAbsorbBuff>());
                break;
            }
        }

        public override void OnHurt(Player.HurtInfo info)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;
            if (!EternalHasCovenant) return;

            if (Player.statLife < Player.statLifeMax2 * 0.25f)
                TriggerEternalProtection();
        }

        private void TriggerEternalProtection()
        {
            if (EternalProtectionCooldown > 0 || EternalIsProtected) return;
            EternalIsProtected = true;
            EternalProtectionCooldown = 2700;

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(
                    new EntitySource_ItemUse(Player, null),
                    Player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<BubbleBulwarkWandPro>(),
                    0, 0f, Player.whoAmI
                );
                Player.AddBuff(ModContent.BuffType<BubbleBulwarkWandBuff>(), 900);
                Player.AddBuff(ModContent.BuffType<LifeQuartzShieldBuff>(), 900);
                Player.AddBuff(ModContent.BuffType<LifeReganBuff>(), 900);
            }
        }

        
        private Vector2 GetShootVelocity(Player player)
        {
            NPC targetNPC = null;
            float dist = 800f;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && npc.CanBeChasedBy())
                {
                    float d = Vector2.Distance(player.Center, npc.Center);
                    if (d < dist)
                    {
                        dist = d;
                        targetNPC = npc;
                    }
                }
            }
            if (targetNPC != null)
                return (targetNPC.Center - player.Center).SafeNormalize(Vector2.Zero) * 12f;
            else
                return new Vector2(0, -12f);
        }
    }
}