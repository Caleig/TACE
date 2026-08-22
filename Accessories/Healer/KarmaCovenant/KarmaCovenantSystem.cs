using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.Healer.KarmaCovenant;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Accessories.Healer.KarmaCovenant
{
    public class KarmaCovenantSystem : ModSystem
    {
        private Hook _thoriumHealTargetHook;
        private Hook _healEffectHook;
        private static bool _isSharing = false;

        public override void Load()
        {
            Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
            if (thoriumMod == null)
            {
                Mod.Logger.Warn("未找到 ThoriumMod，业果圣契的部分效果不会生效。");
                return;
            }

            Type helperType = thoriumMod.Code.GetType("ThoriumMod.Utilities.ProjectileHelper");
            if (helperType == null)
            {
                Mod.Logger.Warn("未找到 ThoriumMod.Utilities.ProjectileHelper，业果圣契的部分效果不会生效。");
                return;
            }

            MethodInfo targetMethod = helperType.GetMethod("ThoriumHealTarget",
                BindingFlags.Public | BindingFlags.Static);
            if (targetMethod == null)
            {
                Mod.Logger.Warn("未找到 ThoriumHealTarget 方法，业果圣契的部分效果不会生效。");
                return;
            }

            _thoriumHealTargetHook = new Hook(targetMethod,
                new Func<Func<Projectile, Player, int, bool, bool, bool, bool, bool, object, bool>,
                        Projectile, Player, int, bool, bool, bool, bool, bool, object, bool>(
                    OnThoriumHealTarget
                ));

            MethodInfo healEffectMethod = typeof(Player).GetMethod("HealEffect",
                new Type[] { typeof(int), typeof(bool) });
            if (healEffectMethod == null)
            {
                Mod.Logger.Warn("未找到 Player.HealEffect 方法，业果圣契的共享治疗将不会生效。");
                return;
            }

            _healEffectHook = new Hook(healEffectMethod,
                new Action<Action<Player, int, bool>, Player, int, bool>(
                    OnHealEffect
                ));
        }

        public override void Unload()
        {
            _thoriumHealTargetHook?.Dispose();
            _thoriumHealTargetHook = null;
            _healEffectHook?.Dispose();
            _healEffectHook = null;
        }

        private static bool OnThoriumHealTarget(
            Func<Projectile, Player, int, bool, bool, bool, bool, bool, object, bool> orig,
            Projectile projectile,
            Player target,
            int healAmount,
            bool onHealEffects,
            bool bonusHealing,
            bool self,
            bool ignoreSetTarget,
            bool statistics,
            object customHealing)
        {
            if (_isSharing)
                return orig(projectile, target, healAmount, onHealEffects, bonusHealing, self, ignoreSetTarget, statistics, customHealing);

            if (Main.netMode == NetmodeID.MultiplayerClient)
                return orig(projectile, target, healAmount, onHealEffects, bonusHealing, self, ignoreSetTarget, statistics, customHealing);

            CovenantPlayer cp = target.GetModPlayer<CovenantPlayer>();
            if (cp != null && cp.KarmaHasCovenant && healAmount > 0)
            {
                if (cp.FallenRadianceStacks >= CovenantPlayer.GlobalMaxStacks)
                {
                    float radius = 800f;
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player p = Main.player[i];
                        if (p != target && p.active && !p.dead && Vector2.Distance(target.Center, p.Center) <= radius)
                        {
                            int sharedHeal = healAmount;
                            _isSharing = true;
                            PlayerHelper.HealLife(p, sharedHeal, target, false, true);
                            _isSharing = false;
                        }
                    }
                }
            }

            return orig(projectile, target, healAmount, onHealEffects, bonusHealing, self, ignoreSetTarget, statistics, customHealing);
        }

        private static void OnHealEffect(Action<Player, int, bool> orig, Player player, int healAmount, bool broadcast)
        {
            CovenantPlayer cp = player.GetModPlayer<CovenantPlayer>();
            if (cp != null && cp.KarmaHasCovenant && healAmount > 0)
            {
                if (cp.FallenRadianceStacks >= CovenantPlayer.GlobalMaxStacks)
                {
                    float radius = 800f;
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        Player p = Main.player[i];
                        if (p != player && p.active && !p.dead && Vector2.Distance(player.Center, p.Center) <= radius)
                        {
                            int sharedHeal = healAmount;
                            _isSharing = true;
                            PlayerHelper.HealLife(p, sharedHeal, player, false, true);
                            _isSharing = false;
                        }
                    }
                }
            }

            orig(player, healAmount, broadcast);
        }
    }
}