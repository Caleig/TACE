using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract;

public class CrystallineAmethystCrystalProjectile : ModProjectile
{
    private const float SearchRange = 400f;
    private const float HomingSpeed = 16f;
    private const float HomingStrength = 0.12f;

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 14;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }


    public override void SetDefaults()
    {
        Projectile.width = 14;
        Projectile.height = 14;

        Projectile.friendly = true;
        Projectile.hostile = false;

        Projectile.DamageType = DamageClass.Summon;

        Projectile.penetrate = 1;

        Projectile.timeLeft = 180;

        Projectile.tileCollide = true;
        Projectile.ignoreWater = true;

        Projectile.knockBack = 0f;

        Projectile.extraUpdates = 1;
    }


    public override void OnSpawn(IEntitySource source)
    {
        for (int i = 0; i < 12; i++)
        {
            Dust dust = Dust.NewDustDirect(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.GemAmethyst
            );

            dust.velocity *= 1.8f;
            dust.noGravity = true;
        }
    }


    public override void AI()
    {
        NPC target = FindTarget();


        if (target != null)
        {
            Vector2 direction =
                target.Center - Projectile.Center;

            direction.Normalize();


            Projectile.velocity =
                Vector2.Lerp(
                    Projectile.velocity,
                    direction * HomingSpeed,
                    HomingStrength
                );
        }


        Projectile.rotation =
            Projectile.velocity.ToRotation()
            + MathHelper.PiOver2;



        Dust trail = Dust.NewDustDirect(
            Projectile.position,
            Projectile.width,
            Projectile.height,
            DustID.GemAmethyst
        );


        trail.velocity *= 0.25f;
        trail.noGravity = true;


        Lighting.AddLight(
            Projectile.Center,
            0.9f,
            0.5f,
            1f
        );
    }



    private NPC FindTarget()
    {
        NPC target = null;

        float distance = SearchRange;


        foreach (NPC npc in Main.npc)
        {
            if (!npc.active)
                continue;

            if (npc.friendly)
                continue;

            if (npc.dontTakeDamage)
                continue;

            if (npc.type == NPCID.TargetDummy)
                continue;

            float currentDistance =
                Vector2.Distance(
                    Projectile.Center,
                    npc.Center
                );


            if (currentDistance < distance)
            {
                distance = currentDistance;
                target = npc;
            }
        }


        return target;
    }



    public override void OnHitNPC(
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {
        Projectile.Kill();
    }



    public override void OnKill(int timeLeft)
    {
        for (int i = 0; i < 20; i++)
        {
            Dust dust = Dust.NewDustDirect(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.GemAmethyst
            );

            dust.velocity *= 2.2f;
            dust.noGravity = true;
        }


        Lighting.AddLight(
            Projectile.Center,
            1f,
            0.6f,
            1f
        );
    }



    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture =
            ModContent.Request<Texture2D>(Texture).Value;


        Vector2 origin =
            texture.Size() / 2f;



        for (int i = 0; i < Projectile.oldPos.Length; i++)
        {
            Vector2 drawPosition =
                Projectile.oldPos[i]
                + Projectile.Size / 2f
                - Main.screenPosition;


            float alpha =
                (Projectile.oldPos.Length - i)
                /
                (float)Projectile.oldPos.Length;



            Main.EntitySpriteDraw(
                texture,
                drawPosition,
                null,
                new Color(220, 150, 255)
                    * alpha
                    * 0.7f,
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
        "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/CrystallineAmethystCrystalProjectile";
}