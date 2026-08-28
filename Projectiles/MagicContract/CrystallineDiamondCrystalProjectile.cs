using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract;


public class CrystallineDiamondCrystalProjectile : ModProjectile
{

    public override void SetStaticDefaults()
    {

        ProjectileID.Sets.TrailCacheLength[Type] = 10;
        ProjectileID.Sets.TrailingMode[Type] = 0;

    }



    public override void SetDefaults()
    {

        Projectile.width = 64;
        Projectile.height = 64;


        Projectile.friendly = true;
        Projectile.hostile = false;


        Projectile.DamageType =
            DamageClass.Summon;

        Projectile.penetrate = 1;


        Projectile.timeLeft = 180;

        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;


        Projectile.extraUpdates = 2;


        Projectile.scale = 1f;

    }



    public override void AI()
    {

        Projectile.rotation =
            Projectile.velocity.ToRotation()
            + MathHelper.PiOver2;



        Lighting.AddLight(
            Projectile.Center,
            1f,
            1f,
            1f
        );



        Dust dust =
            Dust.NewDustDirect(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.WhiteTorch
            );


        dust.noGravity = true;



        if (Main.rand.NextBool(3))
        {

            Dust trail =
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.WhiteTorch,
                    -Projectile.velocity * 0.15f,
                    100,
                    Color.White,
                    1.4f
                );


            trail.noGravity = true;

        }

    }



    public override void OnKill(
        int timeLeft)
    {

        for (int i = 0; i < 20; i++)
        {

            Dust dust =
                Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.WhiteTorch,
                    Main.rand.NextVector2Circular(
                        3f,
                        3f
                    ),
                    100,
                    Color.White,
                    1.5f
                );


            dust.noGravity = true;

        }



        Lighting.AddLight(
            Projectile.Center,
            1f,
            1f,
            1f
        );

    }



    public override bool PreDraw(
        ref Color lightColor)
    {

        Texture2D texture =
            ModContent.Request<Texture2D>(Texture).Value;



        Vector2 origin =
            new Vector2(
                texture.Width / 2f,
                texture.Height / 2f
            );



        Main.EntitySpriteDraw(
            texture,
            Projectile.Center - Main.screenPosition,
            null,
            lightColor,
            Projectile.rotation,
            origin,
            Projectile.scale,
            SpriteEffects.None,
            0
        );



        return false;

    }



    public override string Texture =>
        "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/CrystallineDiamondCrystalProjectile";

}