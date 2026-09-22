using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Systems;

public class CrystalKineticConverterGlobalProjectile
    : GlobalProjectile
{
    public override void OnHitNPC(
        Projectile projectile,
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
            projectile.owner < 0 ||
            projectile.owner >= Main.maxPlayers
        )
        {
            return;
        }

        if (damageDone <= 0)
            return;

        if (
            projectile.DamageType !=
            ModContent.GetInstance<BardDamage>()
        )
        {
            return;
        }

        Player player =
            Main.player[projectile.owner];

        if (!player.active)
            return;

        player.GetModPlayer<
            CrystalKineticConverterPlayer
        >().AddSonicDamage(damageDone);
    }
}