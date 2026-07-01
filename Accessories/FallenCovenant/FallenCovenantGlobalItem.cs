using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.CursedCovenant;
using ThoriumAccessoryExpansion.Accessories.FallenCovenant;
using ThoriumAccessoryExpansion.Accessories.KarmaCovenant;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;

namespace ThoriumAccessoryExpansion.Accessories.FallenCovenant
{
    public class FallenCovenantGlobalItem : GlobalItem
    {
        private bool IsRadiantWeapon(Item item)
        {
            return item.DamageType != null && item.DamageType == ModContent.GetInstance<HealerDamage>();
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (!IsRadiantWeapon(item)) return;
            CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
            if (!cp.FallenHasCovenant) return;

            int stacks = cp.FallenRadianceStacks;
            int bonusFlat = (int)(stacks * 12 / CovenantPlayer.GlobalMaxStacks);
            if (bonusFlat > 0)
                damage.Flat += bonusFlat;
            if (stacks >= CovenantPlayer.GlobalMaxStacks)
                damage += 0.12f;
        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
        {
            if (!IsRadiantWeapon(item)) return;
            CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
            if (!cp.FallenHasCovenant) return;

            if (cp.FallenRadianceStacks >= CovenantPlayer.GlobalMaxStacks)
                crit += 5f;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (IsRadiantWeapon(item))
            {
                CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
                if (cp.FallenHasCovenant && player.statLife > 2)
                {
                    player.statLife -= 2;
                    CombatText.NewText(
                        new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height),
                        CombatText.DamagedFriendly,
                        2,
                        false
                    );
                    player.AddBuff(ModContent.BuffType<LifeDrainCooldown>(), 5);
                }
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!IsRadiantWeapon(item)) return;
            CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
            if (!cp.FallenHasCovenant) return;

            cp.FallenRadianceStacks = (int)MathHelper.Clamp(cp.FallenRadianceStacks + 5, 0, CovenantPlayer.GlobalMaxStacks);
            cp.FallenRadianceTimer = 3600;
            target.AddBuff(BuffID.ShadowFlame, 300);
            base.OnHitNPC(item, player, target, hit, damageDone);
        }
    }

    public class FallenCovenantGlobalProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            CovenantPlayer cp = Main.player[projectile.owner].GetModPlayer<CovenantPlayer>();
            if (hit.DamageType == ModContent.GetInstance<HealerDamage>() && cp.FallenHasCovenant)
            {
                cp.FallenRadianceStacks = (int)MathHelper.Clamp(cp.FallenRadianceStacks + 5, 0, CovenantPlayer.GlobalMaxStacks);
                cp.FallenRadianceTimer = 3600;
                target.AddBuff(BuffID.ShadowFlame, 300);
            }
        }
    }
}