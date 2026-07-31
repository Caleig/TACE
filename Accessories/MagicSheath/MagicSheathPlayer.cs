using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ThoriumAccessoryExpansion.Accessories.MagicSheath
{
    public class MagicSheathPlayer : ModPlayer
    {
        public int SheathLevel = 0; // 0,1,2,3
        public bool HasSpiritBlade = false;
        public List<int> SwordIndices = new List<int>();
        public int SpawnCooldown = 0;
        private int _prevMana = 0;

        public override void Initialize()
        {
            _prevMana = Player.statMana;
        }

        public override void ResetEffects()
        {
            SheathLevel = 0;
            HasSpiritBlade = false;
        }

        public override void UpdateDead()
        {
            SheathLevel = 0;
            HasSpiritBlade = false;
            SwordIndices.Clear();
            SpawnCooldown = 0;
        }

        public override void PostUpdate()
        {
            // 冷却
            if (SpawnCooldown > 0) SpawnCooldown--;

            // 清理无效剑
            SwordIndices.RemoveAll(idx =>
            {
                Projectile p = Main.projectile[idx];
                return !p.active || p.type != ModContent.ProjectileType<MagicSwordPro>();
            });

            // 若未装备任何剑鞘，且没有英灵破刃，清空剑
            if (SheathLevel == 0 && !HasSpiritBlade)
            {
                foreach (int idx in SwordIndices)
                    if (Main.projectile[idx].active) Main.projectile[idx].Kill();
                SwordIndices.Clear();
                _prevMana = Player.statMana;
                return;
            }

            // 上限检查
            int maxSwords = GetMaxSwords();
            while (SwordIndices.Count > maxSwords)
            {
                int idx = SwordIndices[0];
                if (Main.projectile[idx].active) Main.projectile[idx].Kill();
                SwordIndices.RemoveAt(0);
            }

            // 检测魔力消耗
            int currentMana = Player.statMana;
            int manaDiff = _prevMana - currentMana;
            if (manaDiff > 0 && SheathLevel > 0)
            {
                OnManaConsumed(manaDiff);
            }
            _prevMana = currentMana;

            // 低魔检测（仅在SheathLevel>0时）
            if (SheathLevel > 0)
                CheckLowMana();
        }

        // --- 辅助方法 ---
        private int GetMaxSwords()
        {
            switch (SheathLevel)
            {
                case 1: return 2;
                case 2: return 3;
                case 3: return 4;
                default: return 0;
            }
        }

        private float GetSpawnChance()
        {
            switch (SheathLevel)
            {
                case 1: return 0.10f;//10
                case 2: return 0.20f;//20
                case 3: return 0.25f;//25
                default: return 0f;
            }
        }

        private float GetLowManaThreshold()
        {
            switch (SheathLevel)
            {
                case 1: return 0.25f;    // 基础不触发
                case 2: return 0.30f;
                case 3: return 0.35f;
                default: return 0f;
            }
        }

        public int GetExtraPenetrate()
        {
            // 英灵级和泰拉级额外穿透2
            if(SheathLevel >= 2)
                return 2;
            if(HasSpiritBlade)
                return 2;
            return 0;
        }

        public int GetManaRestorePercent()
        {
            switch (SheathLevel)
            {
                case 1: return 10;
                case 2: return 15;
                case 3: return 15;
                default: return 0;
            }
        }

        // 面板伤害加成（供GlobalItem调用）
        public int GetFlatDamageBonus()
        {
            if (SheathLevel == 2) return 3;
            if (SheathLevel == 3) return 4;
            if (HasSpiritBlade) return 5; // 仅英灵破刃
            return 0;
        }

        public float GetManaCostMultiplier()
        {
            int count = SwordIndices.Count;
            return 1f + count * 0.05f;
        }

        public int GetDefenseBonus()
        {
            if (SheathLevel == 3)
                return 3 * SwordIndices.Count;
            return 0;
        }

        public float GetMagicDamageBonus()
        {
            return (SheathLevel == 3) ? 0.15f : 0f;
        }

        // 消耗魔力触发
        private void OnManaConsumed(int manaUsed)
        {
            if (SheathLevel == 0) return;
            if (SpawnCooldown > 0) return;
            if (SwordIndices.Count >= GetMaxSwords()) return;

            if (Main.rand.NextFloat() >= GetSpawnChance()) return;

            SpawnSword();
            SpawnCooldown = 120;
        }

        private void SpawnSword()
        {
            Player player = Player;
            int damage = 0;
            // 基础级：15，英灵级：30，泰拉级：手持武器50%
            if (SheathLevel == 1)
                damage = 15;
            else if (SheathLevel == 2)
                damage = 30;
            else if (SheathLevel == 3)
            {
                Item held = player.HeldItem;
                if (held != null && held.damage > 0)
                    damage = (int)(held.damage * 0.5f);
                else
                    damage = 30; // fallback
            }

            int extraPen = GetExtraPenetrate();
            Projectile proj = Projectile.NewProjectileDirect(
                player.GetSource_Accessory(null),
                player.Center - new Vector2(0, 40f),
                Vector2.Zero,
                ModContent.ProjectileType<MagicSwordPro>(),
                damage,
                2f,
                player.whoAmI,
                ai0: extraPen
            );
            proj.timeLeft = 600;
            SwordIndices.Add(proj.whoAmI);
        }

        private void CheckLowMana()
        {
            float threshold = GetLowManaThreshold();
            if (threshold == 0f) return;
            float ratio = (float)Player.statMana / Player.statManaMax2;
            if (ratio <= threshold)
            {
                // 调试：输出触发信息（测试后注释掉）
                Main.NewText($"低魔触发！魔力比例: {ratio}, 阈值: {threshold}");

                foreach (int idx in SwordIndices)
                {
                    Projectile p = Main.projectile[idx];
                    if (p.active && p.type == ModContent.ProjectileType<MagicSwordPro>())
                    {
                        var mp = p.ModProjectile as MagicSwordPro;
                        if (mp != null && !mp.Fired)
                        {
                            mp.FireAtMouse();
                            // 调试：输出发射信息
                            Main.NewText("已发射一把蕴魔剑");
                        }
                    }
                }
            }
        }
    }
}