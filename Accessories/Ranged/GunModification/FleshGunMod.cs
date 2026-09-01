using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Ranged.GunModification;

public class FleshGunMod
    : GunModificationItem
{
    public override void SetDefaults()
    {
        Item.width = 42;
        Item.height = 28;
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

        modification.HasFleshGunMod = true;

        player.GetAttackSpeed(
            DamageClass.Ranged
        ) += 0.08f;

        player.GetCritChance(
            DamageClass.Ranged
        ) += 12f;
    }
    public override void AddRecipes()
    {
        Mod thorium = ModLoader.GetMod("ThoriumMod");

        int unfathomableFleshType =
            thorium.Find<ModItem>("UnfathomableFlesh").Type;

        CreateRecipe()
            .AddIngredient(ModContent.ItemType<HellstoneGunMod>(), 1)
            .AddIngredient(unfathomableFleshType, 15)
            .AddIngredient(ItemID.Ichor, 10)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}