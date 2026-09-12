using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Projectiles.Scythe;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Players;

public class SoulOfScythePlayer : ModPlayer
{
    public bool SoulOfScythe;
    public bool SpiritFireScythe;
    public bool Anubis;
    public bool StyxBloodAnkh;
    public bool RadiantHolyAnkh;

    private int soulScytheTimer;
    private int lastSoulEssence = -1;
    public int BloodCharge;

    public const int MaxBloodCharge = 4;


    public override void ResetEffects()
    {
        SoulOfScythe = false;
        SpiritFireScythe = false;
        Anubis = false;
        StyxBloodAnkh = false;
        RadiantHolyAnkh = false;
    }


    public override void UpdateDead()
    {
        soulScytheTimer = 0;
        lastSoulEssence = -1;
        BloodCharge = 0;
    }


    public override void PostUpdate()
    {
        ThoriumPlayer thoriumPlayer =
            Player.GetThoriumPlayer();
        if (lastSoulEssence < 0)
        {
            lastSoulEssence =
                thoriumPlayer.soulEssence;
        }
        else
        {
            if (
                RadiantHolyAnkh &&
                thoriumPlayer.soulEssence <
                lastSoulEssence
            )
            {
                TriggerSoulEssenceBubble();
            }


            lastSoulEssence =
                thoriumPlayer.soulEssence;
        }
        if (!StyxBloodAnkh)
        {
            BloodCharge = 0;
        }
        if (soulScytheTimer > 0)
        {
            soulScytheTimer--;
        }


        if (!HasScytheAccessory())
        {
            return;
        }
        for (int i = 0; i < Main.maxProjectiles; i++)
        {
            Projectile projectile =
                Main.projectile[i];


            if (!projectile.active)
                continue;


            if (projectile.owner != Player.whoAmI)
                continue;


            if (projectile.ModProjectile is not ScythePro)
                continue;


            if (soulScytheTimer > 0)
                return;


            int projectileType;
            float damageMultiplier;
            if (RadiantHolyAnkh)
            {
                projectileType =
                    ModContent.ProjectileType<
                        RadiantHolyAnkhProjectile
                    >();

                damageMultiplier = 0.60f;
            }
            else if (StyxBloodAnkh)
            {
                projectileType =
                    ModContent.ProjectileType<
                        StyxBloodAnkhProjectile
                    >();

                damageMultiplier = 0.75f;
            }
            else if (Anubis)
            {
                projectileType =
                    ModContent.ProjectileType<
                        AnubisScytheProjectile
                    >();

                damageMultiplier = 0.75f;
            }
            else if (SpiritFireScythe)
            {
                projectileType =
                    ModContent.ProjectileType<
                        SpiritFireScytheProjectile
                    >();

                damageMultiplier = 0.50f;
            }
            else
            {
                projectileType =
                    ModContent.ProjectileType<
                        SoulScytheProjectile
                    >();

                damageMultiplier = 0.50f;
            }


            if (Main.myPlayer != Player.whoAmI)
                return;


            Vector2 direction =
                Main.MouseWorld - Player.Center;


            if (direction.LengthSquared() <= 0.001f)
                return;


            direction.Normalize();


            Projectile.NewProjectile(
                Player.GetSource_Misc(
                    "SoulOfScythe"),
                Player.Center,
                direction * 12f,
                projectileType,
                (int)(
                    projectile.damage *
                    damageMultiplier
                ),
                0f,
                Player.whoAmI,
                projectile.type
            );
            soulScytheTimer = 60;

            return;
        }
    }


    private bool HasScytheAccessory()
    {
        return
            SoulOfScythe ||
            SpiritFireScythe ||
            Anubis ||
            StyxBloodAnkh ||
            RadiantHolyAnkh;
    }
    private void TriggerSoulEssenceBubble()
    {
        if (Player.team == 0)
            return;


        Mod thorium =
            ModLoader.GetMod("ThoriumMod");


        int bubbleBuff =
            thorium.Find<ModBuff>(
                "BulwarkBubble"
            ).Type;


        for (int i = 0; i < Main.maxPlayers; i++)
        {
            Player ally =
                Main.player[i];


            if (!ally.active)
                continue;


            if (ally.dead)
                continue;


            if (ally.whoAmI == Player.whoAmI)
                continue;


            if (ally.team != Player.team)
                continue;


            if (
                Vector2.DistanceSquared(
                    Player.Center,
                    ally.Center
                ) > 800f * 800f
            )
            {
                continue;
            }


            ally.AddBuff(
                bubbleBuff,
                60
            );
        }
    }
    public void GiveSoulShield(
        int soulEssenceAmount)
    {
        if (!RadiantHolyAnkh)
            return;


        if (soulEssenceAmount <= 0)
            return;


        if (Player.team == 0)
            return;


        for (int i = 0; i < Main.maxPlayers; i++)
        {
            Player ally =
                Main.player[i];


            if (!ally.active)
                continue;


            if (ally.dead)
                continue;


            if (ally.whoAmI == Player.whoAmI)
                continue;


            if (ally.team != Player.team)
                continue;


            if (
                Vector2.DistanceSquared(
                    Player.Center,
                    ally.Center
                ) > 800f * 800f
            )
            {
                continue;
            }


            int shieldAmount =
                (int)(
                    ally.statLifeMax2 *
                    0.01f *
                    soulEssenceAmount
                );


            if (shieldAmount <= 0)
                continue;


            ThoriumPlayer allyThoriumPlayer =
                ally.GetThoriumPlayer();


            int available =
                ThoriumPlayer.ShieldHealthMax -
                allyThoriumPlayer.shieldHealth;


            int actualAmount =
                shieldAmount < available
                    ? shieldAmount
                    : available;


            if (actualAmount <= 0)
                continue;


            allyThoriumPlayer.shieldHealth +=
                actualAmount;

            ally.statLife +=
                actualAmount;
        }
    }
}