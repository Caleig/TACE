using Terraria;
using Terraria.ModLoader;
using Terraria.ID;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract;


public class CrystallineEmeraldCrystalClusterProjectile : ModProjectile
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


        Projectile.penetrate = 4;


        Projectile.timeLeft = 240;


        Projectile.tileCollide = false;

    }



    public override void AI()
    {

        Lighting.AddLight(
            Projectile.Center,
            0.3f,
            1f,
            0.4f
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



    public override bool? CanHitNPC(
        NPC target)
    {

        return hitCount < 4;

    }



    public override void OnHitNPC(
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {

        hitCount++;


        if (hitCount >= 4)
        {
            Projectile.Kill();
        }

    }



    public override string Texture =>
        "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/CrystallineEmeraldCrystalClusterProjectile";

}