using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract;


public class CrystallineAmberCrystalProjectile : ModProjectile
{

    private int reflectionCount;

    private const int MaxReflection = 4;

    private bool initializedRotation;

    private readonly List<int> hitTargets = new();



    public override void SetDefaults()
    {

        Projectile.width = 12;
        Projectile.height = 12;


        Projectile.friendly = true;
        Projectile.hostile = false;


        Projectile.DamageType =
            DamageClass.Summon;


        Projectile.penetrate = -1;


        Projectile.timeLeft = 180;


        Projectile.tileCollide = true;


        Projectile.ignoreWater = true;


        Projectile.extraUpdates = 1;

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


        Lighting.AddLight(
            Projectile.Center,
            1f,
            0.7f,
            0.25f
        );


        Dust dust =
            Dust.NewDustDirect(
                Projectile.position,
                Projectile.width,
                Projectile.height,
                DustID.GoldFlame
            );


        dust.noGravity = true;

    }



    public override void OnHitNPC(
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {

        if (!hitTargets.Contains(target.whoAmI))
        {
            hitTargets.Add(target.whoAmI);
        }


        ConsumeReflection();


        NPC nextTarget =
            FindTarget();


        if (nextTarget != null)
        {

            Projectile.velocity =
                Vector2.Normalize(
                    nextTarget.Center -
                    Projectile.Center
                )
                * 12f;

        }

    }



    public override bool OnTileCollide(
        Vector2 oldVelocity)
    {

        ConsumeReflection();


        Reflect(oldVelocity);



        NPC nextTarget =
            FindTarget();


        if (nextTarget != null)
        {

            Projectile.velocity =
                Vector2.Normalize(
                    nextTarget.Center -
                    Projectile.Center
                )
                * 12f;

        }


        return false;

    }



    private void ConsumeReflection()
    {

        reflectionCount++;


        Projectile.damage =
            (int)(
                Projectile.damage * 0.8f
            );


        if (reflectionCount > MaxReflection)
        {
            Projectile.Kill();
        }

    }



    private void Reflect(
        Vector2 oldVelocity)
    {

        if (oldVelocity.X != Projectile.velocity.X)
        {
            Projectile.velocity.X =
                -oldVelocity.X;
        }


        if (oldVelocity.Y != Projectile.velocity.Y)
        {
            Projectile.velocity.Y =
                -oldVelocity.Y;
        }

    }



    private NPC FindTarget()
    {

        NPC result = null;


        float distance = 600f;



        foreach (NPC npc in Main.npc)
        {

            if (!npc.active)
                continue;


            if (!npc.CanBeChasedBy())
                continue;


            if (hitTargets.Contains(npc.whoAmI))
                continue;



            float currentDistance =
                Vector2.Distance(
                    Projectile.Center,
                    npc.Center
                );


            if (currentDistance < distance)
            {

                distance = currentDistance;

                result = npc;

            }

        }



        return result;

    }



    public override string Texture =>
        "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/CrystallineAmberCrystalProjectile";

}