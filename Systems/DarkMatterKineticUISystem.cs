using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using ThoriumAccessoryExpansion.UI.DarkMatter;

namespace ThoriumAccessoryExpansion.Systems;

public class DarkMatterKineticUISystem : ModSystem
{
    private DarkMatterKineticUI? _uiState;
    private UserInterface? _userInterface;

    public override void Load()
    {
        if (Main.dedServ)
        {
            return;
        }

        _uiState = new DarkMatterKineticUI();

        _userInterface = new UserInterface();
        _userInterface.SetState(_uiState);
    }

    public override void Unload()
    {
        _uiState = null;
        _userInterface = null;
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (Main.dedServ)
        {
            return;
        }

        _userInterface?.Update(gameTime);

        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (_uiState == null)
        {
            return;
        }

        _uiState.KineticBar.Left.Set(
            Main.screenWidth - 110f,
            0f
        );

        _uiState.KineticBar.Top.Set(
            120f,
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

        int resourceBarsIndex =
            layers.FindIndex(
                layer =>
                    layer.Name.Equals(
                        "Vanilla: Resource Bars"
                    )
            );

        if (resourceBarsIndex == -1)
        {
            return;
        }

        layers.Insert(
            resourceBarsIndex + 1,
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
                InterfaceScaleType.UI
            )
        );
    }
}