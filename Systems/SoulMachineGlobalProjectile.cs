using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.NPCs;
using ThoriumAccessoryExpansion.Accessories.Summon.SoulMachineSigil;

namespace ThoriumAccessoryExpansion.Systems;

public class SoulMachineGlobalProjectile
    : GlobalProjectile
{
    public override void OnHitNPC(
        Projectile projectile,
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {
        if (
            projectile.type >= 0 &&
            projectile.type <
                ProjectileID.Sets.IsAWhip.Length &&
            ProjectileID.Sets.IsAWhip[
                projectile.type
            ]
        )
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

            if (
                !player.active ||
                player.dead
            )
            {
                return;
            }
            bool hasSigil =
                player.armor != null &&
                HasSoulMachineSigil(player);

            if (!hasSigil)
                return;

            SoulMachineMarkGlobalNPC mark =
                target.GetGlobalNPC<
                    SoulMachineMarkGlobalNPC
                >();
            mark.ApplyMark(
                target
            );

            return;
        }
        if (
            projectile.type < 0 ||
            projectile.type >=
                ProjectileID.Sets.SentryShot.Length
        )
        {
            return;
        }

        if (
            !ProjectileID.Sets.SentryShot[
                projectile.type
            ]
        )
        {
            return;
        }

        SoulMachineMarkGlobalNPC targetMark =
            target.GetGlobalNPC<
                SoulMachineMarkGlobalNPC
            >();

        if (
            !targetMark.HasSoulMachineMark
        )
        {
            return;
        }
        if (damageDone > 0)
        {
            targetMark.ConsumeMark(
                target
            );
        }
    }

    public override void ModifyHitNPC(
        Projectile projectile,
        NPC target,
        ref NPC.HitModifiers modifiers)
    {
        if (
            projectile.type < 0 ||
            projectile.type >=
                ProjectileID.Sets.SentryShot.Length
        )
        {
            return;
        }

        if (
            !ProjectileID.Sets.SentryShot[
                projectile.type
            ]
        )
        {
            return;
        }

        SoulMachineMarkGlobalNPC mark =
            target.GetGlobalNPC<
                SoulMachineMarkGlobalNPC
            >();

        if (
            !mark.HasSoulMachineMark
        )
        {
            return;
        }
        modifiers.FinalDamage *= 1.25f;
    }

    private static bool HasSoulMachineSigil(
        Player player)
    {
        for (
            int i = 3;
            i < player.armor.Length;
            i++
        )
        {
            Item item =
                player.armor[i];

            if (
                item != null &&
                !item.IsAir &&
                item.type ==
                    ModContent.ItemType<
                        SoulMachineSigil
                    >()
            )
            {
                return true;
            }
        }

        return false;
    }
}