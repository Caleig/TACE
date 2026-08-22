using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath
{
    public abstract class ScrollBase : ModItem
    {
        public abstract int ScrollTypeID { get; }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.UseSound = SoundID.Item4;
            Item.rare = ItemRarityID.LightRed;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override bool? UseItem(Player player)
        {
            var sp = player.GetModPlayer<ScrollPlayer>();
            sp.ToggleScroll(ScrollTypeID);
            string status = sp.IsScrollActive(ScrollTypeID) ? "激活" : "关闭";
            Main.NewText($"刃契 {Item.Name} 已{status}", Color.Orange);
            return true;
        }

        public abstract override void AddRecipes();
    }
}