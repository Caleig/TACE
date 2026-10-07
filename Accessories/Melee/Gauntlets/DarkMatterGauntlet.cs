using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Melee.Gauntlets;

public class DarkMatterGauntlet : MeleeGauntletBase
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

        gauntlet.HasDarkMatterGauntlet = true;

        player.GetKnockback(DamageClass.Melee) += 1f;
        player.GetDamage(DamageClass.Melee) += 0.15f;

        int tier =
            gauntlet.GetKineticTier();
        player.GetAttackSpeed(DamageClass.Melee) +=
            0.15f + tier * 0.03f;

        player.autoReuseGlove = true;
        player.meleeScaleGlove = true;
        if (gauntlet.DarkMatterKinetic >= 100)
        {
            player.GetDamage(DamageClass.Melee) += 0.15f;
        }
    }

    public override void AddRecipes()
    {
        Mod thorium =
            ModLoader.GetMod("ThoriumMod");

        int darkMatterType =
            thorium.Find<ModItem>(
                "DarkMatter"
            ).Type;

        CreateRecipe()
            .AddIngredient(ItemID.FireGauntlet)
            .AddIngredient(darkMatterType, 5)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}