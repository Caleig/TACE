using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Healer.Scythe;

public class RadiantHolyAnkh : ScytheAccessoryItem
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
            .RadiantHolyAnkh = true;
    }
    public override void AddRecipes()
    {
        Mod thorium =
            ModLoader.GetMod("ThoriumMod");

        int holyKnightsAlloyType =
            thorium.Find<ModItem>("HolyKnightsAlloy").Type;

        CreateRecipe()
            .AddIngredient<Anubis>()
            .AddIngredient(holyKnightsAlloyType, 5)
            .AddIngredient(ItemID.SoulofLight, 10)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}