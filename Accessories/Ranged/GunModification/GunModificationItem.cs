using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace ThoriumAccessoryExpansion.Accessories.Ranged.GunModification;

public abstract class GunModificationItem : ThoriumItem
{
    public override bool CanEquipAccessory(
        Player player,
        int slot,
        bool modded)
    {
        for (
            int i = 3;
            i < player.armor.Length;
            i++
        )
        {
            if (i == slot)
                continue;

            if (
                player.armor[i].ModItem
                is GunModificationItem
            )
            {
                return false;
            }
        }

        return true;
    }
}