using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract;


public class CrystallineTopazExplosionProjectile : ModProjectile
{


    public override void SetDefaults()
    {

        Projectile.width = 256;
        Projectile.height = 256;


        Projectile.friendly = true;
        Projectile.hostile = false;


        Projectile.DamageType =
            DamageClass.Summon;


        Projectile.penetrate = -1;


        Projectile.timeLeft = 5;


        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;


        Projectile.alpha = 60;

    }




    public override void OnSpawn(IEntitySource source)
    {

        for (int i = 0; i < 50; i++)
        {

            Vector2 velocity =
                Main.rand.NextVector2CircularEdge(
                    6f,
                    6f
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
            0.85f,
            0.3f
        );

    }




    public override void AI()
    {

        Projectile.alpha -= 15;


        if (Projectile.alpha < 0)
            Projectile.alpha = 0;



        for (int i = 0; i < 10; i++)
        {

            Dust dust =
                Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.GoldFlame
                );


            dust.velocity *= 0.25f;
            dust.noGravity = true;

        }


        Lighting.AddLight(
            Projectile.Center,
            1f,
            0.85f,
            0.3f
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
        "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/CrystallineTopazExplosionProjectile";

}