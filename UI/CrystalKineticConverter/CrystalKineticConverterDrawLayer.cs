using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.UI.CrystalKineticConverter;

public class CrystalKineticConverterDrawLayer : PlayerDrawLayer
{
    private Asset<Texture2D> _frameTexture;
    private Asset<Texture2D> _fillTexture;

    private const float UIScale = 0.6f;

    private const float HorizontalOffset = 42f;
    private const float VerticalOffset = -20f;

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
            return false;

        if (player.whoAmI != Main.myPlayer)
            return false;

        CrystalKineticConverterPlayer converterPlayer =
            player.GetModPlayer<
                CrystalKineticConverterPlayer
            >();

        return converterPlayer.HasCrystalKineticConverter;
    }

    protected override void Draw(
        ref PlayerDrawSet drawInfo)
    {
        Player player =
            drawInfo.drawPlayer;

        CrystalKineticConverterPlayer converterPlayer =
            player.GetModPlayer<
                CrystalKineticConverterPlayer
            >();

        if (!converterPlayer.HasCrystalKineticConverter)
            return;

        _frameTexture ??=
            ModContent.Request<Texture2D>(
                "ThoriumAccessoryExpansion/UI/CrystalKineticConverter/CrystalKineticConverterFrame"
            );

        _fillTexture ??=
            ModContent.Request<Texture2D>(
                "ThoriumAccessoryExpansion/UI/CrystalKineticConverter/CrystalKineticConverterFill"
            );

        Texture2D frame =
            _frameTexture.Value;

        Texture2D fill =
            _fillTexture.Value;

        Rectangle frameSource =
            frame.Frame();

        Vector2 center =
            drawInfo.Center -
            Main.screenPosition;
        center.X -=
            HorizontalOffset * player.direction;

        center.Y +=
            VerticalOffset;
        int frameWidth =
            (int)(
                frameSource.Width *
                UIScale
            );

        int frameHeight =
            (int)(
                frameSource.Height *
                UIScale
            );

        Vector2 framePosition =
            center -
            new Vector2(
                frameWidth / 2f,
                frameHeight / 2f
            );

        framePosition =
            new Vector2(
                (int)framePosition.X,
                (int)framePosition.Y
            );
        Rectangle frameDestination =
            new Rectangle(
                (int)framePosition.X,
                (int)framePosition.Y,
                frameWidth,
                frameHeight
            );

        drawInfo.DrawDataCache.Add(
            new DrawData(
                frame,
                frameDestination,
                frameSource,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                0
            )
        );
        float ratio =
            MathHelper.Clamp(
                converterPlayer.StoredSonicDamage /
                (float)CrystalKineticConverterPlayer.StorageMaximum,
                0f,
                1f
            );

        if (ratio <= 0f)
            return;

        int originalFillHeight =
            (int)(
                fill.Height *
                ratio
            );

        if (originalFillHeight <= 0)
            return;
        Rectangle fillSource =
            new Rectangle(
                0,
                fill.Height - originalFillHeight,
                fill.Width,
                originalFillHeight
            );
        int fillWidth =
            (int)(
                fill.Width *
                UIScale
            );

        int fillHeight =
            (int)(
                originalFillHeight *
                UIScale
            );
        int fillX =
            (int)(
                center.X -
                fillWidth / 2f
            );
        int fillY =
            (int)(
                framePosition.Y +
                frameHeight -
                fillHeight
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