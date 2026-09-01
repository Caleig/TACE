using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;


namespace ThoriumAccessoryExpansion.Accessories.Summon.GemContract;


public class CrystallineAquamarineContract : GemContractBase
{

    public override void SetDefaults()
    {

        Item.width = 32;
        Item.height = 32;


        Item.accessory = true;


        Item.value =
            Item.sellPrice(gold: 10);


        Item.rare =
            ItemRarityID.LightRed;

    }



    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {

        GemContractPlayer contract =
            player.GetModPlayer<GemContractPlayer>();


        contract.crystallineAquamarineContract = true;


        contract.magicContractActive = true;

    }
    public override void AddRecipes()
    {
        Mod thorium = ModLoader.GetMod("ThoriumMod");

        int largeAquamarineType =
            thorium.Find<ModItem>("LargeAquamarine").Type;

        CreateRecipe()
            .AddIngredient<AquamarineContract>()
            .AddIngredient(largeAquamarineType, 2)
            .AddIngredient(ItemID.CrystalShard, 50)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}