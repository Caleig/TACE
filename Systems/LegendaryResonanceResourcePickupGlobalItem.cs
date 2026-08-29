using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.NPCs;
using ThoriumAccessoryExpansion.Players;


namespace ThoriumAccessoryExpansion.Systems;


public class LegendaryResonanceResourcePickupGlobalItem :
    GlobalItem
{

    public override void GrabRange(
        Item item,
        Player player,
        ref int grabRange)
    {

        GemContractPlayer contract =
            player.GetModPlayer<
                GemContractPlayer
            >();



        int stacks =
            contract.GetLegendaryResonanceStacks(
                GemType.Aquamarine
            );



        if (stacks <= 0)
            return;



        if (!IsResourcePickup(item))
            return;



        grabRange =
            (int)(
                grabRange
                *
                (
                    1f
                    +
                    stacks * 0.05f
                )
            );

    }



    private static bool IsResourcePickup(
        Item item)
    {

        switch (item.type)
        {

            case ItemID.Heart:
            case ItemID.Star:

            case ItemID.CopperCoin:
            case ItemID.SilverCoin:
            case ItemID.GoldCoin:
            case ItemID.PlatinumCoin:

                return true;

        }



        if (
            item.ModItem != null
            &&
            item.ModItem.Mod.Name
                == "ThoriumMod"
            &&
            item.ModItem.Name.Contains(
                "InspirationNote"
            )
        )
        {
            return true;
        }



        return false;

    }

}