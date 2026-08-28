using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract;


public class CrystallineRubyCrystalProjectile : ModProjectile
{

    public override void SetDefaults()
    {

        Projectile.width = 8;
        Projectile.height = 8;


        Projectile.friendly = true;
        Projectile.hostile = false;


        Projectile.DamageType =
            DamageClass.Summon;


        Projectile.penetrate = 1;


        Projectile.timeLeft = 120;


        Projectile.tileCollide = true;


        Projectile.ignoreWater = true;


        Projectile.extraUpdates = 2;

    }



    public override void AI()
    {

        Projectile.rotation =
            Projectile.velocity.ToRotation()
            + MathHelper.PiOver2;



        Lighting.AddLight(
            Projectile.Center,
            1f,
            0.2f,
            0.2f
        );


        Dust dust =
            Dust.NewDustDirect(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.RedTorch
            );


        dust.noGravity = true;



        if (Main.rand.NextBool(3))
        {

            Dust trail =
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.RedTorch,
                    -Projectile.velocity * 0.15f,
                    100,
                    Color.White,
                    1.2f
                );


            trail.noGravity = true;

        }

    }



    public override void OnKill(
        int timeLeft)
    {

        for (int i = 0; i < 5; i++)
        {

            Dust dust =
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.RedTorch,
                    Main.rand.NextVector2Circular(
                        2f,
                        2f
                    ),
                    100,
                    Color.White,
                    1.3f
                );


            dust.noGravity = true;

        }

    }



    public override string Texture =>
        "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/CrystallineRubyCrystalProjectile";

}