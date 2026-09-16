using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;

namespace ThoriumAccessoryExpansion.Systems;

public class CovenantGlobalProjectile : GlobalProjectile
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

        Player player =
            Main.player[projectile.owner];

        if (!player.active)
            return;

        if (
            hit.DamageType !=
            ModContent.GetInstance<HealerDamage>()
        )
        {
            return;
        }

        CovenantPlayer cp =
            player.GetModPlayer<CovenantPlayer>();

        if (cp.BoneHasCovenant)
        {
            cp.FallenRadianceStacks =
                (int)MathHelper.Clamp(
                    cp.FallenRadianceStacks + 6,
                    0,
                    CovenantPlayer.GlobalMaxStacks
                );

            cp.FallenRadianceTimer = 3600;

            return;
        }

        if (cp.CursedHasCovenant)
        {
            cp.FallenRadianceStacks =
                (int)MathHelper.Clamp(
                    cp.FallenRadianceStacks + 12,
                    0,
                    CovenantPlayer.GlobalMaxStacks
                );

            cp.FallenRadianceTimer = 3600;

            return;
        }

        if (cp.FallenHasCovenant)
        {
            cp.FallenRadianceStacks =
                (int)MathHelper.Clamp(
                    cp.FallenRadianceStacks + 5,
                    0,
                    CovenantPlayer.GlobalMaxStacks
                );

            cp.FallenRadianceTimer = 3600;

            target.AddBuff(
                BuffID.ShadowFlame,
                300
            );

            return;
        }

        if (cp.KarmaHasCovenant)
        {
            cp.FallenRadianceStacks =
                (int)MathHelper.Clamp(
                    cp.FallenRadianceStacks + 4,
                    0,
                    CovenantPlayer.GlobalMaxStacks
                );

            cp.FallenRadianceTimer = 3600;
        }
    }
}