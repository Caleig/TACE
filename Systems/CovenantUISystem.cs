using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Systems
{
    public class BarState : UIState
    {
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Player p = Main.LocalPlayer;
            CovenantPlayer cp = p.GetModPlayer<CovenantPlayer>();
            var screen = p.MountedCenter - Main.screenPosition;
            screen.Y += 40;
            Asset<Texture2D> tex = null;
            Asset<Texture2D> tex2 = null;
            bool draw = false;
            float value = 0, max = 1;

            // 检测是否有任意使用层数的圣约
            if (cp.BoneHasCovenant || cp.CursedHasCovenant || cp.FallenHasCovenant || cp.KarmaHasCovenant)
            {
                tex = ModContent.Request<Texture2D>("ThoriumAccessoryExpansion/Images/UI/CovenantFrame");
                value = cp.FallenRadianceStacks;
                max = cp.GetMaxStacks();
                draw = true;
            }

            if (draw)
            {
                var size = tex.Size().ToPoint();      // 应为 54x30
                int halfY = size.Y;               

                // 计算绘制位置（屏幕坐标，玩家脚下40像素，水平居中）
                screen -= new Vector2(size.X / 2f, 0);

                // ---------- 1. 绘制进度条框（背景，上半部分） ----------
                spriteBatch.Draw(tex.Value, screen, new Rectangle(0, 0, size.X, halfY), Color.White);

                // ---------- 鼠标悬停显示数值 ----------
                Rectangle bound = new((int)screen.X, (int)screen.Y, size.X, halfY);
                if (bound.Contains(Main.MouseScreen.ToPoint()))
                {
                    Main.hoverItemName = $"{value}/{max}";
                }
                tex2 = ModContent.Request<Texture2D>("ThoriumAccessoryExpansion/Images/UI/CovenantFill");

                // ---------- 2. 绘制进度条填充（下半部分） ----------
                // 填充宽度：最小17px，最大54px，按比例映射
                float fillWidth = 17f;
                if (max > 0)
                {
                    fillWidth = 17f + (value / max) * (54f - 17f);
                    fillWidth = MathHelper.Clamp(fillWidth, 17f, 54f);
                }

                // 填充矩形：从下半部分顶部开始，宽度为 fillWidth，高度为 halfY
                Rectangle fillRect = new Rectangle(0, 0, (int)fillWidth, halfY);

                // 在完全相同的位置绘制（与框位置一致）
                spriteBatch.Draw(tex2.Value, screen, fillRect, Color.White);
            }
        }
    }

    [Autoload(Side = ModSide.Client)]
    public class BarSystem : ModSystem
    {
        private UserInterface barUIF;
        private BarState bar;

        public override void Load()
        {
            bar = new();
            bar.Activate();
            barUIF = new UserInterface();
            barUIF.SetState(bar);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            barUIF?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Entity Health Bars"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "ThoriumAccessoryExpansion: Covenant Layer Bar",
                    delegate
                    {
                        barUIF.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.Game)
                );
            }
        }
    }
}