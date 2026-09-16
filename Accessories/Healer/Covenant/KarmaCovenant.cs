using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Projectiles.Healer;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Accessories.Healer.Covenant;

public class KarmaCovenant : CovenantAccessoryItem
{
    private const int KarmicLifeStep = 100;
    private const int KarmicLifeMax = 500;
    private const float KarmicDamagePerStep = 0.08f;

    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.width = 28;
        Item.height = 28;
        Item.accessory = true;
        Item.rare = ItemRarityID.Cyan;
        Item.value = Item.sellPrice(gold: 5);
        Item.defense = 3;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<FallenCovenant>()
            .AddIngredient<KarmicHolder>()
            .AddIngredient<BloomWeave>(10)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }

    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        ThoriumPlayer thoriumPlayer =
            player.GetThoriumPlayer();

        thoriumPlayer.healBonus -= 1;
        thoriumPlayer.darkIntent = true;
        thoriumPlayer.darkAura = true;
        thoriumPlayer.karmicHolder = true;

        int healStreak =
            Math.Min(
                Math.Max(
                    thoriumPlayer.healStreak,
                    0
                ),
                KarmicLifeMax
            );

        int crucibleSteps =
            healStreak / KarmicLifeStep;

        if (crucibleSteps > 0)
        {
            player.GetDamage(
                ModContent.GetInstance<HealerDamage>()
            ) +=
                crucibleSteps *
                KarmicDamagePerStep;
        }

        CovenantPlayer cp =
            player.GetModPlayer<CovenantPlayer>();

        cp.KarmaHasCovenant = true;

        if (
            cp.FallenRadianceStacks >=
            CovenantPlayer.GlobalMaxStacks
        )
        {
            player.GetDamage(
                ModContent.GetInstance<HealerDamage>()
            ) += 0.15f;
        }

        if (
            player.whoAmI == Main.myPlayer &&
            thoriumPlayer.healStreak >= 0
        )
        {
            int projectileType =
                ModContent.ProjectileType<KarmicHolderPro>();

            if (
                player.ownedProjectileCounts[
                    projectileType
                ] < 1
            )
            {
                Projectile.NewProjectile(
                    player.GetSource_Accessory(Item),
                    player.Center,
                    Vector2.Zero,
                    projectileType,
                    0,
                    0f,
                    player.whoAmI
                );
            }
        }
    }
}