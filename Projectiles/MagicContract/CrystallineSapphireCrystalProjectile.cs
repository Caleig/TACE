using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract;


public class CrystallineSapphireCrystalProjectile : ModProjectile
{

    private const float WaveSpeed = 0.12f;
    private const float WaveDistance = 18f;



    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 14;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }



    public override void SetDefaults()
    {

        Projectile.width = 18;
        Projectile.height = 18;


        Projectile.friendly = true;
        Projectile.hostile = false;


        Projectile.DamageType =
            DamageClass.Summon;


        Projectile.penetrate = 1;


        Projectile.timeLeft = 120;


        Projectile.tileCollide = false;

        Projectile.ignoreWater = true;


        Projectile.extraUpdates = 1;

    }




    public override void AI()
    {

        Vector2 forward =
            Projectile.velocity.SafeNormalize(Vector2.UnitX);



        Vector2 side =
            new Vector2(
                -forward.Y,
                forward.X
            );



        float phase =
            Projectile.ai[0] == 0
            ? 0f
            : MathHelper.Pi;



        float offset =
            (float)Math.Sin(
                Main.GameUpdateCount * WaveSpeed
                + phase
            )
            *
            WaveDistance;



        Vector2 movement =
            forward *
            Projectile.velocity.Length();



        Projectile.velocity =
            movement;



        Projectile.position +=
            side *
            offset *
            0.08f;



        Projectile.rotation =
            Projectile.velocity.ToRotation()
            +
            MathHelper.PiOver2;



        Dust dust =
            Dust.NewDustDirect(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.BlueTorch
            );


        dust.noGravity = true;



        Lighting.AddLight(
            Projectile.Center,
            0.3f,
            0.7f,
            1f
        );

    }





    public override bool PreDraw(
        ref Color lightColor)
    {

        Texture2D texture =
            ModContent.Request<Texture2D>(Texture).Value;



        Vector2 origin =
            texture.Size() / 2;



        for (int i = 0; i < Projectile.oldPos.Length; i++)
        {

            Vector2 pos =
                Projectile.oldPos[i]
                +
                Projectile.Size / 2
                -
                Main.screenPosition;



            float alpha =
                (Projectile.oldPos.Length - i)
                /
                (float)Projectile.oldPos.Length;



            Main.EntitySpriteDraw(
                texture,
                pos,
                null,
                Color.Cyan * alpha * 0.35f,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

        }



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
        "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/CrystallineSapphireCrystalProjectile";

}