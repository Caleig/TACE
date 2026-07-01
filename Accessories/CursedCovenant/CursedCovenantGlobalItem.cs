using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.CursedCovenant;
using ThoriumAccessoryExpansion.Accessories.FallenCovenant;
using ThoriumAccessoryExpansion.Accessories.KarmaCovenant;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Buffs;

namespace ThoriumAccessoryExpansion.Accessories.CursedCovenants
{
    public class CuredCovenantGlobalItem : GlobalItem
    {
        private bool IsRadiantWeapon(Item item)
        {
            return item.DamageType != null && item.DamageType == ModContent.GetInstance<HealerDamage>();
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (!IsRadiantWeapon(item)) return;
            CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
            if (!cp.CursedHasCovenant) return;

            int bonus = (int)(cp.FallenRadianceStacks * 5 / CovenantPlayer.GlobalMaxStacks);
            if (bonus > 0)
                damage.Flat += bonus;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (IsRadiantWeapon(item))
            {
                CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
                if (cp.CursedHasCovenant && player.statLife > 1)
                {
                    player.statLife -= 1;
                    CombatText.NewText(
                        new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height),
                        CombatText.DamagedFriendly,
                        1,
                        false
                    );
                    cp.FallenRadianceStacks = (int)MathHelper.Clamp(cp.FallenRadianceStacks + 12, 0, CovenantPlayer.GlobalMaxStacks);
                    cp.FallenRadianceTimer = 3600;
                    player.AddBuff(ModContent.BuffType<LifeDrainCooldown>(), 5);
                }
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!IsRadiantWeapon(item)) return;
            CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
            if (!cp.CursedHasCovenant) return;

            target.AddBuff(ModContent.BuffType<LightCurse>(), 300);
            base.OnHitNPC(item, player, target, hit, damageDone);
        }
    }

    public class CursedCovenantGlobalProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            CovenantPlayer cp = Main.player[projectile.owner].GetModPlayer<CovenantPlayer>();
            if (hit.DamageType == ModContent.GetInstance<HealerDamage>() && cp.CursedHasCovenant)
            {
                cp.FallenRadianceStacks = (int)MathHelper.Clamp(cp.FallenRadianceStacks + 12, 0, CovenantPlayer.GlobalMaxStacks);
                cp.FallenRadianceTimer = 3600;
            }
        }
    }
}