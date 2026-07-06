using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod.Buffs;

namespace ThoriumAccessoryExpansion.Accessories.HeresyCovenant
{
    public class HeresyCovenantGlobalNPC : GlobalNPC
    {
        // 每个 NPC 实例独立计时器
        public override bool InstancePerEntity => true;

        private int customDotTimer;

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

        // 当玩家用物品攻击 NPC 时
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            if (!AnyPlayerHas()) return;

            bool hasDebuff = npc.HasBuff(BuffID.ShadowFlame) || npc.HasBuff(ModContent.BuffType<LightCurse>());
            if (hasDebuff)
            {
                // 增加玩家生命再生速率 (+5/s)
                player.lifeRegen += 5;
            }
        }

        // 当玩家用弹幕攻击 NPC 时
        public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
        {
            if (!AnyPlayerHas()) return;

            bool hasDebuff = npc.HasBuff(BuffID.ShadowFlame) || npc.HasBuff(ModContent.BuffType<LightCurse>());
            if (hasDebuff)
            {
                Player player = Main.player[projectile.owner];
                if (player != null && player.active)
                    player.lifeRegen += 5;
            }
        }

        // 每帧对 NPC 额外造成 DoT 伤害（暗影焰/光之咒翻倍）
        public override void PostAI(NPC npc)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;

            if (!AnyPlayerHas()) return;

            // 跳过 Boss 等，避免干扰
            if (npc.boss || npc.friendly || npc.lifeMax <= 5) return;

            bool hasDebuff = npc.HasBuff(BuffID.ShadowFlame) || npc.HasBuff(ModContent.BuffType<LightCurse>());
            if (hasDebuff)
            {
                // 使用实例计时器（每个 NPC 独立）
                customDotTimer++;
                if (customDotTimer >= 30)
                {
                    customDotTimer = 0;
                    int extraDamage = 5;
                    // 造成额外伤害，不触发玩家交互
                    npc.SimpleStrikeNPC(extraDamage, 0, false, 0, noPlayerInteraction: true);
                }
            }
            else
            {
                // 如果没有减益，重置计时器
                customDotTimer = 0;
            }
        }
    }
}