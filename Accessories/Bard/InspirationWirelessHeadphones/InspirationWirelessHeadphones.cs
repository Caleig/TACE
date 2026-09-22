using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Accessories.Bard.InspirationWirelessHeadphones;

public class InspirationWirelessHeadphones : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;
        Item.accessory = true;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(gold: 1);
    }

    public override void AddRecipes()
    {
        Mod thorium =
            ModLoader.GetMod("ThoriumMod");

        CreateRecipe()
            .AddIngredient(
                thorium.Find<ModItem>("Headset").Type
            )
            .AddIngredient(
                thorium.Find<ModItem>("InspirationFragment").Type,
                5
            )
            .AddIngredient(
                thorium.Find<ModItem>("BloomWeave").Type,
                5
            )
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }

    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        player.GetThoriumPlayer()
            .bardResourceMax2 += 5;

        player.GetModPlayer<BardAccessoryPlayer>()
            .HasInspirationWirelessHeadphones = true;
    }
}