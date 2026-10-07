using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Melee.Gauntlets;

public class TitanBracer : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;

        Item.accessory = true;
    }

    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        MeleeGauntletPlayer gauntlet =
            player.GetModPlayer<MeleeGauntletPlayer>();

        gauntlet.HasTitanBracer = true;

        player.statDefense += 3;
    }

    public override void AddRecipes()
    {
        Mod thorium =
            ModLoader.GetMod("ThoriumMod");

        int titanicBarType =
            thorium.Find<ModItem>(
                "TitanicBar"
            ).Type;

        int arcaneDustType =
            thorium.Find<ModItem>(
                "ArcaneDust"
            ).Type;

        CreateRecipe()
            .AddIngredient(
                ModContent.ItemType<SoulHunterGlove>()
            )
            .AddIngredient(
                titanicBarType,
                5
            )
            .AddIngredient(
                arcaneDustType,
                10
            )
            .AddTile(
                TileID.TinkerersWorkbench
            )
            .Register();
    }
}