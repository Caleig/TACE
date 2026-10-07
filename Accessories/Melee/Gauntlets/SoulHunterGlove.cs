using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.Melee.SoulHunterGlove;

public class SoulHunterGlove : ModItem
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
        player.GetModPlayer<
            Players.MeleeGauntletPlayer
        >().HasSoulHunterGlove = true;
        player.statDefense += 1;
    }

    public override void AddRecipes()
    {
        Mod thorium =
            ModLoader.GetMod("ThoriumMod");

        int spiritDropletType =
            thorium.Find<ModItem>(
                "SpiritDroplet"
            ).Type;

        CreateRecipe()
            .AddIngredient(
                ItemID.Leather,
                5
            )
            .AddIngredient(
                spiritDropletType,
                5
            )
            .AddTile(
                TileID.TinkerersWorkbench
            )
            .Register();
    }
}