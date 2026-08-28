using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Systems
{
    public class MagicContractTooltip : GlobalItem
    {
        public override void ModifyTooltips(
            Item item,
            List<TooltipLine> tooltips)
        {

            Player player = Main.LocalPlayer;


            if (player == null || !player.active)
                return;


            if (item.DamageType != DamageClass.Magic)
                return;


            if (item.damage <= 0)
                return;



            GemContractPlayer contract =
                player.GetModPlayer<GemContractPlayer>();



            float conversionRate = 0.65f;

            if (!contract.HasAnyContract())
                return;



            int convertedDamage =
                (int)(item.damage * conversionRate);



            foreach (TooltipLine line in tooltips)
            {

                if (line.Mod == "Terraria" &&
                    line.Name == "Damage")
                {

                    line.Text =
                        $"{convertedDamage}{DamageClass.Summon.DisplayName.Value}";


                    line.OverrideColor =
                        Color.Pink;


                    break;

                }

            }

        }
    }
}