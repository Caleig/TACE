using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace ThoriumAccessoryExpansion.Accessories.MagicSheath
{
    public class MagicSheathGlobalItem : GlobalItem
    {
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            var mp = player.GetModPlayer<MagicSheathPlayer>();
            if (mp == null) return;

            if (item.DamageType == DamageClass.Magic)
            {
                int flatBonus = 0;
                if (mp.SheathLevel == 2)
                    flatBonus = 3;
                else if (mp.SheathLevel == 3)
                    flatBonus = 4;
                else if (mp.SheathLevel == 0 && mp.HasSpiritBlade)
                    flatBonus = 5;

                if (mp.SheathLevel == 3)
                {
                    damage += 0.15f;
                }

                damage.Flat += flatBonus;
            }
        }

        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
        {
            var mp = player.GetModPlayer<MagicSheathPlayer>();
            if (mp.SheathLevel > 0 && item.DamageType == DamageClass.Magic)
            {
                mult *= mp.GetManaCostMultiplier();
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            var mp = player.GetModPlayer<MagicSheathPlayer>();
            if (mp.SheathLevel == 3)
            {
                player.statDefense += mp.GetDefenseBonus();
            }
        }
    }
}