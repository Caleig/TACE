using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.Misc;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath;

public class TerraMagicSheath : MagicSheathBase
{
    public override int SheathLevel => 3;

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;
        Item.accessory = true;

        Item.rare = ItemRarityID.Yellow;
        Item.value = Item.sellPrice(gold: 10);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<SpiritMagicSheath>()
            .AddIngredient(ItemID.AvengerEmblem)
            .AddIngredient<BrokenHeroFragment>(3)
            .AddIngredient(ItemID.Ectoplasm, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}