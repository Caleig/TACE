using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath;

public class MagicSheath : MagicSheathBase
{
    public override int SheathLevel => 1;

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;
        Item.accessory = true;

        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(silver: 20);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.ManaCrystal, 5)
            .AddIngredient(
                ModContent.Find<ModItem>(
                    "ThoriumMod",
                    "ThoriumBar"
                ).Type,
                5
            )
            .AddTile(TileID.Anvils)
            .Register();
    }
}