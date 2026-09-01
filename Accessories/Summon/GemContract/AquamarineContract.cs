using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.Healer.CursedCovenant;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;


namespace ThoriumAccessoryExpansion.Accessories.Summon.GemContract;


public class AquamarineContract : GemContractBase
{

    public override void SetDefaults()
    {

        Item.width = 32;
        Item.height = 32;


        Item.accessory = true;


        Item.value =
            Item.sellPrice(gold: 5);


        Item.rare =
            ItemRarityID.LightRed;

    }



    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {

        GemContractPlayer contract =
            player.GetModPlayer<GemContractPlayer>();


        contract.aquamarineContract = true;


        contract.magicContractActive = true;

    }
    public override void AddRecipes()
    {
        Mod thorium = ModLoader.GetMod("ThoriumMod");

        int aquamarineType =
            thorium.Find<ModItem>("Aquamarine").Type;

        int largeAquamarineType =
            thorium.Find<ModItem>("LargeAquamarine").Type;

        CreateRecipe()
            .AddIngredient(aquamarineType, 8)
            .AddIngredient(ItemID.LeadBar,10)
            .AddIngredient(largeAquamarineType, 1)
            .AddTile(TileID.Anvils)
            .Register();
    }
}