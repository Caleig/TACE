using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.MagicItems;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath
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
                .AddIngredient(ModContent.ItemType<HallowedCharm>(), 5) 
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
            {
                if (player.armor[i].type == ModContent.ItemType<MagicSheath>() || player.armor[i].type == ModContent.ItemType<TerraMagicSheath>())
                    return false;
            }
            return true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var mp = player.GetModPlayer<MagicSheathPlayer>();
            player.GetDamage(DamageClass.Magic).Flat -= 3; 
            mp.SheathLevel = Math.Max(mp.SheathLevel, 2);
        }
    }
}