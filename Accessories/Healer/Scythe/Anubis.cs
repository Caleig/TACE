using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Healer.Scythe;

public class Anubis : ScytheAccessoryItem
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
            .Anubis = true;

        player.GetDamage(
            ThoriumMod.HealerDamage.Instance
        ) += 15f;
    }
    public override void AddRecipes()
    {
        Mod thorium =
            ModLoader.GetMod("ThoriumMod");

        int pharaohsBreathType =
            thorium.Find<ModItem>("PharaohsBreath").Type;

        CreateRecipe()
            .AddIngredient<SpiritFireScythe>()
            .AddIngredient(ItemID.AncientBattleArmorMaterial, 1)
            .AddIngredient(pharaohsBreathType, 20)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}