using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.Summon.SoulMachineSigil;

public class SoulMachineSigil : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;

        Item.accessory = true;

        Item.rare = ItemRarityID.Orange;
        Item.value = Item.sellPrice(gold: 5);
    }

    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        player.whipRangeMultiplier += 0.15f;
        player.maxTurrets += 1;
        player.maxMinions =
            Math.Max(
                0,
                player.maxMinions - 1
            );
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
                ItemID.ApprenticeScarf
            )
            .AddIngredient(
                spiritDropletType,
                8
            )
            .AddTile(
                TileID.TinkerersWorkbench
            )
            .Register();
        CreateRecipe()
            .AddIngredient(
                ItemID.SquireShield
            )
            .AddIngredient(
                spiritDropletType,
                8
            )
            .AddTile(
                TileID.TinkerersWorkbench
            )
            .Register();
    }
}