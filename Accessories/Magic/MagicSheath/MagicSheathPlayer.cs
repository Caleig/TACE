using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath
{
    public class MagicSheathPlayer : ModPlayer
    {
        public int SheathLevel = 0; 
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
            if (SpawnCooldown > 0) SpawnCooldown--;

            
            SwordIndices.RemoveAll(idx =>
            {
                Projectile p = Main.projectile[idx];
                return !p.active || p.type != ModContent.ProjectileType<MagicSwordPro>();
            });

            
            if (SheathLevel == 0 && !HasSpiritBlade)
            {
                foreach (int idx in SwordIndices)
                    if (Main.projectile[idx].active) Main.projectile[idx].Kill();
                SwordIndices.Clear();
                _prevMana = Player.statMana;
                return;
            }

            
            int maxSwords = GetMaxSwords();
            while (SwordIndices.Count > maxSwords)
            {
                int idx = SwordIndices[0];
                if (Main.projectile[idx].active) Main.projectile[idx].Kill();
                SwordIndices.RemoveAt(0);
            }

            
            int currentMana = Player.statMana;
            int manaDiff = _prevMana - currentMana;
            if (manaDiff > 0 && SheathLevel > 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                    OnManaConsumed(manaDiff);
            }
            _prevMana = currentMana;

            
            if (SheathLevel > 0 && Main.netMode != NetmodeID.MultiplayerClient)
                CheckLowMana();
        }

        
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
                case 1: return 0.10f;
                case 2: return 0.20f;
                case 3: return 0.25f;
                default: return 0f;
            }
        }

        private float GetLowManaThreshold()
        {
            switch (SheathLevel)
            {
                case 1: return 0.25f;
                case 2: return 0.30f;
                case 3: return 0.35f;
                default: return 0f;
            }
        }

        public int GetExtraPenetrate()
        {
            if (SheathLevel >= 2)
                return 2;
            if (HasSpiritBlade)
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

        public int GetFlatDamageBonus()
        {
            if (SheathLevel == 2) return 3;
            if (SheathLevel == 3) return 4;
            if (HasSpiritBlade) return 5;
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
            return (SheathLevel == 3) ? 0f : 0f;
        }

        
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
                    damage = 30;
            }

            int extraPen = GetExtraPenetrate();
            int swordCount = SwordIndices.Count; 
            Projectile proj = Projectile.NewProjectileDirect(
                player.GetSource_Accessory(null),
                player.Center - new Vector2(0, 40f),
                Vector2.Zero,
                ModContent.ProjectileType<MagicSwordPro>(),
                damage,
                2f,
                player.whoAmI,
                ai0: swordCount,
                ai1: extraPen
            );
            proj.timeLeft = 600;
            
        }

        
        private void CheckLowMana()
        {
            float threshold = GetLowManaThreshold();
            if (threshold == 0f) return;
            float ratio = (float)Player.statMana / Player.statManaMax2;
            if (ratio <= threshold)
            {
                foreach (int idx in SwordIndices)
                {
                    Projectile p = Main.projectile[idx];
                    if (p.active && p.type == ModContent.ProjectileType<MagicSwordPro>())
                    {
                        var mp = p.ModProjectile as MagicSwordPro;
                        if (mp != null && !mp.IsFired)
                        {
                            mp.FireAtMouse();
                        }
                    }
                }
            }
        }
    }
}