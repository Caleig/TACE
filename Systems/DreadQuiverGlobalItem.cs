using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Systems;

public class DreadQuiverGlobalItem
    : GlobalItem
{
    public override bool AppliesToEntity(
        Item entity,
        bool lateInstantiation)
    {
        return entity.ammo == AmmoID.Arrow;
    }


    public override bool CanBeConsumedAsAmmo(
        Item weapon,
        Item ammo,
        Player player)
    {
        GunModificationPlayer modification =
            player.GetModPlayer<
                GunModificationPlayer
            >();


        if (
            modification.HasDreadQuiver &&
            Main.rand.NextFloat() < 0.25f
        )
        {
            return false;
        }


        return base.CanBeConsumedAsAmmo(
            weapon,
            ammo,
            player
        );
    }
}