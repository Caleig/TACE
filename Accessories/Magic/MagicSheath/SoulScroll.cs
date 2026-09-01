using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath;

public class SoulScroll : MagicSwordEnhancementItem
{
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;

        Item.accessory = true;

        Item.rare =
            ItemRarityID.LightRed;

        Item.value =
            Item.sellPrice(silver: 50);
    }

    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        player
            .GetModPlayer<
                MagicSwordEnhancementPlayer
            >()
            .HasSoulScroll = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(
                ItemID.Ectoplasm,
                10
            )
            .AddIngredient(
                ModContent.Find<ModItem>(
                    "ThoriumMod",
                    "CursedCloth"
                ).Type,
                5
            )
            .AddTile(
                TileID.Loom
            )
            .Register();
    }
}