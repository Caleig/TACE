using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.Ranged.GunModification;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Systems;

public class DreadQuiverGlobalProjectile
    : GlobalProjectile
{
    public override bool AppliesToEntity(
        Projectile entity,
        bool lateInstantiation)
    {
        return entity.arrow;
    }


    public override void OnSpawn(
        Projectile projectile,
        IEntitySource source)
    {
        if (
            projectile.owner < 0 ||
            projectile.owner >= Main.maxPlayers
        )
        {
            return;
        }


        Player player =
            Main.player[
                projectile.owner
            ];


        if (!player.active)
            return;


        if (
            player.GetModPlayer<
                GunModificationPlayer
            >()
            .HasDreadQuiver
        )
        {
            projectile.velocity *=
                DreadQuiver.ArrowSpeedMult;
        }
    }


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


        Player player =
            Main.player[
                projectile.owner
            ];


        if (!player.active)
            return;


        if (
            !player.GetModPlayer<
                GunModificationPlayer
            >()
            .HasDreadQuiver
        )
        {
            return;
        }


        int extraDamage =
            (int)(
                projectile.damage *
                DreadQuiver.CopyDamage
            );


        if (extraDamage <= 0)
            return;


        target.SimpleStrikeNPC(
            extraDamage,
            hit.HitDirection,
            hit.Crit,
            projectile.knockBack
        );
    }
}