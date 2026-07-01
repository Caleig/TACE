using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.CursedCovenant;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod.Buffs;

namespace ThoriumAccessoryExpansion.Accessories.HeresyCovenant
{
    public class HeresyCovenantGlobalNPC : GlobalNPC
    {
        private static bool AnyPlayerHas()
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player p = Main.player[i];
                if (p.active && p.GetModPlayer<CovenantPlayer>().HeresyHasCovenant)
                    return true;
            }
            return false;
        }
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            bool hasShadowFlame = AnyNPCHasBuff(BuffID.ShadowFlame);
            bool hasLightCurse = AnyNPCHasBuff(ModContent.BuffType<LightCurse>());

            if(hasShadowFlame || hasLightCurse)
            {
                player.lifeRegen += 5;
            }
        }
        public static bool AnyNPCHasBuff(int buffType)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc.HasBuff(buffType))
                {
                    return true;
                }
            }
            return false;
        }
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            bool hasShadowFlame = AnyNPCHasBuff(BuffID.ShadowFlame);
            bool hasLightCurse = AnyNPCHasBuff(ModContent.BuffType<LightCurse>());
            if (hasShadowFlame || hasLightCurse)
            {
                Main.player[projectile.owner].lifeRegen += 5;
            }
        }
        public override void PostAI(NPC npc)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            if (!AnyPlayerHas()) return;

            bool hasShadowFlame = npc.HasBuff(BuffID.ShadowFlame);
            bool hasLightCurse = npc.HasBuff(ModContent.BuffType<LightCurse>());
            if (hasShadowFlame || hasLightCurse)
            {
                npc.localAI[0]++;

                if (npc.localAI[0] >= 30)
                {
                    npc.localAI[0] = 0;
                    int extraDamage = 5;
                    npc.SimpleStrikeNPC(extraDamage, 0, false, 0);
                }
            }
            else
            {
                npc.localAI[0] = 0;
            }
        }
    }
}