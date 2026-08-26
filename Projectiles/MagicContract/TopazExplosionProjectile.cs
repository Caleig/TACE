using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract;


public class TopazExplosionProjectile : ModProjectile
{


    public override void SetDefaults()
    {

        Projectile.width = 192;
        Projectile.height = 192;


        Projectile.friendly = true;
        Projectile.hostile = false;


        Projectile.DamageType =
            DamageClass.Summon;


        Projectile.penetrate = -1;


        Projectile.timeLeft = 5;


        Projectile.tileCollide = false;

        Projectile.ignoreWater = true;


        Projectile.alpha = 80;

    }




    public override void OnSpawn(IEntitySource source)
    {


        for (int i = 0; i < 35; i++)
        {

            Vector2 velocity =
                Main.rand.NextVector2CircularEdge(
                    5f,
                    5f
                );


            Dust dust =
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.GoldFlame,
                    velocity
                );


            dust.noGravity = true;

        }



        Lighting.AddLight(
            Projectile.Center,
            1f,
            0.8f,
            0.2f
        );

    }





    public override void AI()
    {


        Projectile.alpha -= 20;


        if (Projectile.alpha < 0)
            Projectile.alpha = 0;



        for (int i = 0; i < 8; i++)
        {

            Dust dust =
                Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.GoldFlame
                );


            dust.velocity *= 0.2f;

            dust.noGravity = true;

        }



        Lighting.AddLight(
            Projectile.Center,
            1f,
            0.8f,
            0.2f
        );


    }



    public override bool? CanDamage()
    {
        return true;
    }



    public override bool PreDraw(
        ref Color lightColor)
    {
        return false;
    }




    public override string Texture =>
        "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/TopazExplosionProjectile";

}