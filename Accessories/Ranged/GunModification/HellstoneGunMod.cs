using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Ranged.GunModification;

public class HellstoneGunMod
    : GunModificationItem
{
    public override void SetDefaults()
    {
        Item.width = 34;
        Item.height = 24;
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

        modification.HasHellstoneGunMod = true;

        player.GetAttackSpeed(
            DamageClass.Ranged
        ) += 0.05f;

        player.GetCritChance(
            DamageClass.Ranged
        ) += 5f;
    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.HellstoneBar, 15)
            .AddIngredient(ItemID.IllegalGunParts, 1)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}