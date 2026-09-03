using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Items.LegendaryResonance;
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



        if (
            mark.HasPrismaticResonance(
                projectile.owner
            )
            &&
            hit.Crit
            &&
            mark.legendaryResonanceDropCooldown <= 0
            &&
            Main.rand.NextBool(1)
        )
        {

            if (
                Main.netMode !=
                NetmodeID.MultiplayerClient
            )
            {

                LegendaryResonancePickupItem.SpawnRandom(
                    projectile.GetSource_FromThis(),
                    target.Center
                );


                mark.legendaryResonanceDropCooldown =
                    20;

            }

        }



        if (mark.gemType == GemType.None)
            return;


        if (mark.HasGemMark(GemType.Prismatic))
        {

            int owner =
                mark.gemOwner;



            int resonanceDamage =
                (int)(
                    mark.gemDamage
                    * 0.80f
                );



            if (mark.ConsumeGemMark(target))
            {

                int proj =
                    Projectile.NewProjectile(
                        projectile.GetSource_FromThis(),
                        target.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<
                            LegendaryResonanceBurstProjectile
                        >(),
                        resonanceDamage,
                        0f,
                        owner
                    );


                Main.projectile[proj]
                    .GetGlobalProjectile<
                        MagicContractGlobalProjectile
                    >()
                    .gemProjectile = true;



                Main.projectile[proj]
                    .ai[0] =
                    target.whoAmI;



                mark.AddPrismaticResonance(
                    360,
                    owner
                );

            }



            return;

        }

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

            mark.ConsumeGemMark(target);


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

        else if (mark.HasGemMark(GemType.CrystallineAmethyst))
        {
            mark.ConsumeGemMark(target);


            int proj =
            Projectile.NewProjectile(
                projectile.GetSource_FromThis(),
                spawnPosition,
                shotVelocity * 1.25f,
                ModContent.ProjectileType<CrystallineAmethystCrystalProjectile>(),
                (int)(damage * 1.15f),
                0,
                projectile.owner
            );


            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;



            for (int i = 0; i < 2; i++)
            {

                Vector2 offset =
                    new Vector2(
                        0,
                        i == 0 ? -12f : 12f
                    );


                int shard =
                Projectile.NewProjectile(
                    projectile.GetSource_FromThis(),
                    spawnPosition + offset,
                    shotVelocity,
                    ModContent.ProjectileType<CrystallineShardProjectile>(),
                    (int)(damage * 0.2f),
                    0,
                    projectile.owner
                );


                Main.projectile[shard]
                    .GetGlobalProjectile<MagicContractGlobalProjectile>()
                    .gemProjectile = true;

            }
        }

        else if (mark.HasGemMark(GemType.Topaz))
        {

            mark.ConsumeGemMark(target);


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

        else if (mark.HasGemMark(GemType.CrystallineTopaz))
        {
            mark.ConsumeGemMark(target);


            int proj =
            Projectile.NewProjectile(
                projectile.GetSource_FromThis(),
                spawnPosition,
                shotVelocity * 1.25f,
                ModContent.ProjectileType<CrystallineTopazCrystalProjectile>(),
                (int)(damage * 1.15f),
                0,
                projectile.owner
            );


            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;



            for (int i = 0; i < 2; i++)
            {

                Vector2 offset =
                    new Vector2(
                        0,
                        i == 0 ? -12f : 12f
                    );


                int shard =
                Projectile.NewProjectile(
                    projectile.GetSource_FromThis(),
                    spawnPosition + offset,
                    shotVelocity,
                    ModContent.ProjectileType<CrystallineShardProjectile>(),
                    (int)(damage * 0.2f),
                    0,
                    projectile.owner
                );


                Main.projectile[shard]
                    .GetGlobalProjectile<MagicContractGlobalProjectile>()
                    .gemProjectile = true;

            }
        }

        else if (mark.HasGemMark(GemType.Sapphire))
        {

            mark.ConsumeGemMark(target);


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

        else if (mark.HasGemMark(GemType.CrystallineSapphire))
        {

            mark.ConsumeGemMark(target);


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
                    ModContent.ProjectileType<CrystallineSapphireCrystalProjectile>(),
                    (int)(damage * 0.7f),
                    0,
                    projectile.owner
                );



                Main.projectile[proj]
                    .ai[0] = i;



                Main.projectile[proj]
                    .GetGlobalProjectile<MagicContractGlobalProjectile>()
                    .gemProjectile = true;

            }


            for (int i = 0; i < 2; i++)
            {

                Vector2 offset =
                    new Vector2(
                        0,
                        i == 0 ? -12f : 12f
                    );



                int shard =
                Projectile.NewProjectile(
                    projectile.GetSource_FromThis(),
                    spawnPosition + offset,
                    direction * 10f,
                    ModContent.ProjectileType<CrystallineShardProjectile>(),
                    (int)(damage * 0.2f),
                    0,
                    projectile.owner
                );



                Main.projectile[shard]
                    .GetGlobalProjectile<MagicContractGlobalProjectile>()
                    .gemProjectile = true;

            }

        }

        else if (mark.HasGemMark(GemType.Emerald))
        {

            mark.ConsumeGemMark(target);


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

        else if (mark.HasGemMark(GemType.CrystallineEmerald))
        {
            mark.ConsumeGemMark(target);


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
                ModContent.ProjectileType<CrystallineEmeraldCrystalProjectile>(),
                (int)(damage * 1.15f),
                0,
                projectile.owner
            );


            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;

            for (int i = 0; i < 2; i++)
            {

                Vector2 offset =
                    new Vector2(
                        0,
                        i == 0 ? -12f : 12f
                    );


                int shard =
                Projectile.NewProjectile(
                    projectile.GetSource_FromThis(),
                    spawnPosition + offset,
                    emeraldVelocity,
                    ModContent.ProjectileType<CrystallineShardProjectile>(),
                    (int)(damage * 0.2f),
                    0,
                    projectile.owner
                );


                Main.projectile[shard]
                    .GetGlobalProjectile<MagicContractGlobalProjectile>()
                    .gemProjectile = true;

            }
        }

        else if (mark.HasGemMark(GemType.Amber))
        {

            mark.ConsumeGemMark(target);


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

        else if (mark.HasGemMark(GemType.CrystallineAmber))
        {
            mark.ConsumeGemMark(target);


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
                ModContent.ProjectileType<CrystallineAmberCrystalProjectile>(),
                (int)(damage * 1.15f),
                0,
                projectile.owner
            );


            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;

            for (int i = 0; i < 2; i++)
            {

                Vector2 offset =
                    new Vector2(
                        0,
                        i == 0 ? -12f : 12f
                    );


                int shard =
                Projectile.NewProjectile(
                    projectile.GetSource_FromThis(),
                    spawnPosition + offset,
                    amberVelocity,
                    ModContent.ProjectileType<CrystallineShardProjectile>(),
                    (int)(damage * 0.2f),
                    0,
                    projectile.owner
                );


                Main.projectile[shard]
                    .GetGlobalProjectile<MagicContractGlobalProjectile>()
                    .gemProjectile = true;

            }
        }

        else if (mark.HasGemMark(GemType.Ruby))
        {
            mark.ConsumeGemMark(target);


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

        else if (mark.HasGemMark(GemType.CrystallineRuby))
        {
            mark.ConsumeGemMark(target);


            Vector2 direction =
                Vector2.Normalize(
                    target.Center - spawnPosition
                );



            int proj =
                Projectile.NewProjectile(
                    projectile.GetSource_FromThis(),
                    spawnPosition,
                    direction * 12f,
                    ModContent.ProjectileType<CrystallineRubyBurstProjectile>(),
                    (int)(damage * 0.5f),
                    0,
                    projectile.owner
                );


            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;



            for (int i = 0; i < 2; i++)
            {

                Vector2 offset =
                    new Vector2(
                        0,
                        i == 0 ? -12f : 12f
                    );


                int shard =
                    Projectile.NewProjectile(
                        projectile.GetSource_FromThis(),
                        spawnPosition + offset,
                        direction * 10f,
                        ModContent.ProjectileType<CrystallineShardProjectile>(),
                        (int)(damage * 0.2f),
                        0,
                        projectile.owner
                    );


                Main.projectile[shard]
                    .GetGlobalProjectile<MagicContractGlobalProjectile>()
                    .gemProjectile = true;

            }
        }

        else if (mark.HasGemMark(GemType.Diamond))
        {

            mark.ConsumeGemMark(target);


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
                (int)(damage * 1.75f),
                0,
                projectile.owner
            );



            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;

        }

        else if (mark.HasGemMark(GemType.CrystallineDiamond))
        {

            mark.ConsumeGemMark(target);


            Player player =
                Main.player[projectile.owner];


            Vector2 diamondSpawnPosition =
                player.MountedCenter
                + new Vector2(
                    0,
                    8f
                );


            Vector2 direction =
                Vector2.Normalize(
                    target.Center - diamondSpawnPosition
                );


            Vector2 velocity =
                direction * 10f;



            int proj =
                Projectile.NewProjectile(
                    projectile.GetSource_FromThis(),
                    diamondSpawnPosition,
                    velocity,
                    ModContent.ProjectileType<CrystallineDiamondCrystalProjectile>(),
                    (int)(damage * 2.0f),
                    0,
                    projectile.owner
                );


            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;



            for (int i = 0; i < 2; i++)
            {

                Vector2 offset =
                    new Vector2(
                        0,
                        i == 0 ? -12f : 12f
                    );


                int shard =
                    Projectile.NewProjectile(
                        projectile.GetSource_FromThis(),
                        diamondSpawnPosition + offset,
                        velocity,
                        ModContent.ProjectileType<CrystallineShardProjectile>(),
                        (int)(damage * 0.2f),
                        0,
                        projectile.owner
                    );


                Main.projectile[shard]
                    .GetGlobalProjectile<MagicContractGlobalProjectile>()
                    .gemProjectile = true;

            }

        }

        else if (mark.HasGemMark(GemType.Opal))
        {

            mark.ConsumeGemMark(target);


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

        else if (mark.HasGemMark(GemType.CrystallineOpal))
        {

            mark.ConsumeGemMark(target);


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
                    ModContent.ProjectileType<CrystallineOpalCrystalProjectile>(),
                    (int)(damage * 1.15f),
                    0,
                    projectile.owner
                );


            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;

            for (int i = 0; i < 2; i++)
            {

                Vector2 offset =
                    new Vector2(
                        0,
                        i == 0 ? -12f : 12f
                    );


                int shard =
                    Projectile.NewProjectile(
                        projectile.GetSource_FromThis(),
                        spawnPosition + offset,
                        velocity,
                        ModContent.ProjectileType<CrystallineShardProjectile>(),
                        (int)(damage * 0.2f),
                        0,
                        projectile.owner
                    );


                Main.projectile[shard]
                    .GetGlobalProjectile<MagicContractGlobalProjectile>()
                    .gemProjectile = true;

            }

        }

        else if (mark.HasGemMark(GemType.Aquamarine))
        {

            mark.ConsumeGemMark(target);


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

        else if (mark.HasGemMark(GemType.CrystallineAquamarine))
        {
            mark.ConsumeGemMark(target);


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
                ModContent.ProjectileType<CrystallineAquamarineCrystalProjectile>(),
                (int)(damage * 1.3f),
                0,
                projectile.owner
            );


            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;



            for (int i = 0; i < 2; i++)
            {

                Vector2 offset =
                    new Vector2(
                        0,
                        i == 0 ? -12f : 12f
                    );


                int shard =
                Projectile.NewProjectile(
                    projectile.GetSource_FromThis(),
                    spawnPosition + offset,
                    velocity,
                    ModContent.ProjectileType<CrystallineShardProjectile>(),
                    (int)(damage * 0.2f),
                    0,
                    projectile.owner
                );


                Main.projectile[shard]
                    .GetGlobalProjectile<MagicContractGlobalProjectile>()
                    .gemProjectile = true;

            }

        }
    }
    public override void ModifyHitNPC(
    Projectile projectile,
    NPC target,
    ref NPC.HitModifiers modifiers)
    {

        if (projectile.DamageType != DamageClass.Summon)
            return;



        GemMarkGlobalNPC mark =
            target.GetGlobalNPC<GemMarkGlobalNPC>();



        if (!mark.HasPrismaticResonance())
            return;



        if (!mark.HasPrismaticResonance(
            projectile.owner))
            return;



        Player player =
            Main.player[projectile.owner];



        if (!player.active)
            return;



        float magicCrit =
            player.GetTotalCritChance(
                DamageClass.Magic
            );



        magicCrit =
            Math.Max(
                0f,
                magicCrit
            );



        float resonanceCrit =
            20f
            +
            magicCrit * 0.25f;



        resonanceCrit =
            Math.Min(
                100f,
                resonanceCrit
            );



        if (
            Main.rand.NextFloat(
                100f
            )
            <
            resonanceCrit
        )
        {
            modifiers.SetCrit();
        }
    }
}