using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.NPCs
{
    public class GemMarkGlobalNPC : GlobalNPC
    {

        public override bool InstancePerEntity => true;


        public GemType gemType = GemType.None;

        public int gemMarkTime;

        public int gemDamage;

        public int gemOwner;

        public int prismaticResonanceTime;

        public int prismaticResonanceOwner = -1;

        public int legendaryResonanceDropCooldown;


        public override void ResetEffects(NPC npc)
        {

            if (gemMarkTime > 0)
            {
                gemMarkTime--;

                if (gemMarkTime <= 0)
                {
                    ClearMark();
                }
            }

            if (legendaryResonanceDropCooldown > 0)
            {
                legendaryResonanceDropCooldown--;
            }

            if (prismaticResonanceTime > 0)
            {
                prismaticResonanceTime--;

                if (prismaticResonanceTime <= 0)
                {
                    ClearPrismaticResonance();
                }
            }

        }

        public void AddPrismaticResonance(
            int time,
            int owner)
        {
            prismaticResonanceTime = time;

            prismaticResonanceOwner = owner;
        }


        public bool HasPrismaticResonance()
        {
            return prismaticResonanceTime > 0;
        }


        public bool HasPrismaticResonance(
            int owner)
        {
            return
                prismaticResonanceTime > 0
                &&
                prismaticResonanceOwner == owner;
        }


        private void ClearPrismaticResonance()
        {
            prismaticResonanceTime = 0;

            prismaticResonanceOwner = -1;
        }


        public void AddGemMark(
            GemType type,
            int time,
            int damage,
            int owner)
        {

            gemType = type;

            gemMarkTime = time;

            gemDamage = damage;

            gemOwner = owner;

        }



        public bool HasGemMark(
            GemType type)
        {
            return gemType == type;
        }



        public bool ConsumeGemMark()
        {

            if (gemType == GemType.None)
                return false;


            ClearMark();

            return true;

        }



        private void ClearMark()
        {

            gemType = GemType.None;

            gemMarkTime = 0;

            gemDamage = 0;

            gemOwner = -1;

        }




        public override void PostDraw(
           NPC npc,
           SpriteBatch spriteBatch,
           Vector2 screenPos,
           Color drawColor)
        {

            if (Main.netMode == NetmodeID.Server)
                return;



            if (
                gemType == GemType.None
                &&
                prismaticResonanceTime <= 0
            )
                return;



            float time =
                Main.GameUpdateCount * 0.05f;



            switch (gemType)
            {

                case GemType.Amethyst:

                    DrawAmethystMark(
                        npc,
                        time
                    );

                    break;

                case GemType.CrystallineAmethyst:

                    DrawCrystallineAmethystMark(
                        npc,
                        time
                    );

                    break;

                case GemType.Topaz:

                    DrawTopazMark(
                        npc,
                        time
                    );

                    break;

                case GemType.CrystallineTopaz:

                    DrawCrystallineTopazMark(
                        npc,
                        time
                    );

                    break;

                case GemType.Sapphire:

                    DrawSapphireMark(
                        npc,
                        time
                    );

                    break;

                case GemType.CrystallineSapphire:

                    DrawCrystallineSapphireMark(
                        npc,
                        time
                    );

                    break;

                case GemType.Emerald:

                    DrawEmeraldMark(
                        npc,
                        time
                    );

                    break;

                case GemType.CrystallineEmerald:

                    DrawCrystallineEmeraldMark(
                        npc,
                        time
                    );

                    break;

                case GemType.Amber:

                    DrawAmberMark(
                        npc,
                        time
                    );

                    break;

                case GemType.CrystallineAmber:

                    DrawCrystallineAmberMark(
                        npc,
                        time
                    );

                    break;

                case GemType.Ruby:

                    DrawRubyMark(
                        npc,
                        time
                    );

                    break;

                case GemType.CrystallineRuby:

                    DrawCrystallineRubyMark(
                        npc,
                        time
                    );

                    break;

                case GemType.Diamond:

                    DrawDiamondMark(
                        npc,
                        time
                    );

                    break;

                case GemType.CrystallineDiamond:

                    DrawCrystallineDiamondMark(
                        npc,
                        time
                    );

                    break;

                case GemType.Opal:

                    DrawOpalMark(
                        npc,
                        time
                    );

                    break;

                case GemType.CrystallineOpal:

                    DrawCrystallineOpalMark(
                        npc,
                        time
                    );

                    break;

                case GemType.Aquamarine:

                    DrawAquamarineMark(
                        npc,
                        time
                    );

                    break;

                case GemType.CrystallineAquamarine:

                    DrawCrystallineAquamarineMark(
                        npc,
                        time
                    );

                    break;

                case GemType.Prismatic:

                    DrawPrismaticMark(
                        npc,
                        time
                    );

                    break;

            }



            if (prismaticResonanceTime > 0)
            {
                DrawPrismaticResonance(
                    npc,
                    time
                );
            }

        }

        private void DrawAmethystMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 3; i++)
            {

                float angle =
                    time +
                    i * MathHelper.TwoPi / 3f;



                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    * 25f;



                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.PurpleTorch,
                        Vector2.Zero,
                        120,
                        Color.White,
                        1.2f
                    );


                dust.noGravity = true;

            }

        }

        private void DrawCrystallineAmethystMark(
            NPC npc,
            float time)
        {
            for (int i = 0; i < 4; i++)
            {
                float angle =
                    time * 1.5f
                    +
                    i * MathHelper.TwoPi / 4f;


                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    * 28f;


                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.PurpleTorch,
                        Vector2.Zero,
                        120,
                        Color.White,
                        1.4f
                    );

                dust.noGravity = true;
            }

            if (Main.rand.NextBool(3))
            {
                Vector2 velocity =
                    Main.rand.NextVector2CircularEdge(
                        1f,
                        1f
                    )
                    * 2f;


                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center,
                        DustID.GemAmethyst,
                        velocity,
                        100,
                        Color.White,
                        1.3f
                    );


                dust.noGravity = true;
            }


            Lighting.AddLight(
                npc.Center,
                0.5f,
                0.2f,
                0.8f
            );
        }

        private void DrawTopazMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 4; i++)
            {

                float angle =
                    -time * 1.8f
                    +
                    i * MathHelper.TwoPi / 4f;



                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    * 20f;



                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.GoldFlame,
                        Vector2.Zero,
                        120,
                        Color.White,
                        1.25f
                    );


                dust.noGravity = true;

            }

        }

        private void DrawCrystallineTopazMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 6; i++)
            {

                float angle =
                    time * 2f
                    +
                    i * MathHelper.TwoPi / 6f;



                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    *
                    30f;



                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.GoldFlame,
                        Vector2.Zero,
                        100,
                        Color.White,
                        1.5f
                    );


                dust.noGravity = true;

            }


            if (Main.rand.NextBool(3))
            {

                Vector2 velocity =
                    Main.rand.NextVector2CircularEdge(
                        1f,
                        1f
                    )
                    *
                    2f;



                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center,
                        DustID.GemTopaz,
                        velocity,
                        100,
                        Color.White,
                        1.4f
                    );


                dust.noGravity = true;

            }



            Lighting.AddLight(
                npc.Center,
                1f,
                0.8f,
                0.25f
            );

        }

        private void DrawSapphireMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 2; i++)
            {

                float angle =
                    time * 2f
                    +
                    i * MathHelper.Pi;



                for (int j = 0; j < 6; j++)
                {

                    float progress =
                        j / 6f;



                    float radius =
                        15f +
                        progress * 15f;



                    Vector2 offset =
                        new Vector2(
                            (float)System.Math.Cos(
                                angle + progress * MathHelper.TwoPi),
                            (float)System.Math.Sin(
                                angle + progress * MathHelper.TwoPi)
                        )
                        * radius;



                    Dust dust =
                        Dust.NewDustPerfect(
                            npc.Center + offset,
                            DustID.BlueTorch,
                            Vector2.Zero,
                            100,
                            Color.White,
                            1.15f
                        );


                    dust.noGravity = true;

                }

            }


            Lighting.AddLight(
                npc.Center,
                0.2f,
                0.5f,
                1f
            );

        }

        private void DrawCrystallineSapphireMark(
    NPC npc,
    float time)
        {

            for (int i = 0; i < 2; i++)
            {

                float angle =
                    time * 2f
                    +
                    i * MathHelper.Pi;



                for (int j = 0; j < 8; j++)
                {

                    float progress =
                        j / 8f;



                    float radius =
                        20f +
                        progress * 18f;



                    Vector2 offset =
                        new Vector2(
                            (float)System.Math.Cos(
                                angle + progress * MathHelper.TwoPi),
                            (float)System.Math.Sin(
                                angle + progress * MathHelper.TwoPi)
                        )
                        *
                        radius;



                    Dust dust =
                        Dust.NewDustPerfect(
                            npc.Center + offset,
                            DustID.BlueTorch,
                            Vector2.Zero,
                            100,
                            Color.White,
                            1.3f
                        );


                    dust.noGravity = true;

                }

            }


            Lighting.AddLight(
                npc.Center,
                0.3f,
                0.7f,
                1f
            );

        }

        private void DrawEmeraldMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 5; i++)
            {

                float angle =
                    time * 1.2f
                    +
                    i * MathHelper.TwoPi / 5f;



                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    *
                    28f;



                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.GreenTorch,
                        Vector2.Zero,
                        100,
                        Color.White,
                        1.3f
                    );


                dust.noGravity = true;

            }



            if (Main.rand.NextBool(4))
            {

                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center,
                        DustID.GreenTorch,
                        Vector2.UnitY * -1f,
                        80,
                        Color.White,
                        1.5f
                    );


                dust.noGravity = true;

            }



            Lighting.AddLight(
                npc.Center,
                0.2f,
                1f,
                0.3f
            );

        }

        private void DrawCrystallineEmeraldMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 6; i++)
            {

                float angle =
                    time * 1.5f
                    +
                    i * MathHelper.TwoPi / 6f;


                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    * 30f;


                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.GreenTorch,
                        Vector2.Zero,
                        100,
                        Color.White,
                        1.45f
                    );


                dust.noGravity = true;

            }



            if (Main.rand.NextBool(3))
            {

                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center,
                        DustID.GreenTorch,
                        Vector2.UnitY * -1f,
                        80,
                        Color.White,
                        1.6f
                    );


                dust.noGravity = true;

            }



            Lighting.AddLight(
                npc.Center,
                0.3f,
                1f,
                0.4f
            );

        }

        private void DrawAmberMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 4; i++)
            {

                float angle =
                    time * 1.5f
                    +
                    i * MathHelper.TwoPi / 4f;



                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    *
                    22f;



                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.YellowTorch,
                        Vector2.Zero,
                        100,
                        Color.White,
                        1.25f
                    );


                dust.noGravity = true;

            }



            if (Main.rand.NextBool(5))
            {

                Vector2 offset =
                    Main.rand.NextVector2Circular(
                        18f,
                        18f
                    );


                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.YellowTorch,
                        Vector2.Zero,
                        80,
                        Color.White,
                        1.4f
                    );


                dust.noGravity = true;

            }



            Lighting.AddLight(
                npc.Center,
                1f,
                0.7f,
                0.2f
            );

        }

        private void DrawCrystallineAmberMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 6; i++)
            {

                float angle =
                    time * 1.8f
                    +
                    i * MathHelper.TwoPi / 6f;


                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    * 28f;


                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.GoldFlame,
                        Vector2.Zero,
                        100,
                        Color.White,
                        1.45f
                    );


                dust.noGravity = true;

            }


            if (Main.rand.NextBool(3))
            {

                Vector2 velocity =
                    Main.rand.NextVector2CircularEdge(
                        1f,
                        1f
                    )
                    * 2f;


                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center,
                        DustID.GemTopaz,
                        velocity,
                        100,
                        Color.White,
                        1.4f
                    );


                dust.noGravity = true;

            }


            Lighting.AddLight(
                npc.Center,
                1f,
                0.75f,
                0.25f
            );

        }

        private void DrawRubyMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 3; i++)
            {

                float angle =
                    time * 2f
                    +
                    i * MathHelper.TwoPi / 3f;


                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    *
                    24f;



                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.RedTorch,
                        Vector2.Zero,
                        100,
                        Color.White,
                        1.3f
                    );


                dust.noGravity = true;

            }

            if (Main.rand.NextBool(3))
            {

                Vector2 velocity =
                    Main.rand.NextVector2CircularEdge(
                        1f,
                        1f
                    )
                    *
                    2f;


                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center,
                        DustID.RedTorch,
                        velocity,
                        100,
                        Color.White,
                        1.5f
                    );


                dust.noGravity = true;

            }



            Lighting.AddLight(
                npc.Center,
                1f,
                0.2f,
                0.2f
            );

        }

        private void DrawCrystallineRubyMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 5; i++)
            {

                float angle =
                    time * 2.2f
                    +
                    i * MathHelper.TwoPi / 5f;


                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    * 28f;


                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.RedTorch,
                        Vector2.Zero,
                        100,
                        Color.White,
                        1.45f
                    );


                dust.noGravity = true;

            }



            if (Main.rand.NextBool(3))
            {

                Vector2 velocity =
                    Main.rand.NextVector2CircularEdge(
                        1f,
                        1f
                    )
                    * 2f;


                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center,
                        DustID.RedTorch,
                        velocity,
                        100,
                        Color.White,
                        1.5f
                    );


                dust.noGravity = true;

            }



            Lighting.AddLight(
                npc.Center,
                1f,
                0.25f,
                0.25f
            );

        }

        private void DrawDiamondMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 4; i++)
            {

                float angle =
                    time * 1.5f
                    +
                    i * MathHelper.PiOver2;



                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    *
                    30f;



                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.WhiteTorch,
                        Vector2.Zero,
                        100,
                        Color.White,
                        1.5f
                    );


                dust.noGravity = true;

            }

            if (Main.rand.NextBool(3))
            {

                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center +
                        Main.rand.NextVector2Circular(
                            10f,
                            10f
                        ),
                        DustID.GemDiamond,
                        Vector2.Zero,
                        80,
                        Color.White,
                        1.4f
                    );


                dust.noGravity = true;

            }



            Lighting.AddLight(
                npc.Center,
                0.8f,
                0.8f,
                1f
            );

        }

        private void DrawCrystallineDiamondMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 6; i++)
            {

                float angle =
                    time * 1.8f
                    +
                    i * MathHelper.TwoPi / 6f;


                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    * 32f;


                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.WhiteTorch,
                        Vector2.Zero,
                        90,
                        Color.White,
                        1.6f
                    );


                dust.noGravity = true;

            }


            if (Main.rand.NextBool(3))
            {

                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center +
                        Main.rand.NextVector2Circular(
                            12f,
                            12f
                        ),
                        DustID.GemDiamond,
                        Vector2.Zero,
                        80,
                        Color.White,
                        1.5f
                    );


                dust.noGravity = true;

            }



            Lighting.AddLight(
                npc.Center,
                1f,
                1f,
                1f
            );

        }

        private void DrawOpalMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 5; i++)
            {

                float angle =
                    time * 1.5f
                    +
                    i * MathHelper.TwoPi / 5f;



                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    * 24f;

                float hue =
                    (float)(
                        (System.Math.Sin(
                            time * 0.6f
                            + i * 0.9f
                        ) + 1f)
                        * 0.5f
                    );


                Color opalColor;


                if (hue < 0.5f)
                {

                    opalColor =
                        Color.Lerp(
                            new Color(255, 120, 190),
                            new Color(190, 80, 190),
                            hue * 2f
                        );

                }
                else
                {

                    opalColor =
                        Color.Lerp(
                            new Color(190, 80, 190),
                            new Color(100, 210, 255),
                            (hue - 0.5f) * 2f
                        );

                }



                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.WhiteTorch,
                        Vector2.Zero,
                        100,
                        opalColor,
                        1.3f
                    );


                dust.noGravity = true;

            }

            if (Main.rand.NextBool(2))
            {

                Dust highlight =
                    Dust.NewDustPerfect(
                        npc.Center +
                        Main.rand.NextVector2Circular(
                            5f,
                            5f
                        ),
                        DustID.WhiteTorch,
                        Vector2.Zero,
                        80,
                        Color.White,
                        1.15f
                    );


                highlight.noGravity = true;

            }



            Lighting.AddLight(
                npc.Center,
                0.8f,
                0.55f,
                0.9f
            );

        }

        private void DrawCrystallineOpalMark(
    NPC npc,
    float time)
        {

            for (int i = 0; i < 7; i++)
            {

                float angle =
                    time * 1.8f
                    +
                    i * MathHelper.TwoPi / 7f;



                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    * 30f;



                float hue =
                    (float)(
                        (System.Math.Sin(
                            time * 0.8f
                            + i * 0.8f
                        ) + 1f)
                        * 0.5f
                    );


                Color opalColor;


                if (hue < 0.5f)
                {

                    opalColor =
                        Color.Lerp(
                            new Color(255, 150, 215),
                            new Color(210, 90, 190),
                            hue * 2f
                        );

                }
                else
                {

                    opalColor =
                        Color.Lerp(
                            new Color(210, 90, 190),
                            new Color(90, 220, 255),
                            (hue - 0.5f) * 2f
                        );

                }



                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.WhiteTorch,
                        Vector2.Zero,
                        80,
                        opalColor,
                        1.5f
                    );


                dust.noGravity = true;

            }

            for (int i = 0; i < 3; i++)
            {

                Vector2 offset =
                    Main.rand.NextVector2Circular(
                        10f,
                        10f
                    );


                Dust highlight =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.WhiteTorch,
                        Vector2.Zero,
                        70,
                        Color.White,
                        1.25f
                    );


                highlight.noGravity = true;

            }

            if (Main.rand.NextBool(3))
            {

                float flash =
                    (float)(
                        (System.Math.Sin(time * 2f) + 1f)
                        * 0.5f
                    );


                Color flashColor =
                    Color.Lerp(
                        new Color(255, 130, 210),
                        new Color(100, 220, 255),
                        flash
                    );


                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center +
                        Main.rand.NextVector2Circular(
                            14f,
                            14f
                        ),
                        DustID.WhiteTorch,
                        Vector2.Zero,
                        60,
                        flashColor,
                        1.4f
                    );


                dust.noGravity = true;

            }



            Lighting.AddLight(
                npc.Center,
                0.9f,
                0.65f,
                1f
            );

        }

        private void DrawAquamarineMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 5; i++)
            {

                float angle =
                    time * 2f
                    +
                    i * MathHelper.TwoPi / 5f;



                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    *
                    28f;



                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.BlueTorch,
                        Vector2.Zero,
                        100,
                        Color.White,
                        1.35f
                    );


                dust.noGravity = true;

            }



            for (int i = 0; i < 3; i++)
            {

                Vector2 velocity =
                    Main.rand.NextVector2CircularEdge(
                        1f,
                        1f
                    )
                    *
                    1.5f;



                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center,
                        DustID.BlueTorch,
                        velocity,
                        100,
                        Color.White,
                        1.2f
                    );


                dust.noGravity = true;

            }



            Lighting.AddLight(
                npc.Center,
                0.2f,
                0.8f,
                1f
            );

        }

        private void DrawCrystallineAquamarineMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 6; i++)
            {

                float angle =
                    time * 2.2f
                    +
                    i * MathHelper.TwoPi / 6f;


                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    *
                    32f;


                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.BlueTorch,
                        Vector2.Zero,
                        90,
                        Color.White,
                        1.5f
                    );


                dust.noGravity = true;

            }


            for (int i = 0; i < 3; i++)
            {

                Vector2 velocity =
                    Main.rand.NextVector2CircularEdge(
                        1f,
                        1f
                    )
                    * 1.8f;


                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center,
                        DustID.BlueTorch,
                        velocity,
                        90,
                        Color.White,
                        1.25f
                    );


                dust.noGravity = true;

            }


            Lighting.AddLight(
                npc.Center,
                0.3f,
                0.9f,
                1f
            );

        }

        private void DrawPrismaticMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 7; i++)
            {

                float angle =
                    time * 1.8f
                    +
                    i * MathHelper.TwoPi / 7f;



                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    * 30f;



                float hue =
                    (
                        time * 0.12f
                        +
                        i / 7f
                    )
                    % 1f;



                Color color =
                    Main.hslToRgb(
                        hue,
                        1f,
                        0.65f
                    );



                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.RainbowTorch,
                        Vector2.Zero,
                        90,
                        color,
                        1.35f
                    );


                dust.noGravity = true;

            }



            Lighting.AddLight(
                npc.Center,
                0.8f,
                0.8f,
                1f
            );

        }

        private void DrawPrismaticResonance(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 8; i++)
            {

                float angle =
                    -time * 1.5f
                    +
                    i * MathHelper.TwoPi / 8f;



                Vector2 offset =
                    new Vector2(
                        (float)System.Math.Cos(angle),
                        (float)System.Math.Sin(angle)
                    )
                    * 34f;



                float hue =
                    (
                        time * 0.2f
                        +
                        i / 8f
                    )
                    % 1f;



                Color color =
                    Main.hslToRgb(
                        hue,
                        1f,
                        0.7f
                    );



                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center + offset,
                        DustID.RainbowTorch,
                        Vector2.Zero,
                        70,
                        color,
                        1.5f
                    );


                dust.noGravity = true;

            }



            for (int i = 0; i < 2; i++)
            {

                Dust dust =
                    Dust.NewDustPerfect(
                        npc.Center +
                        Main.rand.NextVector2Circular(
                            12f,
                            12f
                        ),
                        DustID.WhiteTorch,
                        Vector2.Zero,
                        60,
                        Color.White,
                        1.3f
                    );


                dust.noGravity = true;

            }



            Lighting.AddLight(
                npc.Center,
                1f,
                0.9f,
                1f
            );

        }

    }
}