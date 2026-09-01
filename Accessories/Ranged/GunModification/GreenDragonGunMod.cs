using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Ranged.GunModification;

public class GreenDragonGunMod
    : GunModificationItem
{
    public override void SetDefaults()
    {
        Item.width = 40;
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

        modification.HasGreenDragonGunMod = true;

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

        int greenDragonScaleType =
            thorium.Find<ModItem>("GreenDragonScale").Type;

        CreateRecipe()
            .AddIngredient(ModContent.ItemType<HellstoneGunMod>(), 1)
            .AddIngredient(greenDragonScaleType, 15)
            .AddIngredient(ItemID.CursedFlame, 10)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}