using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Projectiles.Scythe;

public class LifeSpiritProjectile : ModProjectile
{
    public override string Texture =>
        "Terraria/Images/Projectile_" +
        ProjectileID.EnchantedBeam;


    public override void SetDefaults()
    {
        Projectile.width = 18;
        Projectile.height = 18;

        Projectile.friendly = false;
        Projectile.hostile = false;

        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;

        Projectile.timeLeft = 120;
    }


    public override void AI()
    {
        Player owner =
            Main.player[Projectile.owner];


        int targetIndex =
            FindLowestHealthAlly(owner);


        if (targetIndex < 0)
        {
            Projectile.Kill();
            return;
        }


        Player target =
            Main.player[targetIndex];


        Vector2 direction =
            target.Center -
            Projectile.Center;


        if (
            direction.LengthSquared() <=
            24f * 24f
        )
        {
            if (
                Main.netMode !=
                NetmodeID.MultiplayerClient
            )
            {
                target.Heal(2);
            }


            Projectile.Kill();
            return;
        }


        direction.Normalize();


        Projectile.velocity =
            Vector2.Lerp(
                Projectile.velocity,
                direction * 8f,
                0.12f
            );


        Lighting.AddLight(
            Projectile.Center,
            0.3f,
            0.8f,
            1.0f
        );
    }


    private static int FindLowestHealthAlly(
        Player owner)
    {
        if (owner.team == 0)
            return -1;


        int targetIndex = -1;

        float lowestRatio =
            float.MaxValue;


        for (int i = 0; i < Main.maxPlayers; i++)
        {
            Player player =
                Main.player[i];


            if (!player.active)
                continue;


            if (player.dead)
                continue;


            if (player.whoAmI == owner.whoAmI)
                continue;


            if (player.team != owner.team)
                continue;


            if (
                player.statLife >=
                player.statLifeMax2
            )
            {
                continue;
            }


            float ratio =
                (float)player.statLife /
                player.statLifeMax2;


            if (ratio < lowestRatio)
            {
                lowestRatio = ratio;
                targetIndex = player.whoAmI;
            }
        }


        return targetIndex;
    }
}