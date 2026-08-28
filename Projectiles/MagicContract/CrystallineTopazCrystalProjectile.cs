using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Systems.Projectiles;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract;


public class CrystallineTopazCrystalProjectile : ModProjectile
{

    private bool initializedRotation;


    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 10;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }



    public override void SetDefaults()
    {
        Projectile.width = 16;
        Projectile.height = 16;


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




    public override void OnSpawn(IEntitySource source)
    {

        for (int i = 0; i < 15; i++)
        {

            Dust dust =
                Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.GoldFlame
                );


            dust.velocity *= 1.5f;
            dust.noGravity = true;

        }

    }




    public override void AI()
    {

        if (!initializedRotation)
        {

            Projectile.rotation =
                Projectile.velocity.ToRotation()
                + MathHelper.PiOver2;


            initializedRotation = true;

        }


        Projectile.rotation += 0.15f;



        Dust dust =
            Dust.NewDustDirect(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.GoldFlame
            );


        dust.velocity *= 0.2f;
        dust.noGravity = true;



        Lighting.AddLight(
            Projectile.Center,
            1f,
            0.85f,
            0.3f
        );

    }




    public override void OnHitNPC(
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {

        int explosion =
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<CrystallineTopazExplosionProjectile>(),
                Projectile.damage / 2,
                0,
                Projectile.owner
                );


        Main.projectile[explosion]
        .GetGlobalProjectile<MagicContractGlobalProjectile>()
        .gemProjectile = true;


        Projectile.Kill();

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
                + Projectile.Size / 2
                - Main.screenPosition;


            float alpha =
                (Projectile.oldPos.Length - i)
                /
                (float)Projectile.oldPos.Length;



            Main.EntitySpriteDraw(
                texture,
                pos,
                null,
                Color.Gold * alpha * 0.4f,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

        }


        return true;

    }




    public override string Texture =>
        "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/CrystallineTopazCrystalProjectile";

}