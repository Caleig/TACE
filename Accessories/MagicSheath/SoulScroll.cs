using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.MagicSheath;
namespace ThoriumAccessoryExpansion.Accessories.MagicSheath;

public class SoulScroll : ScrollBase
{
    public override int ScrollTypeID => 4;
    public override void SetStaticDefaults()
    {
    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Ectoplasm, 10)
            .AddIngredient(ModContent.Find<ModItem>("ThoriumMod", "CursedCloth").Type, 5)
            .AddTile(TileID.Loom)
            .Register();
    }
}