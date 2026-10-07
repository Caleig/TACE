using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.UI.DarkMatter;

public class DarkMatterKineticDrawLayer : PlayerDrawLayer
{
    private Asset<Texture2D>? _emptyTexture;
    private Asset<Texture2D>? _fillTexture;
    private const float UIScale = 0.6f;
    private const float HorizontalOffset = 0f;
    private const float VerticalOffset = 8f;
    private const int FillSourceX = 26;
    private const int FillSourceY = 6;
    private const int FillSourceWidth = 62;
    private const int FillSourceHeight = 20;

    public override Position GetDefaultPosition()
    {
        return new BeforeParent(
            PlayerDrawLayers.Torso
        );
    }

    public override bool GetDefaultVisibility(
        PlayerDrawSet drawInfo)
    {
        Player player =
            drawInfo.drawPlayer;
        if (drawInfo.shadow != 0f)
        {
            return false;
        }
        if (player.whoAmI != Main.myPlayer)
        {
            return false;
        }

        MeleeGauntletPlayer gauntlet =
            player.GetModPlayer<
                MeleeGauntletPlayer
            >();
        return gauntlet.HasDarkMatterGauntlet;
    }

    protected override void Draw(
        ref PlayerDrawSet drawInfo)
    {
        Player player =
            drawInfo.drawPlayer;

        MeleeGauntletPlayer gauntlet =
            player.GetModPlayer<
                MeleeGauntletPlayer
            >();

        if (!gauntlet.HasDarkMatterGauntlet)
        {
            return;
        }
        _emptyTexture ??=
            ModContent.Request<Texture2D>(
                "ThoriumAccessoryExpansion/UI/DarkMatter/DarkMatterKineticBarEmpty"
            );

        _fillTexture ??=
            ModContent.Request<Texture2D>(
                "ThoriumAccessoryExpansion/UI/DarkMatter/DarkMatterKineticBarFill"
            );

        Texture2D empty =
            _emptyTexture.Value;

        Texture2D fill =
            _fillTexture.Value;
        Rectangle emptySource =
            empty.Frame();
        Vector2 center =
            player.Bottom -
            Main.screenPosition;

        center.X +=
            HorizontalOffset;

        center.Y +=
            VerticalOffset;
        int emptyWidth =
            (int)(
                emptySource.Width *
                UIScale
            );

        int emptyHeight =
            (int)(
                emptySource.Height *
                UIScale
            );
        Vector2 emptyPosition =
            center -
            new Vector2(
                emptyWidth / 2f,
                0f
            );

        emptyPosition =
            new Vector2(
                (int)emptyPosition.X,
                (int)emptyPosition.Y
            );

        Rectangle emptyDestination =
            new Rectangle(
                (int)emptyPosition.X,
                (int)emptyPosition.Y,
                emptyWidth,
                emptyHeight
            );
        drawInfo.DrawDataCache.Add(
            new DrawData(
                empty,
                emptyDestination,
                emptySource,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0
            )
        );
        float ratio =
            MathHelper.Clamp(
                gauntlet.DarkMatterKinetic /
                100f,
                0f,
                1f
            );

        if (ratio <= 0f)
        {
            return;
        }
        int originalFillWidth =
            (int)(
                FillSourceWidth *
                ratio
            );

        if (originalFillWidth <= 0)
        {
            return;
        }
        Rectangle fillSource =
            new Rectangle(
                FillSourceX,
                FillSourceY,
                originalFillWidth,
                FillSourceHeight
            );
        int fillWidth =
            (int)(
                originalFillWidth *
                UIScale
            );

        int fillHeight =
            (int)(
                FillSourceHeight *
                UIScale
            );
        int fillX =
            (int)(
                emptyPosition.X +
                FillSourceX * UIScale
            );

        int fillY =
            (int)(
                emptyPosition.Y +
                FillSourceY * UIScale
            );

        Rectangle fillDestination =
            new Rectangle(
                fillX,
                fillY,
                fillWidth,
                fillHeight
            );

        drawInfo.DrawDataCache.Add(
            new DrawData(
                fill,
                fillDestination,
                fillSource,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0
            )
        );
    }
}