using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Items.Donate;
using ThoriumMod.Items.Terrarium;

namespace ThoriumAccessoryExpansion.Accessories.EternalBulwark
{
    public class EternalBulwark : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(gold: 5);
            Item.defense = 12;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<LifeQuartzShield>(), 1)
                .AddIngredient(ModContent.ItemType<TerrariumCore>(), 5)
                .AddIngredient(ItemID.HeroShield, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noKnockback = true;
            player.lifeRegen += 3;

            player.GetModPlayer<CovenantPlayer>().EternalHasCovenant = true;
        }
    }
}