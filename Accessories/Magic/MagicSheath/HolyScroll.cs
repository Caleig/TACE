using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath;

public class HolyScroll : MagicSwordEnhancementItem
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
            .HasHolyScroll = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(
                ItemID.HallowedBar,
                10
            )
            .AddIngredient(
                ItemID.ChlorophyteBar,
                10
            )
            .AddIngredient(
                ModContent.Find<ModItem>(
                    "ThoriumMod",
                    "BloomWeave"
                ).Type,
                5
            )
            .AddIngredient(
                ItemID.BrokenHeroSword,
                1
            )
            .AddTile(
                TileID.Loom
            )
            .Register();
    }
}