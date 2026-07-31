using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.MagicItems;

namespace ThoriumAccessoryExpansion.Accessories.MagicSheath
{
    public class SpiritMagicSheath : ModItem
    {
        public override void SetStaticDefaults()
        {      }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SpiritBlade>(), 1)
                .AddIngredient(ModContent.ItemType<MagicSheath>(), 1)
                .AddIngredient(ModContent.ItemType<HallowedCharm>(), 5) // 神圣护身符（实际ID？可用其他）
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var mp = player.GetModPlayer<MagicSheathPlayer>();
            player.GetDamage(DamageClass.Magic).Flat -= 3; // 增加15%魔法伤害)
            mp.SheathLevel = Math.Max(mp.SheathLevel, 2);
        }
    }
}