using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.UI.DarkMatter;

public class DarkMatterKineticBar : UIElement
{
    private const int BarWidth = 90;
    private const int BarHeight = 28;
    private const int FillSourceX = 26;
    private const int FillSourceY = 6;
    private const int FillSourceWidth = 62;
    private const int FillSourceHeight = 20;

    private readonly Texture2D _emptyTexture;
    private readonly Texture2D _fillTexture;

    public DarkMatterKineticBar()
    {
        Width.Set(BarWidth, 0f);
        Height.Set(BarHeight, 0f);

        _emptyTexture = ModContent.Request<Texture2D>(
            "ThoriumAccessoryExpansion/UI/DarkMatter/DarkMatterKineticBarEmpty"
        ).Value;

        _fillTexture = ModContent.Request<Texture2D>(
            "ThoriumAccessoryExpansion/UI/DarkMatter/DarkMatterKineticBarFill"
        ).Value;
    }

    protected override void DrawSelf(
        SpriteBatch spriteBatch)
    {
        Player player = Main.LocalPlayer;

        if (!player.active || player.dead)
        {
            return;
        }

        MeleeGauntletPlayer gauntlet =
            player.GetModPlayer<MeleeGauntletPlayer>();
        if (!gauntlet.HasDarkMatterGauntlet)
        {
            return;
        }

        Rectangle barRectangle =
            GetInnerDimensions().ToRectangle();
        spriteBatch.Draw(
            _emptyTexture,
            barRectangle,
            Color.White
        );
        float progress =
            MathHelper.Clamp(
                gauntlet.DarkMatterKinetic / 100f,
                0f,
                1f
            );

        if (progress <= 0f)
        {
            return;
        }

        int fillWidth =
            (int)(FillSourceWidth * progress);

        if (fillWidth <= 0)
        {
            return;
        }
        Rectangle sourceRectangle =
            new Rectangle(
                FillSourceX,
                FillSourceY,
                fillWidth,
                FillSourceHeight
            );
        Rectangle destinationRectangle =
            new Rectangle(
                barRectangle.X + FillSourceX,
                barRectangle.Y + FillSourceY,
                fillWidth,
                FillSourceHeight
            );

        spriteBatch.Draw(
            _fillTexture,
            destinationRectangle,
            sourceRectangle,
            Color.White
        );
    }
}