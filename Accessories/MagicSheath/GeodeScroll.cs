using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.MagicSheath;
using ThoriumMod.Items.Geode;
using ThoriumMod.Items.Lodestone;
namespace ThoriumAccessoryExpansion.Accessories.MagicSheath
;
public class GeodeScroll : ScrollBase
{
    public override int ScrollTypeID => 2;
    public override void SetStaticDefaults()
    {
    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<LodeStoneIngot>(), 10)
            .AddIngredient(ItemID.Silk, 5)
            .AddIngredient(ItemID.SoulofMight, 1)
            .AddIngredient(ItemID.SoulofSight, 1)
            .AddIngredient(ItemID.SoulofFright, 1)
            .AddTile(TileID.Loom)
            .Register();
    }
}