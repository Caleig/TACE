using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;
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
            projectile.owner < 0 ||
            projectile.owner >= Main.maxPlayers
        )
        {
            return;
        }

        if (damageDone <= 0)
            return;

        bool isBardProjectile =
            projectile.ModProjectile is BardProjectile;

        bool hasBardDamage =
            projectile.DamageType ==
            BardDamage.Instance;

        if (
            !isBardProjectile &&
            !hasBardDamage
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