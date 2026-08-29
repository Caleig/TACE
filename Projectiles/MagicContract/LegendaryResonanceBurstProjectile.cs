using Terraria;
using Terraria.ModLoader;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract;


public class LegendaryResonanceBurstProjectile :
    ModProjectile
{

    public override void SetDefaults()
    {

        Projectile.width = 16;
        Projectile.height = 16;

        Projectile.friendly = true;
        Projectile.hostile = false;

        Projectile.DamageType =
            DamageClass.Summon;

        Projectile.penetrate = 1;

        Projectile.timeLeft = 2;

        Projectile.tileCollide = false;

        Projectile.ignoreWater = true;

        Projectile.hide = true;

    }



    public override bool? CanHitNPC(
        NPC target)
    {

        return
            target.whoAmI
            ==
            (int)Projectile.ai[0];

    }



    public override void AI()
    {

        Projectile.velocity =
            Microsoft.Xna.Framework.Vector2.Zero;

    }



    public override string Texture =>
        "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/PrismaticCrystalProjectile";

}