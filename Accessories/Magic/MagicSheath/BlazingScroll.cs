using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath;

public class BlazingScroll : MagicSwordEnhancementItem
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
            .HasBlazingScroll = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(
                ItemID.HellstoneBar,
                10
            )
            .AddIngredient(
                ItemID.Silk,
                5
            )
            .AddTile(
                TileID.Loom
            )
            .Register();
    }
}