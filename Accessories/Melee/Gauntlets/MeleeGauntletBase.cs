using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.Melee.Gauntlets;

public abstract class MeleeGauntletBase : ModItem
{
    public override bool CanEquipAccessory(
        Player player,
        int slot,
        bool modded)
    {
        for (int i = 3; i < player.armor.Length; i++)
        {
            if (i == slot)
            {
                continue;
            }

            if (player.armor[i].ModItem is MeleeGauntletBase)
            {
                return false;
            }
        }

        return true;
    }
}