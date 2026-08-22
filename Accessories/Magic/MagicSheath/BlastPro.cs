using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath
{
    public class BlastPro : ModProjectile
    {
        private bool _damageDealt = false;

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.timeLeft = 1; 
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            
            if (Main.netMode != NetmodeID.MultiplayerClient && !_damageDealt)
            {
                _damageDealt = true;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (npc.active && !npc.friendly && npc.Distance(Projectile.Center) < 120f)
                    {
                        npc.SimpleStrikeNPC(Projectile.damage, 0);
                        npc.AddBuff(BuffID.ShadowFlame, 600);
                    }
                }
            }

            
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(40, 40), 80, 80, DustID.Torch, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(6f, 6f);
            }
        }
    }
}