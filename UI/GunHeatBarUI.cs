using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.UI;

public class GunHeatBarUI : PlayerDrawLayer
{
    private static Texture2D HeatBarTexture =>
        ModContent.Request<Texture2D>(
            "ThoriumAccessoryExpansion/Images/UI/GunHeatBar_1"
        ).Value;


    private static Texture2D HeatEnergyTexture =>
        ModContent.Request<Texture2D>(
            "ThoriumAccessoryExpansion/Images/UI/GunHeatBar_2"
        ).Value;

    public override Position GetDefaultPosition()
    {
        return PlayerDrawLayers.BeforeFirstVanillaLayer;
    }

    public override bool GetDefaultVisibility(
        PlayerDrawSet drawInfo)
    {
        Player player =
            drawInfo.drawPlayer;


        if (
            player.whoAmI != Main.myPlayer
        )
        {
            return false;
        }


        if (
            !player.active ||
            player.dead
        )
        {
            return false;
        }


        GunModificationPlayer modification =
            player.GetModPlayer<
                GunModificationPlayer
            >();


        return
            modification.HasHeatModification &&
            modification.HeatMaximum > 0;
    }

    protected override void Draw(
        ref PlayerDrawSet drawInfo)
    {
        Player player =
            drawInfo.drawPlayer;


        GunModificationPlayer modification =
            player.GetModPlayer<
                GunModificationPlayer
            >();


        int heat =
            modification.Heat;


        int maximum =
            modification.HeatMaximum;


        if (
            maximum <= 0
        )
        {
            return;
        }
        float progress =
            MathHelper.Clamp(
                (float)heat / maximum,
                0f,
                1f
            );
        const int BarWidth = 24;
        const int BarHeight = 80;
        const int EnergyTop = 30;
        const int EnergyHeight = 50;
        const float BehindDistance = 32f;


        float horizontalDistance =
            player.width / 2f +
            BehindDistance +
            BarWidth / 2f;


        Vector2 barCenter =
            player.Center -
            Main.screenPosition;


        barCenter.X -=
            player.direction *
            horizontalDistance;
        barCenter.Y =
            player.Center.Y -
            Main.screenPosition.Y;
        Vector2 barTopLeft =
            new Vector2(
                (int)(
                    barCenter.X -
                    BarWidth / 2f
                ),
                (int)(
                    barCenter.Y -
                    BarHeight / 2f
                )
            );
        drawInfo.DrawDataCache.Add(
            new DrawData(
                HeatBarTexture,
                barTopLeft,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0
            )
        );
        int visibleHeight =
            (int)(
                EnergyHeight *
                progress
            );


        if (
            visibleHeight <= 0
        )
        {
            return;
        }
        int sourceY =
            EnergyTop +
            EnergyHeight -
            visibleHeight;


        Rectangle sourceRectangle =
            new Rectangle(
                0,
                sourceY,
                BarWidth,
                visibleHeight
            );
        Vector2 energyPosition =
            new Vector2(
                barTopLeft.X,
                barTopLeft.Y +
                sourceY
            );


        drawInfo.DrawDataCache.Add(
            new DrawData(
                HeatEnergyTexture,
                energyPosition,
                sourceRectangle,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0
            )
        );
    }
}