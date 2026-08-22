using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;

namespace ThoriumAccessoryExpansion.Accessories.Healer.KarmaCovenant
{
    public class KarmaCovenantGlobalProjectile : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.owner < 0 || projectile.owner >= Main.maxPlayers)
                return;

            Player player = Main.player[projectile.owner];
            if (player == null || !player.active)
                return;

            CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
            if (cp == null)
                return;

            var healerDamage = ModContent.GetInstance<HealerDamage>();
            if (hit.DamageType == null || hit.DamageType != healerDamage)
                return;

            if (cp.KarmaHasCovenant)
            {
                cp.FallenRadianceStacks = (int)MathHelper.Clamp(cp.FallenRadianceStacks + 4, 0, CovenantPlayer.GlobalMaxStacks);
                cp.FallenRadianceTimer = 3600;
            }
        }
    }
}