using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.MagicSheath;

namespace ThoriumAccessoryExpansion.Accessories.MagicSheath
{
    public class EnergyScroll : ScrollBase
    {
        public override int ScrollTypeID => 1;
        public override void SetStaticDefaults()
        {
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.Find<ModItem>("ThoriumMod", "GraniteEnergyCore").Type, 10)
                .AddIngredient(ItemID.Silk, 5)
                .AddTile(TileID.Loom)
                .Register();
        }
    }
}