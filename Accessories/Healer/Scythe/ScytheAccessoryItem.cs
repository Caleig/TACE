using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace ThoriumAccessoryExpansion.Accessories.Healer.Scythe;

public abstract class ScytheAccessoryItem : ThoriumItem
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
                is ScytheAccessoryItem
            )
            {
                return false;
            }
        }

        return true;
    }

    public override void SetDefaults()
    {
        isHealer = true;
    }
}