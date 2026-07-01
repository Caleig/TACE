using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;

namespace ThoriumAccessoryExpansion.Accessories.CursedCovenant
{
    public class CursedCovenant : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 20);
        }

        public override void AddRecipes()
        {
            int unholyShardType = ModContent.ItemType<UnholyShards>();

            CreateRecipe()
                .AddIngredient(ItemID.RottenChunk, 10)
                .AddIngredient(unholyShardType, 10)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            CreateRecipe()
               .AddIngredient(ItemID.Vertebrae, 10)
               .AddIngredient(unholyShardType, 10)
               .AddTile(TileID.TinkerersWorkbench)
               .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ThoriumPlayer>().darkIntent = true;
            player.GetModPlayer<ThoriumPlayer>().darkAura = true;

            player.GetModPlayer<CovenantPlayer>().CursedHasCovenant = true;
        }
    }
}