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


            if (gemType == GemType.None)
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

                case GemType.Topaz:

                    DrawTopazMark(
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

                case GemType.Emerald:

                    DrawEmeraldMark(
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

                case GemType.Ruby:

                    DrawRubyMark(
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

                case GemType.Opal:

                    DrawOpalMark(
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
        private void DrawOpalMark(
            NPC npc,
            float time)
        {

            for (int i = 0; i < 3; i++)
            {

                float angle =
                    time * 1.5f
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
                        DustID.WhiteTorch,
                        Vector2.Zero,
                        100,
                        Color.White,
                        1.3f
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
    }
}