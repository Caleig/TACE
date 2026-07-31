using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.MagicSheath;

namespace ThoriumAccessoryExpansion.Accessories.MagicSheath;

public class HolyScroll : ScrollBase
{
    public override int ScrollTypeID => 3;
    public override void SetStaticDefaults()
    {
    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.HallowedBar, 10)
            .AddIngredient(ItemID.ChlorophyteBar, 10)
            .AddIngredient(ModContent.Find<ModItem>("ThoriumMod", "BloomWeave").Type, 5)
            .AddIngredient(ItemID.BrokenHeroSword, 1) // 英雄碎片
            .AddTile(TileID.Loom)
            .Register();
    }
}