using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract;


public class AquamarineCrystalProjectile : ModProjectile
{

    private int hitCount;


    public override void SetDefaults()
    {

        Projectile.width = 32;
        Projectile.height = 32;


        Projectile.friendly = true;
        Projectile.hostile = false;


        Projectile.DamageType =
            DamageClass.Summon;


        Projectile.penetrate = 5;


        Projectile.timeLeft = 180;


        Projectile.tileCollide = false;
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
            0.2f,
            0.8f,
            1f
        );



        Dust dust =
            Dust.NewDustDirect(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.BlueTorch
            );


        dust.noGravity = true;

    }



    public override void OnHitNPC(
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {

        hitCount++;


        Projectile.damage =
            (int)(Projectile.damage * 0.8f);



        if (hitCount >= 5)
        {
            Projectile.Kill();
        }

    }



    public override bool PreDraw(
        ref Color lightColor)
    {

        Texture2D texture =
            ModContent.Request<Texture2D>(Texture).Value;


        Main.EntitySpriteDraw(
            texture,
            Projectile.Center - Main.screenPosition,
            null,
            lightColor,
            Projectile.rotation,
            texture.Size() / 2f,
            1f,
            SpriteEffects.None,
            0
        );


        return false;

    }



    public override string Texture =>
        "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/AquamarineCrystalProjectile";

}