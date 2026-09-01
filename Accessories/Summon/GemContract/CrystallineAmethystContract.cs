using Terraria;
using Terraria.ID;
using ThoriumAccessoryExpansion.Players;


namespace ThoriumAccessoryExpansion.Accessories.Summon.GemContract
{

    public class CrystallineAmethystContract : GemContractBase
    {

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.accessory = true;

            Item.rare =
                ItemRarityID.Lime;

            Item.value =
                Item.buyPrice(gold: 10);
        }


        public override void UpdateAccessory(
            Player player,
            bool hideVisual)
        {

            GemContractPlayer contract =
                player.GetModPlayer<GemContractPlayer>();


            contract.crystallineAmethystContract = true;

            contract.magicContractActive = true;

        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<AmethystContract>()
                .AddIngredient(ItemID.LargeAmethyst, 2)
                .AddIngredient(ItemID.CrystalShard, 50)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

}