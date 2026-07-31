using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace ThoriumAccessoryExpansion.Accessories.MagicSheath
{
    public class MagicSheathGlobalItem : GlobalItem
    {
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            var mp = player.GetModPlayer<MagicSheathPlayer>();
            if (mp == null) return;

            // 魔法武器面板伤害加成
            if (item.DamageType == DamageClass.Magic)
            {
                // 来自英灵的破刃 (+5) 或英灵级 (+3) 或泰拉级 (+4)
                int flatBonus = 0;
                if (mp.HasSpiritBlade || mp.SheathLevel == 2) // 英灵破刃或英灵级鞘
                    flatBonus += 5;
                if (mp.SheathLevel == 2)
                    flatBonus += 3; // 英灵级额外+3（总共+8？但描述是面板+3，这里单独处理）
                if (mp.SheathLevel == 3)
                    flatBonus += 4; // 泰拉级+4

                // 但英灵破刃和剑鞘同时佩戴时，可能叠加？不，升级后玩家只会佩戴更高一级，所以不会同时。
                // 按等级确定：
                if (mp.SheathLevel == 1)
                    flatBonus = 0;
                else if (mp.SheathLevel == 2)
                    flatBonus = 5 + 3; // 英灵破刃效果+剑鞘效果，但英灵破刃作为材料已不存在，所以只算剑鞘的+3？
                // 实际上描述：英灵的蕴魔剑鞘效果包括“魔法武器面板伤害+3”，所以只加3
                // 英灵的破刃单独佩戴时+5，但不影响剑鞘
                // 所以分情况：
                if (mp.SheathLevel == 2)
                    flatBonus = 3;
                else if (mp.SheathLevel == 3)
                    flatBonus = 4;
                else if (mp.SheathLevel == 0 && mp.HasSpiritBlade)
                    flatBonus = 5;
                // 但HasSpiritBlade只在单独佩戴英灵破刃时为true

                // 另外泰拉级有魔法伤害+15%
                if (mp.SheathLevel == 3)
                {
                    damage += 0.15f;
                }

                damage.Flat += flatBonus;
            }
        }

        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
        {
            // 蓝耗增加（每把剑+5%）
            var mp = player.GetModPlayer<MagicSheathPlayer>();
            if (mp.SheathLevel > 0 && item.DamageType == DamageClass.Magic)
            {
                mult *= mp.GetManaCostMultiplier();
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            // 防御加成（仅泰拉级）
            var mp = player.GetModPlayer<MagicSheathPlayer>();
            if (mp.SheathLevel == 3)
            {
                player.statDefense += mp.GetDefenseBonus();
            }
        }
    }
}