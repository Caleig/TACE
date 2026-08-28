using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract;


public class CrystallineOpalShardProjectile : ModProjectile
{

    private const float SearchRange = 600f;



    public override void SetDefaults()
    {

        Projectile.width = 8;
        Projectile.height = 8;


        Projectile.friendly = true;
        Projectile.hostile = false;


        Projectile.DamageType =
            DamageClass.Summon;


        Projectile.penetrate = 1;


        Projectile.timeLeft = 120;


        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;


        Projectile.extraUpdates = 2;

    }



    public override void AI()
    {

        NPC target =
            FindTarget();



        if (target != null)
        {

            Vector2 direction =
                Vector2.Normalize(
                    target.Center -
                    Projectile.Center
                );



            Projectile.velocity =
                Vector2.Lerp(
                    Projectile.velocity,
                    direction * 12f,
                    0.08f
                );

        }



        Projectile.rotation =
            Projectile.velocity.ToRotation()
            +
            MathHelper.PiOver2;



        Lighting.AddLight(
            Projectile.Center,
            0.7f,
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



    public override bool? CanHitNPC(
        NPC target)
    {

        int originalTarget =
            (int)Projectile.ai[0];


        if (target.whoAmI == originalTarget)
            return false;


        return null;

    }



    private NPC FindTarget()
    {

        NPC result = null;


        float distance =
            SearchRange;



        int originalTarget =
            (int)Projectile.ai[0];



        foreach (NPC npc in Main.npc)
        {

            if (!npc.active)
                continue;


            if (!npc.CanBeChasedBy())
                continue;


            if (npc.whoAmI == originalTarget)
                continue;



            float currentDistance =
                Vector2.Distance(
                    Projectile.Center,
                    npc.Center
                );


            if (currentDistance < distance)
            {

                distance =
                    currentDistance;


                result =
                    npc;

            }

        }



        return result;

    }



    public override string Texture =>
        "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/CrystallineOpalShardProjectile";

}