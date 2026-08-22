using Terraria;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;

namespace ThoriumAccessoryExpansion.Accessories.Healer.MichaelasAid
{
    public class MichaelasAidGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        private int originalUseTime;
        private int originalUseAnimation;
        private bool originalRecorded = false;

        private bool IsRadiantWeapon(Item item)
        {
            return item.DamageType != null && item.DamageType == ModContent.GetInstance<HealerDamage>();
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (!IsRadiantWeapon(item)) return;
            CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
            if (!cp.MichaelasHasCovenant) return;

            int healBonus = GetHealBonus(player);
            if (healBonus > 0)
                damage += healBonus * 0.04f;
        }

        public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
        {
            if (!IsRadiantWeapon(item)) return;
            CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
            if (!cp.MichaelasHasCovenant) return;

            int healBonus = GetHealBonus(player);


            if (healBonus > 0)
            {
                
                crit += healBonus * 4f;
                
            }

        }

        public override void HoldItem(Item item, Player player)
        {
            if (!IsRadiantWeapon(item)) return;
            CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
            if (!cp.MichaelasHasCovenant) return;

            if (!originalRecorded)
            {
                originalUseTime = item.useTime;
                originalUseAnimation = item.useAnimation;
                originalRecorded = true;
            }

            int healBonus = GetHealBonus(player);
            float speedMultiplier = 1.2f;
            if (healBonus > 0)
                speedMultiplier *= (1f + healBonus * 0.04f);

            int newUseTime = (int)(originalUseTime / speedMultiplier);
            int newUseAnimation = (int)(originalUseAnimation / speedMultiplier);

            if (newUseTime < 1) newUseTime = 1;
            if (newUseAnimation < 1) newUseAnimation = 1;

            if (item.useTime != newUseTime)
                item.useTime = newUseTime;
            if (item.useAnimation != newUseAnimation)
                item.useAnimation = newUseAnimation;
        }

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!IsRadiantWeapon(item)) return;
            CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
            if (!cp.MichaelasHasCovenant) return;

            target.AddBuff(ModContent.BuffType<SupremeWrath>(), 600);

            if (target.HasBuff(ModContent.BuffType<SupremeWrath>()))
            {
                ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
                if (thoriumPlayer != null)
                {
                    thoriumPlayer.soulEssence += 1;
                }
            }
        }

        private static int GetHealBonus(Player player)
        {
            if (ModLoader.TryGetMod("ThoriumMod", out Mod thoriumMod))
                return thoriumMod.Call("GetHealerHealBonus", player) as int? ?? 0;
            return 0;
        }
    }
}