using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.EternalBulwark;
using ThoriumMod;
using ThoriumMod.Buffs.Healer;
using ThoriumMod.Projectiles.Healer;

namespace ThoriumAccessoryExpansion.Players
{
    public class CovenantPlayer : ModPlayer
    {
        // 全局最大层数（公倍数）
        public const int GlobalMaxStacks = 60; // 10,5,12,15 的最小公倍数为 60

        // ========== 各圣约佩戴标志 ==========
        public bool BoneHasCovenant = false;
        public bool CursedHasCovenant = false;
        public bool FallenHasCovenant = false;
        public bool KarmaHasCovenant = false;
        public bool HeresyHasCovenant = false;
        public bool EternalHasCovenant = false;
        public bool MichaelasHasCovenant = false;

        // ========== 共用堕落光辉层数（上限 GlobalMaxStacks） ==========
        public int FallenRadianceStacks = 0;
        public int FallenRadianceTimer = 0;

        // ========== 各圣约独立数据（非层数） ==========
        public int HeresyLifeRegenTimer = 0;
        public int EternalProtectionCooldown = 0;
        public bool EternalIsProtected = false;

        private bool _boneHasAppliedHealDebuff = false;
        private int _boneDebugTimer = 0;
        private int _boneLastDisplayedHealBonus = int.MinValue;

        // ========== 初始化 ==========
        public override void Initialize()
        {
        }

        // ========== ResetEffects ==========
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
        }

        public int GetMaxStacks() => GlobalMaxStacks;

        // ========== UpdateDead ==========
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
            HeresyLifeRegenTimer = 0;
            EternalProtectionCooldown = 0;
            EternalIsProtected = false;

            _boneDebugTimer = 0;
            _boneLastDisplayedHealBonus = int.MinValue;
        }

        // ========== UpdateEquips（骨圣约治疗减益） ==========
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

        // ========== PostUpdate ==========
        public override void PostUpdate()
        {
            if (FallenRadianceTimer > 0)
            {
                FallenRadianceTimer--;
                if (FallenRadianceTimer == 0)
                    FallenRadianceStacks = 0;
            }

            // ---- 骨圣约调试输出（仅单机） ----
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

            
            // ---- 赫瑞之孽：生命再生 ----
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

            // ---- 永恒壁垒：冷却 & 保护状态 ----
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

        // ========== EternalBulwark 的 ModifyHurt 和 OnHurt ==========
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

        // ========== 辅助方法 ==========
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