using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Summon.GemContract;

public class TopazContract : GemContractBase
{
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.accessory = true;
        Item.value = Item.sellPrice(gold: 5);
        Item.rare = ItemRarityID.Orange;
    }


    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        GemContractPlayer contract =
            player.GetModPlayer<GemContractPlayer>();

        contract.topazContract = true;
        contract.magicContractActive = true;
    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Topaz, 8)
            .AddIngredient(ItemID.TinBar, 7)
            .AddIngredient(ItemID.LargeTopaz, 1)
            .AddTile(TileID.Anvils)
            .Register();
    }
}