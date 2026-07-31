using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.MagicSheath
{
    public class BlastPro : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.timeLeft = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 对命中的 NPC 施加狱炎（如果未由卷轴施加，这里也可以加）
            // 但卷轴已施加，这里只造成伤害
        }

        public override void AI()
        {
            // 造成范围伤害（对接近的 NPC）
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.Distance(Projectile.Center) < 120f)
                {
                    // 造成伤害，并应用狱炎（如果还没施加）
                    // 利用 Projectile 的伤害
                    npc.SimpleStrikeNPC(Projectile.damage, 0);
                    npc.AddBuff(323, 600);
                }
            }
            // 视觉效果（可选）
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(40, 40), 80, 80, DustID.Torch, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(6f, 6f);
            }
        }
    }
}