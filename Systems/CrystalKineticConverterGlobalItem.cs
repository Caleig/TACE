using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Systems;

public class CrystalKineticConverterGlobalItem
    : GlobalItem
{
    public override void OnHitNPC(
        Item item,
        Player player,
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {
        if (
            Main.netMode ==
            NetmodeID.MultiplayerClient
        )
        {
            return;
        }

        if (
            item.DamageType !=
            ModContent.GetInstance<BardDamage>()
        )
        {
            return;
        }

        if (damageDone <= 0)
            return;

        player.GetModPlayer<
            CrystalKineticConverterPlayer
        >().AddSonicDamage(damageDone);
    }
}