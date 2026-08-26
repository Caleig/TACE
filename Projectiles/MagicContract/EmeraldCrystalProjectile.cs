using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract;


public class EmeraldCrystalProjectile : ModProjectile
{


    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 12;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }



    public override void SetDefaults()
    {

        Projectile.width = 8;
        Projectile.height = 8;


        Projectile.friendly = true;
        Projectile.hostile = false;


        Projectile.DamageType =
            DamageClass.Summon;


        Projectile.penetrate = 3;


        Projectile.timeLeft = 180;


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



    public override bool OnTileCollide(Vector2 oldVelocity)
    {

        Projectile.NewProjectile(
            Projectile.GetSource_FromThis(),
            Projectile.Center,
            Vector2.Zero,
            ModContent.ProjectileType<EmeraldCrystalClusterProjectile>(),
            (int)(Projectile.damage * 0.4f),
            0,
            Projectile.owner
        );


        Projectile.Kill();


        return false;
    }



    public override string Texture =>
        "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/EmeraldCrystalProjectile";

}