using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Healer.MichaelasAid
{
    public class MichaelasAidSystem : ModSystem
    {
        private Hook _thoriumHealTargetHook;

        public override void Load()
        {
            Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
            if (thoriumMod == null) return;

            Type helperType = thoriumMod.Code.GetType("ThoriumMod.Utilities.ProjectileHelper");
            if (helperType == null) return;

            MethodInfo targetMethod = helperType.GetMethod("ThoriumHealTarget",
                BindingFlags.Public | BindingFlags.Static);
            if (targetMethod == null) return;

            _thoriumHealTargetHook = new Hook(targetMethod,
                new Func<Func<Projectile, Player, int, bool, bool, bool, bool, bool, object, bool>,
                        Projectile, Player, int, bool, bool, bool, bool, bool, object, bool>(
                    OnThoriumHealTarget
                ));
        }

        public override void Unload()
        {
            _thoriumHealTargetHook?.Dispose();
            _thoriumHealTargetHook = null;
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
            Player healer = Main.player[projectile.owner];
            if (healer != null && healer.active)
            {
                CovenantPlayer cp = healer.GetModPlayer<CovenantPlayer>();
                if (cp.MichaelasHasCovenant)
                {
                    healAmount += 2;
                }
            }

            return orig(projectile, target, healAmount, onHealEffects, bonusHealing, self, ignoreSetTarget, statistics, customHealing);
        }
    }
}