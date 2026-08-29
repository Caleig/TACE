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

            Player player =
                Main.LocalPlayer;


            if (player == null || !player.active)
                return;


            if (item.DamageType != DamageClass.Magic)
                return;


            if (item.damage <= 0)
                return;



            GemContractPlayer contract =
                player.GetModPlayer<GemContractPlayer>();


            if (!contract.HasAnyContract())
                return;

            float convertedBaseDamage =
                item.damage * 0.65f;

            StatModifier summon =
                player.GetTotalDamage(
                    DamageClass.Summon
                );


            int convertedDamage =
                (int)summon.ApplyTo(
                    convertedBaseDamage
                );



            foreach (TooltipLine line in tooltips)
            {

                if (
                    line.Mod == "Terraria"
                    &&
                    line.Name == "Damage"
                )
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