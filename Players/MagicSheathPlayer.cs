using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath;
using ThoriumAccessoryExpansion.Projectiles.MagicSheath;

namespace ThoriumAccessoryExpansion.Players;

public class MagicSheathPlayer : ModPlayer
{
    private int sheathLevel;
    private int spawnCooldown;
    private int previousMana;

    public int SheathLevel =>
        sheathLevel;

    private static int SwordProjectileType =>
        ModContent.ProjectileType<MagicSwordProjectile>();

    public override void Initialize()
    {
        previousMana = Player.statMana;
    }

    public override void ResetEffects()
    {
        sheathLevel = 0;
    }

    public void SetSheathLevel(int level)
    {
        sheathLevel = Math.Max(
            sheathLevel,
            level
        );
    }

    public int GetMaxSwords()
    {
        return sheathLevel switch
        {
            1 => 2,
            2 => 3,
            3 => 4,
            _ => 0
        };
    }

    public float GetSpawnChance()
    {
        return sheathLevel switch
        {
            1 => 0.10f,
            2 => 0.20f,
            3 => 0.25f,
            _ => 0f
        };
    }

    public float GetLowManaThreshold()
    {
        return sheathLevel switch
        {
            1 => 0.25f,
            2 => 0.30f,
            3 => 0.35f,
            _ => 0f
        };
    }

    public int GetManaRestorePercent()
    {
        return sheathLevel switch
        {
            1 => 10,
            2 => 15,
            3 => 15,
            _ => 0
        };
    }

    public int GetFlatMagicDamagePerSword()
    {
        return sheathLevel switch
        {
            2 => 3,
            3 => 4,
            _ => 0
        };
    }

    public int GetDefensePerSword()
    {
        return sheathLevel == 3
            ? 3
            : 0;
    }

    public int GetExtraSwordPenetration()
    {
        if (sheathLevel == 3)
            return 4;

        if (sheathLevel == 2)
            return 2;

        MagicSwordEnhancementPlayer enhancement =
            Player.GetModPlayer<
                MagicSwordEnhancementPlayer
            >();

        return enhancement.HasSpiritBlade
            ? 2
            : 0;
    }

    public int GetSwordCount()
    {
        if (
            SwordProjectileType < 0 ||
            SwordProjectileType >=
                Player.ownedProjectileCounts.Length
        )
        {
            return 0;
        }

        return Player.ownedProjectileCounts[
            SwordProjectileType
        ];
    }

    public float GetManaCostMultiplier()
    {
        return 1f +
               GetSwordCount() * 0.05f;
    }

    public int GetSwordDamage()
    {
        Item heldItem =
            Player.HeldItem;

        if (
            heldItem == null ||
            heldItem.damage <= 0
        )
        {
            return sheathLevel switch
            {
                1 => 15,
                2 => 30,
                3 => 30,
                _ => 0
            };
        }


        if (
            heldItem.DamageType !=
            DamageClass.Magic
        )
        {
            return sheathLevel switch
            {
                1 => 15,
                2 => 30,
                3 => 30,
                _ => 0
            };
        }


        int finalWeaponDamage =
            Player.GetWeaponDamage(
                heldItem
            );


        float multiplier =
            sheathLevel switch
            {
                1 => 0.15f,
                2 => 0.30f,
                3 => 0.50f,
                _ => 0f
            };


        return Math.Max(
            1,
            (int)(
                finalWeaponDamage *
                multiplier
            )
        );
    }

    public override void PostUpdateEquips()
    {
        base.PostUpdateEquips();

        int swordCount =
            GetSwordCount();

        if (sheathLevel == 3)
        {
            Player.GetDamage(
                DamageClass.Magic
            ) += 0.15f;
        }

        if (swordCount > 0)
        {
            Player.GetDamage(
                DamageClass.Magic
            ).Flat +=
                swordCount *
                GetFlatMagicDamagePerSword();

            Player.statDefense +=
                swordCount *
                GetDefensePerSword();
        }
    }

    public override void ModifyManaCost(
        Item item,
        ref float reduce,
        ref float mult)
    {
        if (
            sheathLevel > 0 &&
            item.DamageType == DamageClass.Magic
        )
        {
            mult *=
                GetManaCostMultiplier();
        }
    }

    public override void PostUpdate()
    {
        if (spawnCooldown > 0)
            spawnCooldown--;

        if (sheathLevel == 0)
        {
            if (
                Main.netMode !=
                    NetmodeID.MultiplayerClient ||
                Player.whoAmI ==
                    Main.myPlayer
            )
            {
                KillAllSwords();
            }

            previousMana =
                Player.statMana;

            return;
        }

        bool isLocalPlayer =
            Player.whoAmI == Main.myPlayer;

        if (isLocalPlayer)
        {
            int currentMana =
                Player.statMana;

            int manaDifference =
                previousMana -
                currentMana;

            if (
                manaDifference > 0 &&
                spawnCooldown <= 0 &&
                GetSwordCount() <
                    GetMaxSwords()
            )
            {
                TrySpawnSword();
            }

            previousMana =
                currentMana;

            CheckLowMana();
        }
        else
        {
            previousMana =
                Player.statMana;
        }
    }

    private void TrySpawnSword()
    {
        if (
            sheathLevel <= 0 ||
            spawnCooldown > 0
        )
        {
            return;
        }

        if (
            GetSwordCount() >=
            GetMaxSwords()
        )
        {
            return;
        }

        if (
            Main.rand.NextFloat() >=
            GetSpawnChance()
        )
        {
            return;
        }

        SpawnSword();

        spawnCooldown = 30;
    }

    private void SpawnSword()
    {
        int swordDamage =
            GetSwordDamage();

        int extraPenetration =
            GetExtraSwordPenetration();

        Projectile projectile =
            Projectile.NewProjectileDirect(
                Player.GetSource_FromThis(
                    "MagicSheath"
                ),
                Player.Center -
                new Microsoft.Xna.Framework.Vector2(
                    0f,
                    40f
                ),
                Microsoft.Xna.Framework.Vector2.Zero,
                SwordProjectileType,
                swordDamage,
                2f,
                Player.whoAmI,
                ai0: 0f,
                ai1: extraPenetration
            );

        projectile.netUpdate = true;
    }

    private void CheckLowMana()
    {
        if (Player.statManaMax2 <= 0)
            return;

        float threshold =
            GetLowManaThreshold();

        if (threshold <= 0f)
            return;

        float ratio =
            (float)Player.statMana /
            Player.statManaMax2;

        if (ratio > threshold)
            return;

        foreach (
            Projectile projectile
            in Main.projectile
        )
        {
            if (
                !projectile.active ||
                projectile.owner !=
                    Player.whoAmI ||
                projectile.type !=
                    SwordProjectileType
            )
            {
                continue;
            }

            if (
                projectile.ModProjectile
                    is MagicSwordProjectile sword
            )
            {
                sword.FireAt(
                    Main.MouseWorld
                );
            }
        }
    }

    private void KillAllSwords()
    {
        foreach (
            Projectile projectile
            in Main.projectile
        )
        {
            if (
                !projectile.active ||
                projectile.owner !=
                    Player.whoAmI ||
                projectile.type !=
                    SwordProjectileType
            )
            {
                continue;
            }

            projectile.Kill();
        }
    }

    public override void UpdateDead()
    {
        KillAllSwords();

        sheathLevel = 0;
        spawnCooldown = 0;
        previousMana =
            Player.statMana;
    }
}