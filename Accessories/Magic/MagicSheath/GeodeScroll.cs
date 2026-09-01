using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod.Items.Lodestone;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath;

public class GeodeScroll : MagicSwordEnhancementItem
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
            .HasGeodeScroll = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(
                ModContent.ItemType<LodeStoneIngot>(),
                10
            )
            .AddIngredient(
                ItemID.Silk,
                5
            )
            .AddIngredient(
                ItemID.SoulofMight,
                1
            )
            .AddIngredient(
                ItemID.SoulofSight,
                1
            )
            .AddIngredient(
                ItemID.SoulofFright,
                1
            )
            .AddTile(
                TileID.Loom
            )
            .Register();
    }
}