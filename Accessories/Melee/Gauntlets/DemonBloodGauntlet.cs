using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Melee.Gauntlets;

public class DemonBloodGauntlet : ModItem
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

        gauntlet.HasDemonBloodGauntlet = true;

        player.statDefense += 9;
        player.endurance += 0.05f;

        player.GetKnockback(DamageClass.Melee) += 1f;
        player.GetAttackSpeed(DamageClass.Melee) += 0.15f;
        player.autoReuseGlove = true;
        player.meleeScaleGlove = true;
        player.aggro += 400;
    }

    public override void AddRecipes()
    {
        Mod thorium =
            ModLoader.GetMod("ThoriumMod");

        int demonBloodShardType =
            thorium.Find<ModItem>(
                "DemonBloodShard"
            ).Type;

        CreateRecipe()
            .AddIngredient(
                ItemID.BerserkerGlove
            )
            .AddIngredient(
                demonBloodShardType,
                10
            )
            .AddTile(
                TileID.TinkerersWorkbench
            )
            .Register();
    }
}