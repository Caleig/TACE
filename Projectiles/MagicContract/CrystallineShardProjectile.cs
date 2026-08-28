using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace ThoriumAccessoryExpansion.Projectiles.MagicContract
{

    public class CrystallineShardProjectile : ModProjectile
    {

        private const float SearchRange = 300f;
        private const float HomingSpeed = 14f;
        private const float HomingStrength = 0.12f;



        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 6;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }



        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.DamageType = DamageClass.Summon;

            Projectile.penetrate = 1;

            Projectile.timeLeft = 120;

            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.extraUpdates = 1;
        }



        public override void AI()
        {
            Projectile.rotation += 0.25f;


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


            Dust dust =
                Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.GemAmethyst
                );


            dust.noGravity = true;


            Lighting.AddLight(
                Projectile.Center,
                0.5f,
                0.2f,
                0.8f
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

                float d =
                    Vector2.Distance(
                        Projectile.Center,
                        npc.Center
                    );



                if (d < distance)
                {
                    distance = d;
                    target = npc;
                }

            }


            return target;

        }




        public override void OnKill(int timeLeft)
        {

            for (int i = 0; i < 4; i++)
            {

                Dust dust =
                    Dust.NewDustDirect(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.GemDiamond
                    );


                dust.velocity *= 1.5f;
                dust.noGravity = true;

            }

        }



        public override string Texture =>
            "ThoriumAccessoryExpansion/Images/Projectiles/MagicContract/CrystallineShardProjectile";

    }

}