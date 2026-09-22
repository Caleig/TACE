using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using Microsoft.Xna.Framework;

namespace ThoriumAccessoryExpansion.NPCs;

public class OrchestraConductorScoreGlobalNPC : GlobalNPC
{
    public override bool InstancePerEntity =>
        true;

    private const float AllyRange = 800f;

    private const int HitWindow = 120;

    private const float CombinedDamageRatio = 0.30f;

    private readonly int[] lastHitDamage =
        new int[Main.maxPlayers];

    private readonly ulong[] lastHitTick =
        new ulong[Main.maxPlayers];

    private ulong combinedDamageCooldownUntil;

    public override void OnHitByItem(
        NPC npc,
        Player player,
        Item item,
        NPC.HitInfo hit,
        int damageDone)
    {
        if (
            player.whoAmI < 0 ||
            player.whoAmI >= Main.maxPlayers
        )
        {
            return;
        }

        TryTriggerCombinedDamage(
            npc,
            player,
            damageDone,
            hit.HitDirection
        );

        RecordHit(
            player.whoAmI,
            damageDone
        );
    }

    public override void OnHitByProjectile(
        NPC npc,
        Projectile projectile,
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
            Main.player[projectile.owner];

        if (!player.active)
            return;

        TryTriggerCombinedDamage(
            npc,
            player,
            damageDone,
            hit.HitDirection
        );

        RecordHit(
            player.whoAmI,
            damageDone
        );
    }

    private void TryTriggerCombinedDamage(
        NPC npc,
        Player player,
        int damageDone,
        int hitDirection)
    {
        if (
            Main.netMode ==
            NetmodeID.MultiplayerClient
        )
        {
            return;
        }

        if (
            !npc.active ||
            npc.life <= 0 ||
            damageDone <= 0
        )
        {
            return;
        }

        if (
            Main.GameUpdateCount <
            combinedDamageCooldownUntil
        )
        {
            return;
        }

        BardAccessoryPlayer bardPlayer =
            player.GetModPlayer<
                BardAccessoryPlayer
            >();

        if (!bardPlayer.HasOrchestraConductorScore)
            return;

        Player ally =
            FindRecentNearbyAlly(
                player,
                npc
            );

        if (ally == null)
            return;

        int allyDamage =
            lastHitDamage[ally.whoAmI];

        int combinedDamage =
            (int)Math.Round(
                (damageDone + allyDamage) *
                CombinedDamageRatio
            );

        if (combinedDamage <= 0)
            return;

        combinedDamageCooldownUntil =
            Main.GameUpdateCount +
            HitWindow;

        npc.SimpleStrikeNPC(
            combinedDamage,
            hitDirection,
            false,
            0f
        );
    }

    private Player FindRecentNearbyAlly(
        Player player,
        NPC npc
    )
    {
        ulong currentTick =
            Main.GameUpdateCount;

        Player bestAlly = null;

        ulong bestTick = 0;

        for (
            int i = 0;
            i < Main.maxPlayers;
            i++
        )
        {
            if (i == player.whoAmI)
                continue;

            Player ally =
                Main.player[i];

            if (
                !ally.active ||
                ally.dead
            )
            {
                continue;
            }

            if (
                player.team == 0 ||
                ally.team == 0 ||
                ally.team != player.team
            )
            {
                continue;
            }

            if (
                Vector2.DistanceSquared(
                    player.Center,
                    ally.Center
                ) >
                AllyRange * AllyRange
            )
            {
                continue;
            }

            ulong tick =
                lastHitTick[i];

            if (
                currentTick < tick ||
                currentTick - tick >
                    HitWindow
            )
            {
                continue;
            }

            if (
                tick > bestTick &&
                lastHitDamage[i] > 0
            )
            {
                bestTick = tick;
                bestAlly = ally;
            }
        }

        return bestAlly;
    }

    private void RecordHit(
        int playerWhoAmI,
        int damageDone
    )
    {
        if (
            playerWhoAmI < 0 ||
            playerWhoAmI >= Main.maxPlayers ||
            damageDone <= 0
        )
        {
            return;
        }

        lastHitDamage[playerWhoAmI] =
            damageDone;

        lastHitTick[playerWhoAmI] =
            Main.GameUpdateCount;
    }
}