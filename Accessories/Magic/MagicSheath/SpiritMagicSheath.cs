using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.MagicItems;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath;

public class SpiritMagicSheath : MagicSheathBase
{
    public override int SheathLevel => 2;

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;
        Item.accessory = true;

        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(gold: 5);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<SpiritBlade>()
            .AddIngredient<MagicSheath>()
            .AddIngredient<HallowedCharm>(5)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}