using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Systems.Projectiles;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract;


public class CrystallineOpalCrystalProjectile : ModProjectile
{

    public override void SetDefaults()
    {

        Projectile.width = 14;
        Projectile.height = 14;


        Projectile.friendly = true;
        Projectile.hostile = false;


        Projectile.DamageType =
            DamageClass.Summon;


        Projectile.penetrate = 1;


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
            0.9f,
            0.9f,
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

    }



    public override void OnHitNPC(
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {

        for (int i = 0; i < 4; i++)
        {

            Vector2 velocity =
                Main.rand.NextVector2CircularEdge(
                    1f,
                    1f
                )
                * 8f;


            int proj =
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    velocity,
                    ModContent.ProjectileType<CrystallineOpalShardProjectile>(),
                    (int)(Projectile.damage * 0.35f),
                    0,
                    Projectile.owner
                );

            Main.projectile[proj]
                .ai[0] = target.whoAmI;


            Main.projectile[proj]
                .GetGlobalProjectile<MagicContractGlobalProjectile>()
                .gemProjectile = true;

        }


        Projectile.Kill();

    }



    public override string Texture =>
        "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/CrystallineOpalCrystalProjectile";

}