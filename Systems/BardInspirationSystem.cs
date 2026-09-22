using MonoMod.RuntimeDetour;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Buffs;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Systems;

public class BardInspirationSystem : ModSystem
{
    private delegate bool ConsumeInspirationDelegate(
        Player player,
        int cost,
        bool pay
    );

    private Hook _consumeInspirationHook;

    public override void Load()
    {
        MethodInfo method =
            typeof(BardItem).GetMethod(
                "ConsumeInspiration",
                BindingFlags.Public |
                BindingFlags.Static
            );

        if (method == null)
            return;

        _consumeInspirationHook =
            new Hook(
                method,
                (System.Func<
                    ConsumeInspirationDelegate,
                    Player,
                    int,
                    bool,
                    bool
                >)OnConsumeInspiration
            );
    }

    public override void Unload()
    {
        _consumeInspirationHook?.Dispose();
        _consumeInspirationHook = null;
    }

    private static bool OnConsumeInspiration(
        ConsumeInspirationDelegate orig,
        Player player,
        int cost,
        bool pay
    )
    {
        BardAccessoryPlayer accessoryPlayer =
            player.GetModPlayer<BardAccessoryPlayer>();

        if (
            accessoryPlayer.HasInspirationWirelessHeadphones &&
            player.HasBuff(
                ModContent.BuffType<InspirationOverflowBuff>()
            ) &&
            cost > 0
        )
        {
            return true;
        }

        ThoriumPlayer thoriumPlayer =
            player.GetThoriumPlayer();

        int before =
            thoriumPlayer.bardResource;

        bool result =
            orig(
                player,
                cost,
                pay
            );

        if (
            result &&
            pay &&
            accessoryPlayer.HasInspirationWirelessHeadphones &&
            cost > 0
        )
        {
            int after =
                thoriumPlayer.bardResource;

            int consumed =
                before - after;

            if (consumed > 0)
            {
                accessoryPlayer.AddInspirationSpent(
                    consumed
                );
            }
        }

        return result;
    }
}