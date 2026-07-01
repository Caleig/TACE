using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.MagicItems;
using ThoriumMod.Items.NPCItems;

namespace ThoriumAccessoryExpansion.Accessories.HeresyCovenant
{
    public class HeresyCovenant : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<DemonTongue>(), 1)
                .AddIngredient(ModContent.ItemType<DarkEffigy>(), 1)
                .AddIngredient(ModContent.ItemType<DarkIntent>(), 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            thoriumPlayer.healBonus -= 1;
            player.GetModPlayer<ThoriumPlayer>().darkIntent = true;
            player.GetModPlayer<ThoriumPlayer>().darkAura = true;
            player.aggro += 400; // 与皇家凝胶相反的效果  
            player.GetModPlayer<CovenantPlayer>().HeresyHasCovenant = true;
        }
    }
}