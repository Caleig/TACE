using Terraria;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Systems;

public class MeleeGauntletGlobalProjectile : GlobalProjectile
{
    public override void ModifyHitNPC(
        Projectile projectile,
        NPC target,
        ref NPC.HitModifiers modifiers)
    {
        if (
            !projectile.DamageType.CountsAsClass(
                DamageClass.Melee
            )
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

        Player player =
            Main.player[projectile.owner];

        if (
            !player.active ||
            player.dead
        )
        {
            return;
        }

        MeleeGauntletPlayer gauntlet =
            player.GetModPlayer<MeleeGauntletPlayer>();

        if (gauntlet.HasTitanBracer)
        {
            modifiers.CritDamage += 0.25f;
        }
    }

    public override void OnHitNPC(
        Projectile projectile,
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {
        if (
            !projectile.DamageType.CountsAsClass(
                DamageClass.Melee
            )
        )
        {
            return;
        }

        if (damageDone <= 0)
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

        Player player =
            Main.player[projectile.owner];

        if (
            !player.active ||
            player.dead
        )
        {
            return;
        }

        MeleeGauntletPlayer gauntlet =
            player.GetModPlayer<MeleeGauntletPlayer>();
        gauntlet.RegisterMeleeHit();
        if (
            gauntlet.HasSoulHunterGlove &&
            Main.rand.Next(100) < 15
        )
        {
            target.SimpleStrikeNPC(
                damageDone,
                hit.HitDirection,
                false,
                0f,
                DamageClass.Melee,
                false,
                0f
            );
        }
    }
}