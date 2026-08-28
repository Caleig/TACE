using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Systems.Projectiles;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract;


public class CrystallineRubyBurstProjectile : ModProjectile
{

    private Vector2 shootDirection;



    public override void SetDefaults()
    {
        Projectile.width = 2;
        Projectile.height = 2;

        Projectile.friendly = false;
        Projectile.hostile = false;

        Projectile.timeLeft = 60;

        Projectile.tileCollide = false;
        Projectile.hide = true;
    }



    public override void OnSpawn(
        Terraria.DataStructures.IEntitySource source)
    {
        shootDirection =
            Projectile.velocity.SafeNormalize(Vector2.UnitX);
    }



    public override void AI()
    {

        Player player =
            Main.player[Projectile.owner];


        if (!player.active)
        {
            Projectile.Kill();
            return;
        }


        Projectile.Center =
            player.Center;



        Projectile.ai[1]++;

        if (Projectile.ai[1] >= 12)
        {

            Projectile.ai[1] = 0;


            FireRuby();


            Projectile.ai[0]++;

            if (Projectile.ai[0] >= 4)
            {
                Projectile.Kill();
            }

        }

    }



    private void FireRuby()
    {

        int proj =
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                shootDirection * 12f,
                ModContent.ProjectileType<CrystallineRubyCrystalProjectile>(),
                Projectile.damage,
                Projectile.knockBack,
                Projectile.owner
            );


        Main.projectile[proj]
            .GetGlobalProjectile<MagicContractGlobalProjectile>()
            .gemProjectile = true;

    }



    public override string Texture =>
        "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/RubyCrystalProjectile";

}