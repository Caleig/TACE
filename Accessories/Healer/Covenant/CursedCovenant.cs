using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;

namespace ThoriumAccessoryExpansion.Accessories.Healer.Covenant;

public class CursedCovenant : CovenantAccessoryItem
{
    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.width = 24;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(silver: 20);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.RottenChunk, 10)
            .AddIngredient<UnholyShards>(10)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();

        CreateRecipe()
            .AddIngredient(ItemID.Vertebrae, 10)
            .AddIngredient<UnholyShards>(10)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }

    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        ThoriumPlayer thoriumPlayer =
            player.GetModPlayer<ThoriumPlayer>();

        thoriumPlayer.darkIntent = true;
        thoriumPlayer.darkAura = true;

        player.GetModPlayer<CovenantPlayer>()
            .CursedHasCovenant = true;
    }
}