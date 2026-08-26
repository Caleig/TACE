using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.Summon.GemContract
{

    public abstract class GemContractBase : ModItem
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


                if (player.armor[i].ModItem is GemContractBase)
                {
                    return false;
                }

            }


            return true;

        }

    }

}