using Terraria;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;

namespace ThoriumAccessoryExpansion.Accessories.Healer.MichaelasAid
{
    public class MichaelasAidGlobalProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[projectile.owner];
            if (!player.active) return;

            CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
            if (!cp.MichaelasHasCovenant) return;
            if (projectile.DamageType != ModContent.GetInstance<HealerDamage>()) return;

            target.AddBuff(ModContent.BuffType<SupremeWrath>(), 600);
            if (target.HasBuff(ModContent.BuffType<SupremeWrath>()))
            {
                ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
                if (thoriumPlayer != null)
                    thoriumPlayer.soulEssence += 1;
            }
        }
    }
}