using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Buffs;

namespace ThoriumAccessoryExpansion.Projectiles;

public class HeresyCovenantGlobalProjectile : GlobalProjectile
{
    public override void ModifyHitNPC(
        Projectile projectile,
        NPC target,
        ref NPC.HitModifiers modifiers)
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

        CovenantPlayer cp =
            player.GetModPlayer<CovenantPlayer>();

        if (!cp.HeresyHasCovenant)
            return;

        if (
            projectile.DamageType !=
            ModContent.GetInstance<HealerDamage>()
        )
        {
            return;
        }

        bool hasDebuff =
            target.HasBuff(BuffID.ShadowFlame) ||
            target.HasBuff(
                ModContent.BuffType<LightCurse>()
            );

        if (hasDebuff)
        {
            modifiers.FinalDamage *= 1.15f;

            cp.HeresyLifeRegenTimer = 120;
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
            Main.player[projectile.owner];

        if (!player.active)
            return;

        CovenantPlayer cp =
            player.GetModPlayer<CovenantPlayer>();

        if (!cp.HeresyHasCovenant)
            return;

        if (
            projectile.DamageType ==
            ModContent.GetInstance<HealerDamage>()
        )
        {
            target.AddBuff(
                BuffID.ShadowFlame,
                300
            );
        }

        base.OnHitNPC(
            projectile,
            target,
            hit,
            damageDone
        );
    }
}