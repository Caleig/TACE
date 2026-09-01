using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath;

public abstract class MagicSwordEnhancementItem : ModItem
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
                is MagicSwordEnhancementItem
            )
            {
                return false;
            }
        }

        return true;
    }
}