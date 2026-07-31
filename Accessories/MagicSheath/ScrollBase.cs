using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.MagicSheath
{
    /// <summary>
    /// 卷轴基类，所有刃契卷轴继承此类
    /// </summary>
    public abstract class ScrollBase : ModItem
    {
        // 子类需重写返回卷轴类型ID（0~4）
        public abstract int ScrollTypeID { get; }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.UseSound = SoundID.Item4;
            Item.rare = ItemRarityID.LightRed;
        }

        public override bool CanUseItem(Player player)
        {
            // 仅当玩家佩戴了至少一种剑鞘时才能切换（但允许切换状态，提示）
            var mp = player.GetModPlayer<ScrollPlayer>();
            // 如果未装备剑鞘，仍然可以切换，但会提示无效（可选）
            return true;
        }

        public override bool? UseItem(Player player)
        {
            var sp = player.GetModPlayer<ScrollPlayer>();
            sp.ToggleScroll(ScrollTypeID);
            // 显示切换提示
            string status = sp.IsScrollActive(ScrollTypeID) ? "激活" : "关闭";
            Main.NewText($"刃契 {Item.Name} 已{status}", Color.Orange);
            return true;
        }

        // 子类实现配方
        public abstract override void AddRecipes();
    }
}