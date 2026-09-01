using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Summon.GemContract;

public class OpalContract : GemContractBase
{

    public override void SetDefaults()
    {

        Item.width = 32;
        Item.height = 32;


        Item.accessory = true;


        Item.value =
            Item.sellPrice(gold: 5);


        Item.rare =
            ItemRarityID.Orange;

    }



    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {

        GemContractPlayer contract =
            player.GetModPlayer<GemContractPlayer>();


        contract.opalContract = true;


        contract.magicContractActive = true;

    }
    public override void AddRecipes()
    {
        Mod thorium = ModLoader.GetMod("ThoriumMod");

        int opalType =
            thorium.Find<ModItem>("Opal").Type;

        int largeOpalType =
            thorium.Find<ModItem>("LargeOpal").Type;

        CreateRecipe()
            .AddIngredient(opalType,8)
            .AddIngredient(ItemID.IronBar,10)
            .AddIngredient(largeOpalType,1)
            .AddTile(TileID.Anvils)
            .Register();
    }
}