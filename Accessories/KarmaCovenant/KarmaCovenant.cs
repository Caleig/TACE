using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.FallenCovenant;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Items.BardItems;

namespace ThoriumAccessoryExpansion.Accessories.KarmaCovenant
{
    public class KarmaCovenant : ModItem
    {
     

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(gold: 5);
            Item.defense = 3;
        }

        public override void AddRecipes()
        {
            int fallenCovenantType = ModContent.ItemType<FallenCovenant.FallenCovenant>();
            int darkHeartType = ModContent.Find<ModItem>("ThoriumMod", "DarkHeart")?.Type ?? ItemID.DirtBlock;

            CreateRecipe()
                .AddIngredient(fallenCovenantType, 1)
                .AddIngredient(darkHeartType, 1)
                .AddIngredient(ModContent.ItemType<BloomWeave>(), 10)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            thoriumPlayer.healBonus -= 1;
            player.GetModPlayer<ThoriumPlayer>().darkIntent = true;
            player.GetModPlayer<ThoriumPlayer>().darkAura = true;
            player.GetModPlayer<CovenantPlayer>().KarmaHasCovenant = true;
        }
    }
}