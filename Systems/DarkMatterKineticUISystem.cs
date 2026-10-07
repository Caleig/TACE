using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using ThoriumAccessoryExpansion.Players;
using ThoriumAccessoryExpansion.UI.DarkMatter;

namespace ThoriumAccessoryExpansion.Systems;

[Autoload(Side = ModSide.Client)]
public class DarkMatterKineticUISystem : ModSystem
{
    private UserInterface? _userInterface;
    private DarkMatterKineticUI? _uiState;

    public override void Load()
    {
        _uiState =
            new DarkMatterKineticUI();

        _userInterface =
            new UserInterface();

        _userInterface.SetState(
            _uiState
        );
    }

    public override void Unload()
    {
        _userInterface = null;
        _uiState = null;
    }

    public override void UpdateUI(
        GameTime gameTime)
    {
        if (
            _userInterface == null ||
            _uiState == null
        )
        {
            return;
        }

        UpdatePosition();

        _userInterface.Update(
            gameTime
        );
    }

    private void UpdatePosition()
    {
        if (_uiState == null)
        {
            return;
        }

        Player player =
            Main.LocalPlayer;

        if (
            !player.active ||
            player.dead
        )
        {
            return;
        }

        MeleeGauntletPlayer gauntlet =
            player.GetModPlayer<
                Players.MeleeGauntletPlayer
            >();

        if (!gauntlet.HasDarkMatterGauntlet)
        {
            return;
        }
        Vector2 playerBottom =
            player.Bottom +
            new Vector2(
                0f,
                player.gfxOffY
            ) -
            Main.screenPosition;
        const float barWidth = 90f;

        float x =
            playerBottom.X -
            barWidth / 2f;
        float y =
            playerBottom.Y +
            8f;

        _uiState.KineticBar.Left.Set(
            x,
            0f
        );

        _uiState.KineticBar.Top.Set(
            y,
            0f
        );
    }

    public override void ModifyInterfaceLayers(
        List<GameInterfaceLayer> layers)
    {
        if (_userInterface == null)
        {
            return;
        }

        int index =
            layers.FindIndex(
                layer =>
                    layer.Name.Equals(
                        "Vanilla: Mouse Text"
                    )
            );

        if (index == -1)
        {
            return;
        }

        layers.Insert(
            index,
            new LegacyGameInterfaceLayer(
                "ThoriumAccessoryExpansion: Dark Matter Kinetic",
                () =>
                {
                    _userInterface.Draw(
                        Main.spriteBatch,
                        Main.gameTimeCache
                    );

                    return true;
                },
                InterfaceScaleType.Game
            )
        );
    }
}