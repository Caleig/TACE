using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath
{
    public class MagicSheath : ModItem
    {
        public override void SetStaticDefaults()
        { }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 20);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ManaCrystal, 5)
                .AddIngredient(ModContent.Find<ModItem>("ThoriumMod", "ThoriumBar").Type, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
            {
                if (player.armor[i].type == ModContent.ItemType<SpiritMagicSheath>() || player.armor[i].type == ModContent.ItemType<TerraMagicSheath>())
                    return false;
            }
            return true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var mp = player.GetModPlayer<MagicSheathPlayer>();
            mp.SheathLevel = Math.Max(mp.SheathLevel, 1);
        }
    }
}