using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract;


public class EmeraldCrystalClusterProjectile : ModProjectile
{


    private int hitCount;



    public override void SetDefaults()
    {

        Projectile.width = 48;
        Projectile.height = 48;


        Projectile.friendly = true;
        Projectile.hostile = false;


        Projectile.DamageType =
            DamageClass.Summon;


        Projectile.penetrate = 2;


        Projectile.timeLeft = 180;


        Projectile.tileCollide = false;

    }



    public override void AI()
    {

        Lighting.AddLight(
            Projectile.Center,
            0.2f,
            1f,
            0.3f
        );


        Dust dust =
            Dust.NewDustDirect(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.GreenTorch
            );


        dust.noGravity = true;

    }



    public override bool? CanHitNPC(NPC target)
    {
        return hitCount < 2;
    }



    public override void OnHitNPC(
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {

        hitCount++;


        if (hitCount >= 2)
        {
            Projectile.Kill();
        }

    }



    public override string Texture =>
        "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/EmeraldCrystalClusterProjectile";

}