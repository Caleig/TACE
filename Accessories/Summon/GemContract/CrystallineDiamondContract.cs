using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;


namespace ThoriumAccessoryExpansion.Accessories.Summon.GemContract;


public class CrystallineDiamondContract : GemContractBase
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


        contract.crystallineDiamondContract = true;


        contract.magicContractActive = true;

    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<DiamondContract>()
            .AddIngredient(ItemID.LargeDiamond, 2)
            .AddIngredient(ItemID.CrystalShard, 50)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}