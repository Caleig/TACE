using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.Healer.CursedCovenant;
using ThoriumAccessoryExpansion.Accessories.Healer.KarmaCovenant;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;

namespace ThoriumAccessoryExpansion.Accessories.Healer.KarmaCovenant
{
    public class KarmaCovenantGlobalItem : GlobalItem
    {
        private bool IsRadiantWeapon(Item item)
        {
            return item.DamageType != null && item.DamageType == ModContent.GetInstance<HealerDamage>();
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (!IsRadiantWeapon(item)) return;
            CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
            if (!cp.KarmaHasCovenant) return;

            int stacks = cp.FallenRadianceStacks;
            int bonusFlat = (int)(stacks * 15 / CovenantPlayer.GlobalMaxStacks);
            if (bonusFlat > 0)
                damage.Flat += bonusFlat;
            if (stacks >= CovenantPlayer.GlobalMaxStacks)
                damage += 0.15f;
        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
        {
            if (!IsRadiantWeapon(item)) return;
            CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
            if (!cp.KarmaHasCovenant) return;

            if (cp.FallenRadianceStacks >= CovenantPlayer.GlobalMaxStacks)
                crit += 8f;
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
            if (IsRadiantWeapon(item) && cp.KarmaHasCovenant)
            {
                if (player.statLife > 3)
                {
                    player.statLife -= 3;
                    CombatText.NewText(
                    new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height),
                    CombatText.DamagedFriendly,
                    3,
                    false
                );
                }
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!IsRadiantWeapon(item)) return;
            CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
            if (!cp.KarmaHasCovenant) return;

            cp.FallenRadianceStacks = (int)MathHelper.Clamp(cp.FallenRadianceStacks + 4, 0, CovenantPlayer.GlobalMaxStacks);
            cp.FallenRadianceTimer = 3600;

            target.AddBuff(ModContent.BuffType<UnholyKarma>(), 300);
            base.OnHitNPC(item, player, target, hit, damageDone);
        }
    }
}