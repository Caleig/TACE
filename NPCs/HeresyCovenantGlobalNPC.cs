using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod.Buffs;

namespace ThoriumAccessoryExpansion.NPCs;

public class HeresyCovenantGlobalNPC : GlobalNPC
{
    public override bool InstancePerEntity => true;

    private int _heresyDamageTimer;

    private static bool AnyPlayerHas()
    {
        for (int i = 0; i < Main.maxPlayers; i++)
        {
            Player player = Main.player[i];

            if (
                player.active &&
                player.GetModPlayer<CovenantPlayer>()
                    .HeresyHasCovenant
            )
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasHeresyDebuff(NPC npc)
    {
        return
            npc.HasBuff(BuffID.ShadowFlame) ||
            npc.HasBuff(ModContent.BuffType<LightCurse>());
    }

    public override void OnHitByItem(
        NPC npc,
        Player player,
        Item item,
        NPC.HitInfo hit,
        int damageDone)
    {
        if (!player.active)
            return;

        if (
            !player.GetModPlayer<CovenantPlayer>()
                .HeresyHasCovenant
        )
        {
            return;
        }
        if (HasHeresyDebuff(npc))
        {
            player.lifeRegen += 5;
        }
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

        Player player = Main.player[projectile.owner];

        if (!player.active)
            return;

        if (
            !player.GetModPlayer<CovenantPlayer>()
                .HeresyHasCovenant
        )
        {
            return;
        }
        if (HasHeresyDebuff(npc))
        {
            player.lifeRegen += 5;
        }
    }

    public override void PostAI(NPC npc)
    {
        if (Main.netMode == NetmodeID.MultiplayerClient)
            return;

        if (!AnyPlayerHas())
        {
            _heresyDamageTimer = 0;
            return;
        }

        if (!HasHeresyDebuff(npc))
        {
            _heresyDamageTimer = 0;
            return;
        }

        _heresyDamageTimer++;

        if (_heresyDamageTimer >= 30)
        {
            _heresyDamageTimer = 0;

            npc.SimpleStrikeNPC(
                5,
                0,
                false,
                0
            );
        }
    }
}