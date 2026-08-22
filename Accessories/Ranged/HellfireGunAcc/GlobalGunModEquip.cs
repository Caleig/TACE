using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.Ranged.HellfireGunAcc;

public class GlobalGunModEquip : GlobalItem
{
    private static bool IsGunModAccessory(Item item)
    {
        if (item == null || item.ModItem == null)
            return false;

        return item.ModItem is HellstoneGunMod
            || item.ModItem is GreenDragonGunMod
            || item.ModItem is FleshGunMod
            || item.ModItem is FleshTrigger
            || item.ModItem is TitanGunMod;
    }

    public override bool CanAccessoryBeEquippedWith(Item selectedItem, Item equippedItem, Player player)
    {
        if (IsGunModAccessory(selectedItem) && IsGunModAccessory(equippedItem))
        {
            return false;
        }
        return base.CanAccessoryBeEquippedWith(selectedItem, equippedItem, player);
    }
}
