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

            
            if (cp.BoneHasCovenant || cp.CursedHasCovenant || cp.FallenHasCovenant || cp.KarmaHasCovenant)
            {
                tex = ModContent.Request<Texture2D>("ThoriumAccessoryExpansion/Images/UI/CovenantFrame");
                value = cp.FallenRadianceStacks;
                max = cp.GetMaxStacks();
                draw = true;
            }

            if (draw)
            {
                var size = tex.Size().ToPoint();      
                int halfY = size.Y;               

                
                screen -= new Vector2(size.X / 2f, 0);

                
                spriteBatch.Draw(tex.Value, screen, new Rectangle(0, 0, size.X, halfY), Color.White);

                
                Rectangle bound = new((int)screen.X, (int)screen.Y, size.X, halfY);
                if (bound.Contains(Main.MouseScreen.ToPoint()))
                {
                    Main.hoverItemName = $"{value}/{max}";
                }
                tex2 = ModContent.Request<Texture2D>("ThoriumAccessoryExpansion/Images/UI/CovenantFill");

                
                
                float fillWidth = 17f;
                if (max > 0)
                {
                    fillWidth = 17f + (value / max) * (54f - 17f);
                    fillWidth = MathHelper.Clamp(fillWidth, 17f, 54f);
                }

                
                Rectangle fillRect = new Rectangle(0, 0, (int)fillWidth, halfY);

                
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