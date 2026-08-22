using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Items.BasicAccessories;
using ThoriumMod.Items.HealerItems;

namespace ThoriumAccessoryExpansion.Accessories.Healer.MichaelasAid
{
    public class MichaelasAid : ModItem
    {
     

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ArchangelHeart>(), 1)
                .AddIngredient(ItemID.FragmentSolar, 5)
                .AddIngredient(ModContent.ItemType<CelestialFragment>(), 5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 10;
            player.statManaMax2 += 10;

            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            thoriumPlayer.healBonus += 2;

            player.GetModPlayer<CovenantPlayer>().MichaelasHasCovenant = true;
        }
    }
}