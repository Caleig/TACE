using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.Misc;

namespace ThoriumAccessoryExpansion.Accessories.MagicSheath
{
    public class TerraMagicSheath : ModItem
    {
        public override void SetStaticDefaults()
        {   }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.sellPrice(gold: 10);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SpiritMagicSheath>(), 1)
                .AddIngredient(ItemID.AvengerEmblem, 1)
                .AddIngredient(ModContent.ItemType<BrokenHeroFragment>(), 3) // 残缺的英雄碎片（实际是断剑？）
                .AddIngredient(ItemID.Ectoplasm, 10) // 灵气
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var mp = player.GetModPlayer<MagicSheathPlayer>();
            mp.SheathLevel = Math.Max(mp.SheathLevel, 3);
        }
    }
}