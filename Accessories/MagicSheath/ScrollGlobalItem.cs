using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace ThoriumAccessoryExpansion.Accessories.MagicSheath
{
    public class ScrollGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type != ModContent.ItemType<MagicSheath>() &&
                item.type != ModContent.ItemType<SpiritMagicSheath>() &&
                item.type != ModContent.ItemType<TerraMagicSheath>())
                return;

            Player player = Main.LocalPlayer;
            var sp = player.GetModPlayer<ScrollPlayer>();
            if (sp.ActiveScrolls.Count == 0) return;

            Color color;
            if (item.type == ModContent.ItemType<MagicSheath>())
                color = Color.Blue;
            else if (item.type == ModContent.ItemType<SpiritMagicSheath>())
                color = Color.Pink;
            else
                color = Color.Yellow;

            foreach (int id in sp.ActiveScrolls)
            {
                string name = "";
                switch (id)
                {
                    case 0: name = "暴炎"; break;
                    case 1: name = "能量"; break;
                    case 2: name = "地脉"; break;
                    case 3: name = "神圣"; break;
                    case 4: name = "幽冥"; break;
                }
                TooltipLine line = new TooltipLine(Mod, "ScrollActive", $"[c/{color.Hex3()}:{name}]");
                tooltips.Add(line);
            }
        }
    }
}