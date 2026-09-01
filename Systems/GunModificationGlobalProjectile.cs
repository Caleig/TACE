using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Projectiles.GunModification;
using ThoriumMod.Items.Donate;

using GunModificationPlayerState =
    global::ThoriumAccessoryExpansion.Players.GunModificationPlayer;

namespace ThoriumAccessoryExpansion.Systems;

public class GunModificationGlobalProjectile : GlobalProjectile
{
    public override bool InstancePerEntity =>
        true;

    private bool heatOverloadProjectile;


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
        if (
            source is not IEntitySource_WithStatsFromItem itemSource
        )
        {
            return;
        }


        Item weapon =
            itemSource.Item;


        if (
            weapon == null
        )
        {
            return;
        }

        if (
            weapon.useAmmo != AmmoID.Bullet
        )
        {
            return;
        }

        if (
            weapon.ModItem is HellfireMinigun
        )
        {
            return;
        }


        Player player =
            Main.player[
                projectile.owner
            ];


        if (
            !player.active
        )
        {
            return;
        }


        GunModificationPlayerState modification =
            player.GetModPlayer<
                GunModificationPlayerState
            >();


        if (
            !modification.HasHeatModification
        )
        {
            return;
        }

        if (
            modification.IsPendingOverloadProjectile()
        )
        {
            heatOverloadProjectile = true;
        }
    }


    public override void ModifyHitNPC(
        Projectile projectile,
        NPC target,
        ref NPC.HitModifiers modifiers)
    {
        Player player =
            GetOwner(projectile);


        if (
            player == null
        )
        {
            return;
        }


        GunModificationPlayerState modification =
            player.GetModPlayer<
                GunModificationPlayerState
            >();

        if (
            modification.HasTitanGunMod
        )
        {
            modifiers.CritDamage += 1f;
        }
    }


    public override void OnHitNPC(
        Projectile projectile,
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {

        if (
            !heatOverloadProjectile
        )
        {
            return;
        }


        Player player =
            GetOwner(projectile);


        if (
            player == null
        )
        {
            return;
        }


        GunModificationPlayerState modification =
            player.GetModPlayer<
                GunModificationPlayerState
            >();

        if (
            !modification.TryConsumeHeatForHit()
        )
        {
            heatOverloadProjectile = false;

            return;
        }
        int extraDamage =
            modification.HeatOverloadDamage;


        if (
            extraDamage > 0
        )
        {
            bool crit =
                modification.HeatOverloadCanCrit &&
                hit.Crit;


            target.SimpleStrikeNPC(
                extraDamage,
                hit.HitDirection,
                crit,
                projectile.knockBack
            );
        }
        if (
            modification.HasHellstoneGunMod
        )
        {
            target.AddBuff(
                BuffID.OnFire3,
                600
            );


            SpawnExplosion(
                projectile,
                target
            );
        }
        if (
            modification.HasGreenDragonGunMod
        )
        {
            target.AddBuff(
                BuffID.CursedInferno,
                600
            );
        }
        if (
            modification.HasFleshGunMod ||
            modification.HasFleshTrigger
        )
        {
            target.AddBuff(
                BuffID.Ichor,
                600
            );
        }
    }


    private static void SpawnExplosion(
        Projectile bullet,
        NPC target)
    {
        Projectile.NewProjectile(
            bullet.GetSource_OnHit(
                target
            ),
            target.Center,
            Microsoft.Xna.Framework.Vector2.Zero,
            ModContent.ProjectileType<
                HellfireExplosionProjectile
            >(),
            bullet.damage * 2,
            4f,
            bullet.owner
        );
    }


    private static Player GetOwner(
        Projectile projectile)
    {
        if (
            projectile.owner < 0 ||
            projectile.owner >= Main.maxPlayers
        )
        {
            return null;
        }


        Player player =
            Main.player[
                projectile.owner
            ];


        if (
            !player.active
        )
        {
            return null;
        }


        return player;
    }
}