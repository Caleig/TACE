using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Healer.Scythe;

public class SoulOfScythe : ScytheAccessoryItem
{
    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.width = 32;
        Item.height = 32;

        Item.accessory = true;
        Item.rare = ItemRarityID.LightRed;
    }

    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        player.GetModPlayer<SoulOfScythePlayer>()
            .SoulOfScythe = true;
    }
    public override void AddRecipes()
    {
        Mod thorium =
            ModLoader.GetMod("ThoriumMod");

        int unholyShardsType =
            thorium.Find<ModItem>("UnholyShards").Type;

        int purifiedShardsType =
            thorium.Find<ModItem>("PurifiedShards").Type;

        CreateRecipe()
            .AddIngredient(unholyShardsType, 5)
            .AddIngredient(purifiedShardsType, 5)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}