using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.MagicSheath
{
    public class ScrollPlayer : ModPlayer
    {
        // 存储激活的卷轴类型ID（0~4），最多2个
        public List<int> ActiveScrolls = new List<int>();

        public override void ResetEffects()
        {
            // 不重置，因为卷轴激活状态是持久的
        }

        // 检查某卷轴是否激活
        public bool IsScrollActive(int typeID)
        {
            return ActiveScrolls.Contains(typeID);
        }

        // 切换卷轴激活状态
        public void ToggleScroll(int typeID)
        {
            if (ActiveScrolls.Contains(typeID))
            {
                // 已激活则关闭
                ActiveScrolls.Remove(typeID);
            }
            else
            {
                // 未激活则添加
                if (ActiveScrolls.Count >= 2)
                {
                    // 移除最老的（第一个）
                    ActiveScrolls.RemoveAt(0);
                }
                ActiveScrolls.Add(typeID);
            }
        }
    }
}