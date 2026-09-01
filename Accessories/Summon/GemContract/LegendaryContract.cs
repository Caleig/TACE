using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;


namespace ThoriumAccessoryExpansion.Accessories.Summon.GemContract;


public class LegendaryContract : GemContractBase
{

    public override void SetDefaults()
    {

        Item.width = 32;
        Item.height = 32;


        Item.accessory = true;


        Item.value =
            Item.sellPrice(gold: 15);


        Item.rare =
            ItemRarityID.Lime;

    }



    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {

        GemContractPlayer contract =
            player.GetModPlayer<GemContractPlayer>();


        contract.legendaryContract = true;


        contract.magicContractActive = true;

        player.whipRangeMultiplier +=
            0.10f;


        player.maxMinions +=
            1;


        player.maxTurrets +=
            1;

    }
    public override void AddRecipes()
    {
        Mod thorium = ModLoader.GetMod("ThoriumMod");

        int largeOpalType =
            thorium.Find<ModItem>("LargeOpal").Type;

        int largeAquamarineType =
            thorium.Find<ModItem>("LargeAquamarine").Type;

        int largePrismiteType =
            thorium.Find<ModItem>("LargePrismite").Type;

        int titanicBarType =
            thorium.Find<ModItem>("TitanicBar").Type;

        int concentratedThoriumType =
            thorium.Find<ModItem>("ConcentratedThorium").Type;

        CreateRecipe()
            .AddIngredient(ItemID.LargeAmethyst, 1)
            .AddIngredient(ItemID.LargeTopaz, 1)
            .AddIngredient(ItemID.LargeSapphire, 1)
            .AddIngredient(ItemID.LargeEmerald, 1)
            .AddIngredient(ItemID.LargeAmber, 1)
            .AddIngredient(ItemID.LargeRuby, 1)
            .AddIngredient(ItemID.LargeDiamond, 1)
            .AddIngredient(largeOpalType, 1)
            .AddIngredient(largeAquamarineType, 1)
            .AddIngredient(largePrismiteType, 1)
            .AddIngredient(titanicBarType, 5)
            .AddIngredient(concentratedThoriumType, 5)
            .AddIngredient(ItemID.CrystalShard, 100)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}