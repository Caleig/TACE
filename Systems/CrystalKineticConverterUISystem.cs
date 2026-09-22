using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Systems;

public class CrystalKineticConverterUISystem : ModSystem
{
    private Asset<Texture2D> _frameTexture;
    private Asset<Texture2D> _fillTexture;

    private const int MaxStoredSonicDamage = 1500;

    public override void Load()
    {
        if (Main.dedServ)
            return;

        _frameTexture = ModContent.Request<Texture2D>(
            "ThoriumAccessoryExpansion/UI/CrystalKineticConverter/CrystalKineticConverterFrame"
        );

        _fillTexture = ModContent.Request<Texture2D>(
            "ThoriumAccessoryExpansion/UI/CrystalKineticConverter/CrystalKineticConverterFill"
        );
    }

    public override void Unload()
    {
        _frameTexture = null;
        _fillTexture = null;
    }

    public override void ModifyInterfaceLayers(
        List<GameInterfaceLayer> layers)
    {
        if (Main.dedServ)
            return;

        int index = layers.FindIndex(
            layer => layer.Name.Equals("Vanilla: Mouse Text")
        );

        if (index == -1)
            return;

        layers.Insert(
            index,
            new LegacyGameInterfaceLayer(
                "ThoriumAccessoryExpansion: Crystal Kinetic Converter",
                DrawUI,
                InterfaceScaleType.UI
            )
        );
    }

    private bool DrawUI()
    {
        Player player = Main.LocalPlayer;

        if (
            player == null ||
            player.dead ||
            player.ghost
        )
        {
            return true;
        }

        CrystalKineticConverterPlayer converterPlayer =
            player.GetModPlayer<
                CrystalKineticConverterPlayer
            >();

        if (!converterPlayer.HasCrystalKineticConverter)
            return true;

        if (
            _frameTexture == null ||
            _fillTexture == null
        )
        {
            return true;
        }

        DrawConverterUI(converterPlayer);

        return true;
    }

    private void DrawConverterUI(
        CrystalKineticConverterPlayer converterPlayer)
    {
        SpriteBatch spriteBatch = Main.spriteBatch;

        Texture2D frame = _frameTexture.Value;
        Texture2D fill = _fillTexture.Value;

        Vector2 position =
            new Vector2(20f, 300f);

        float ratio = MathHelper.Clamp(
            converterPlayer.StoredSonicDamage /
            (float)MaxStoredSonicDamage,
            0f,
            1f
        );

        if (ratio > 0f)
        {
            int fillHeight =
                (int)(fill.Height * ratio);

            if (fillHeight > 0)
            {
                Rectangle sourceRectangle =
                    new Rectangle(
                        0,
                        fill.Height - fillHeight,
                        fill.Width,
                        fillHeight
                    );

                Rectangle destinationRectangle =
                    new Rectangle(
                        (int)position.X,
                        (int)(
                            position.Y +
                            frame.Height -
                            fillHeight
                        ),
                        fill.Width,
                        fillHeight
                    );

                spriteBatch.Draw(
                    fill,
                    destinationRectangle,
                    sourceRectangle,
                    Color.White
                );
            }
        }

        spriteBatch.Draw(
            frame,
            position,
            Color.White
        );
    }
}