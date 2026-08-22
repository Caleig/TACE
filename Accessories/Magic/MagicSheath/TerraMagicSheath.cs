using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.Misc;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath
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
                .AddIngredient(ModContent.ItemType<BrokenHeroFragment>(), 3) 
                .AddIngredient(ItemID.Ectoplasm, 10) 
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
            {
                if (player.armor[i].type == ModContent.ItemType<SpiritMagicSheath>() || player.armor[i].type == ModContent.ItemType<MagicSheath>())
                    return false;
            }
            return true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.15f;
            var mp = player.GetModPlayer<MagicSheathPlayer>();
            mp.SheathLevel = Math.Max(mp.SheathLevel, 3);
        }
    }
}