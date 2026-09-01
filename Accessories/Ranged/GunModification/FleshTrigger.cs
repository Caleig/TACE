using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Ranged.GunModification;

public class FleshTrigger
    : GunModificationItem
{
    public override void SetDefaults()
    {
        Item.width = 54;
        Item.height = 34;
        Item.accessory = true;
    }


    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        GunModificationPlayer modification =
            player.GetModPlayer<
                GunModificationPlayer
            >();

        modification.HasFleshTrigger = true;

        player.GetAttackSpeed(
            DamageClass.Ranged
        ) += 0.20f;
    }
    public override void AddRecipes()
    {
        Mod thorium = ModLoader.GetMod("ThoriumMod");

        int demonBloodShardType =
            thorium.Find<ModItem>("DemonBloodShard").Type;
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<FleshGunMod>(), 1)
            .AddIngredient(demonBloodShardType, 10)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<GreenDragonGunMod>(), 1)
            .AddIngredient(demonBloodShardType, 10)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}