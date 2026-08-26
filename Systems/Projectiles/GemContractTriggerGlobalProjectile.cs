using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.NPCs;
using ThoriumAccessoryExpansion.Projectiles.MagicContract;


namespace ThoriumAccessoryExpansion.Systems.Projectiles;

public class GemContractTriggerGlobalProjectile : GlobalProjectile
{

    public override void OnHitNPC(
        Projectile projectile,
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {

        MagicContractGlobalProjectile magic =
            projectile.GetGlobalProjectile<MagicContractGlobalProjectile>();


        if (magic.gemProjectile)
            return;


        if (magic.magicConverted)
            return;


        if (projectile.DamageType != DamageClass.Summon)
            return;



        GemMarkGlobalNPC mark =
            target.GetGlobalNPC<GemMarkGlobalNPC>();


        if (mark.gemType == GemType.None)
            return;



        int damage = mark.gemDamage;



        Vector2 spawnPosition =
            Main.player[projectile.owner].Center;



        Vector2 shotVelocity =
            Vector2.Normalize(
                target.Center - spawnPosition
            )
            * 10f;



        if (mark.HasGemMark(GemType.Amethyst))
        {

            mark.ConsumeGemMark();


            int proj =
            Projectile.NewProjectile(
                projectile.GetSource_FromThis(),
                spawnPosition,
                shotVelocity,
                ModContent.ProjectileType<AmethystCrystalProjectile>(),
                damage,
                0,
                projectile.owner
            );


            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;

        }

        else if (mark.HasGemMark(GemType.Topaz))
        {

            mark.ConsumeGemMark();


            int proj =
            Projectile.NewProjectile(
                projectile.GetSource_FromThis(),
                spawnPosition,
                shotVelocity,
                ModContent.ProjectileType<TopazCrystalProjectile>(),
                damage,
                0,
                projectile.owner
            );


            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;

        }

        else if (mark.HasGemMark(GemType.Sapphire))
        {

            mark.ConsumeGemMark();


            Vector2 direction =
                target.Center - spawnPosition;


            direction.Normalize();



            for (int i = 0; i < 2; i++)
            {

                int proj =
                Projectile.NewProjectile(
                    projectile.GetSource_FromThis(),
                    spawnPosition,
                    direction * 10f,
                    ModContent.ProjectileType<SapphireCrystalProjectile>(),
                    (int)(damage * 0.6f),
                    0,
                    projectile.owner
                );


                Main.projectile[proj]
                    .ai[0] = i;



                Main.projectile[proj]
                    .GetGlobalProjectile<MagicContractGlobalProjectile>()
                    .gemProjectile = true;

            }

        }

        else if (mark.HasGemMark(GemType.Emerald))
        {

            mark.ConsumeGemMark();


            Vector2 emeraldVelocity =
                Vector2.Normalize(
                    target.Center - spawnPosition
                )
                * 16f;



            int proj =
            Projectile.NewProjectile(
                projectile.GetSource_FromThis(),
                spawnPosition,
                emeraldVelocity,
                ModContent.ProjectileType<EmeraldCrystalProjectile>(),
                damage,
                0,
                projectile.owner
            );


            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;

        }

        else if (mark.HasGemMark(GemType.Amber))
        {

            mark.ConsumeGemMark();


            Vector2 amberVelocity =
                Vector2.Normalize(
                    target.Center - spawnPosition
                )
                * 12f;



            int proj =
            Projectile.NewProjectile(
                projectile.GetSource_FromThis(),
                spawnPosition,
                amberVelocity,
                ModContent.ProjectileType<AmberCrystalProjectile>(),
                damage,
                0,
                projectile.owner
            );


            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;

        }

        else if (mark.HasGemMark(GemType.Ruby))
        {
            mark.ConsumeGemMark();


            Vector2 direction =
                Vector2.Normalize(
                    target.Center - spawnPosition
                );


            int proj =
                Projectile.NewProjectile(
                    projectile.GetSource_FromThis(),
                    spawnPosition,
                    direction * 12f,
                    ModContent.ProjectileType<RubyBurstProjectile>(),
                    (int)(damage * 0.5f),
                    0,
                    projectile.owner
                    );


            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;
        }

        else if (mark.HasGemMark(GemType.Diamond))
        {

            mark.ConsumeGemMark();


            Player player =
                Main.player[projectile.owner];


            Vector2 diamondSpawnPosition =
                player.MountedCenter
                + new Vector2(
                    0,
                    8f
                );



            Vector2 velocity =
                Vector2.Normalize(
                    target.Center - diamondSpawnPosition
                )
                * 10f;



            int proj =
            Projectile.NewProjectile(
                projectile.GetSource_FromThis(),
                diamondSpawnPosition,
                velocity,
                ModContent.ProjectileType<DiamondCrystalProjectile>(),
                (int)(damage * 1.5f),
                0,
                projectile.owner
            );



            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;

        }

        else if (mark.HasGemMark(GemType.Opal))
        {

            mark.ConsumeGemMark();


            Vector2 velocity =
                Vector2.Normalize(
                    target.Center - spawnPosition
                )
                * 12f;



            int proj =
            Projectile.NewProjectile(
                projectile.GetSource_FromThis(),
                spawnPosition,
                velocity,
                ModContent.ProjectileType<OpalCrystalProjectile>(),
                damage,
                0,
                projectile.owner
            );


            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;

        }

        else if (mark.HasGemMark(GemType.Aquamarine))
        {

            mark.ConsumeGemMark();


            Vector2 velocity =
                Vector2.Normalize(
                    target.Center - spawnPosition
                )
                * 16f;



            int proj =
            Projectile.NewProjectile(
                projectile.GetSource_FromThis(),
                spawnPosition,
                velocity,
                ModContent.ProjectileType<AquamarineCrystalProjectile>(),
                (int)(damage * 1.2f),
                0,
                projectile.owner
            );


            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;

        }
    }

}