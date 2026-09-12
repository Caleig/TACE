using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Healer.Scythe;

public class SpiritFireScythe : ScytheAccessoryItem
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
            .SpiritFireScythe = true;

        player.GetDamage(
            ThoriumMod.HealerDamage.Instance
        ) += 15f;
    }
    public override void AddRecipes()
    {
        Mod thorium =
            ModLoader.GetMod("ThoriumMod");

        int innerFlameType =
            thorium.Find<ModItem>("InnerFlame").Type;

        int darksteelAlloyType =
            thorium.Find<ModItem>("aDarksteelAlloy").Type;

        CreateRecipe()
            .AddIngredient<SoulOfScythe>()
            .AddIngredient(innerFlameType)
            .AddIngredient(darksteelAlloyType, 10)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}