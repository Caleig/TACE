using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Healer.Scythe;

public class StyxBloodAnkh : ScytheAccessoryItem
{
    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.width = 32;
        Item.height = 32;

        Item.accessory = true;
        Item.rare = ItemRarityID.Lime;
    }

    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        player.GetModPlayer<SoulOfScythePlayer>()
            .StyxBloodAnkh = true;
    }
    public override void AddRecipes()
    {
        Mod thorium =
            ModLoader.GetMod("ThoriumMod");

        int demonBloodShardType =
            thorium.Find<ModItem>("DemonBloodShard").Type;

        CreateRecipe()
            .AddIngredient<Anubis>()
            .AddIngredient(demonBloodShardType, 10)
            .AddIngredient(ItemID.SoulofNight, 10)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}