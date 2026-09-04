using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.BossBuriedChampion;

namespace ThoriumAccessoryExpansion.Accessories.Warrior
{
    public abstract class WarriorShieldBase : ModItem
    {
        public override bool CanEquipAccessory(
            Player player,
            int slot,
            bool modded)
        {
            for (int i = 3; i < player.armor.Length; i++)
            {
                if (i == slot)
                    continue;

                ModItem equippedItem = player.armor[i].ModItem;
                if (equippedItem is WarriorShieldBase)
                {
                    return false;
                }
                if (equippedItem is ChampionsRebuttal)
                {
                    return false;
                }
            }

            return true;
        }
    }
}