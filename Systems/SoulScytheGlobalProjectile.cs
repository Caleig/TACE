using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumAccessoryExpansion.Projectiles.Scythe;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Systems;

public class SoulScytheGlobalProjectile : GlobalProjectile
{
    public override void ModifyHitNPC(
        Projectile projectile,
        NPC target,
        ref NPC.HitModifiers modifiers)
    {
        if (!IsScythe(projectile))
            return;


        SoulOfScythePlayer player =
            Main.player[projectile.owner]
                .GetModPlayer<SoulOfScythePlayer>();


        if (!player.StyxBloodAnkh)
            return;


        if (
            player.BloodCharge <
            SoulOfScythePlayer.MaxBloodCharge
        )
        {
            return;
        }
        modifiers.FinalDamage *= 2f;
    }


    public override void OnHitNPC(
        Projectile projectile,
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {
        if (!IsScythe(projectile))
            return;


        if (damageDone <= 0)
            return;


        SoulOfScythePlayer player =
            Main.player[projectile.owner]
                .GetModPlayer<SoulOfScythePlayer>();
        int mode = -1;


        if (
            projectile.ModProjectile
            is SoulScytheProjectile
        )
        {
            mode =
                (int)projectile.ai[1];
        }
        if (
            mode == 1 &&
            target.life <= 0 &&
            Main.myPlayer == projectile.owner
        )
        {
            Projectile.NewProjectile(
                projectile.GetSource_FromThis(),
                target.Center,
                Vector2.Zero,
                ModContent.ProjectileType<
                    LifeSpiritProjectile
                >(),
                0,
                0f,
                projectile.owner
            );
        }
        if (mode == 2)
        {
            ApplyHolyGlare(
                projectile,
                target
            );


            if (
                target.life <= 0 &&
                Main.netMode !=
                NetmodeID.MultiplayerClient
            )
            {
                Main.player[projectile.owner]
                    .Heal(3);
            }
        }
        if (mode == 3)
        {
            ApplyHolyGlare(
                projectile,
                target
            );
        }
        if (mode == 4)
        {
            ApplyHolyGlare(
                projectile,
                target
            );

            ApplyTuned(
                projectile,
                target
            );


            GrantRadiantSoulEssence(
                projectile,
                target
            );
        }
        if (player.StyxBloodAnkh)
        {
            if (
                player.BloodCharge >=
                SoulOfScythePlayer.MaxBloodCharge
            )
            {
                player.BloodCharge = 0;


                int healAmount =
                    (int)(damageDone * 0.20f);


                if (
                    healAmount > 0 &&
                    Main.netMode !=
                    NetmodeID.MultiplayerClient
                )
                {
                    Main.player[projectile.owner]
                        .Heal(healAmount);
                }
            }
            else
            {
                player.BloodCharge++;
            }


            GiveBloodBoost(
                projectile
            );
        }
    }

    private static bool IsScythe(
        Projectile projectile)
    {
        return
            projectile.ModProjectile is ScythePro
            ||
            projectile.ModProjectile
                is SoulScytheProjectile;
    }

    private static void ApplyHolyGlare(
        Projectile projectile,
        NPC target)
    {
        Mod thorium =
            ModLoader.GetMod("ThoriumMod");


        int buff =
            thorium.Find<ModBuff>(
                "HolyGlare"
            ).Type;


        target.AddBuff(
            buff,
            300
        );
    }

    private static void ApplyTuned(
        Projectile projectile,
        NPC target)
    {
        Mod thorium =
            ModLoader.GetMod("ThoriumMod");


        int buff =
            thorium.Find<ModBuff>(
                "Tuned"
            ).Type;


        target.AddBuff(
            buff,
            300
        );
    }

    private static void GrantRadiantSoulEssence(
        Projectile projectile,
        NPC target)
    {
        if (
            projectile.ModProjectile
            is not SoulScytheProjectile
        )
        {
            return;
        }

        if (projectile.localAI[0] != 0f)
            return;


        int originalProjectileType =
            (int)projectile.ai[0];


        int scytheCharge =
            ThoriumMod.Items.HealerItems.ScytheItem
                .GetScytheChargeFromPro(
                    originalProjectileType
                );


        if (scytheCharge <= 0)
            return;


        if (!target.CanBeChasedBy())
            return;


        projectile.localAI[0] = 1f;


        Player player =
            Main.player[projectile.owner];


        ThoriumPlayer thoriumPlayer =
            player.GetThoriumPlayer();


        if (
            Main.netMode !=
            NetmodeID.MultiplayerClient
        )
        {
            player.AddBuff(
                ModContent.BuffType<
                    ThoriumMod.Buffs.Healer.SoulEssence
                >(),
                1800
            );


            thoriumPlayer.soulEssence +=
                scytheCharge;
        }


        player.GetModPlayer<
            SoulOfScythePlayer
        >().GiveSoulShield(
            scytheCharge
        );
    }
    private static void GiveBloodBoost(
        Projectile projectile)
    {
        Player owner =
            Main.player[projectile.owner];


        if (owner.team == 0)
            return;


        Mod thorium =
            ModLoader.GetMod("ThoriumMod");


        int bloodBoost =
            thorium.Find<ModBuff>(
                "BloodBoost"
            ).Type;


        for (int i = 0; i < Main.maxPlayers; i++)
        {
            Player ally =
                Main.player[i];


            if (!ally.active)
                continue;


            if (ally.dead)
                continue;


            if (ally.whoAmI == owner.whoAmI)
                continue;


            if (ally.team != owner.team)
                continue;


            if (
                Vector2.DistanceSquared(
                    owner.Center,
                    ally.Center
                ) > 800f * 800f
            )
            {
                continue;
            }


            ally.AddBuff(
                bloodBoost,
                300
            );
        }
    }
}