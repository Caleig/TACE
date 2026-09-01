using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumAccessoryExpansion.Projectiles.MagicSheath;
using ThoriumMod.Buffs;
using ThoriumMod.Buffs.Healer;

namespace ThoriumAccessoryExpansion.Systems;

public class MagicSwordEnhancementGlobalProjectile
    : GlobalProjectile
{
    public override void OnHitNPC(
        Projectile projectile,
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {

        if (
            projectile.ModProjectile
            is not MagicSwordProjectile
        )
        {
            return;
        }

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


        Player player =
            Main.player[
                projectile.owner
            ];


        if (!player.active)
            return;


        MagicSwordEnhancementPlayer enhancement =
            player.GetModPlayer<
                MagicSwordEnhancementPlayer
            >();

        if (enhancement.HasBlazingScroll)
        {
            target.AddBuff(
                BuffID.ShadowFlame,
                600
            );


            SpawnExplosion(
                projectile,
                target
            );

            return;
        }

        if (enhancement.HasEnergyScroll)
        {
            target.AddBuff(
                ModContent.BuffType<
                    GraniteSurge
                >(),
                600
            );

            return;
        }

        if (enhancement.HasGeodeScroll)
        {
            target.AddBuff(
                ModContent.BuffType<
                    Sundered
                >(),
                600
            );

            return;
        }

        if (enhancement.HasHolyScroll)
        {
            target.AddBuff(
                ModContent.BuffType<
                    HolyGlare
                >(),
                600
            );

            return;
        }

        if (enhancement.HasSoulScroll)
        {
            int healAmount = 5;


            player.statLife =
                System.Math.Min(
                    player.statLifeMax2,
                    player.statLife +
                    healAmount
                );
        }
    }


    private static void SpawnExplosion(
        Projectile sword,
        NPC target)
    {
        Projectile.NewProjectile(
            sword.GetSource_OnHit(
                target
            ),
            target.Center,
            Vector2.Zero,
            ModContent.ProjectileType<
                MagicSwordExplosionProjectile
            >(),
            sword.damage * 2,
            4f,
            sword.owner
        );
    }
}