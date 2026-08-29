using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.NPCs;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod.Utilities;


namespace ThoriumAccessoryExpansion.Items.LegendaryResonance;


public class LegendaryResonancePickupItem : ModItem
{

    public const byte PickupPacket = 200;



    public GemType VisualType =
        GemType.Prismatic;



    public override string Texture =>
        "ThoriumAccessoryExpansion/Images/Items/LegendaryResonance/Prismatic";



    public override void SetStaticDefaults()
    {

        ItemID.Sets.ItemNoGravity[Type] =
            true;

    }



    public override void SetDefaults()
    {

        Item.width = 18;
        Item.height = 18;

        Item.maxStack = 1;

        Item.value = 0;

        Item.rare =
            ItemRarityID.Lime;

    }



    public override void OnSpawn(
        IEntitySource source)
    {

        VisualType =
            Main.rand.Next(10) switch
            {

                0 => GemType.Amethyst,

                1 => GemType.Topaz,

                2 => GemType.Sapphire,

                3 => GemType.Emerald,

                4 => GemType.Amber,

                5 => GemType.Ruby,

                6 => GemType.Diamond,

                7 => GemType.Opal,

                8 => GemType.Aquamarine,

                _ => GemType.Prismatic

            };

    }



    public override bool ItemSpace(
        Player player)
    {

        return true;

    }



    public override bool OnPickup(
        Player player)
    {

        if (
            Main.netMode ==
            NetmodeID.SinglePlayer
        )
        {

            GemType type =
                ResolveResonanceType(
                    VisualType
                );


            ApplyPickup(
                player,
                type
            );


            Item.TurnToAir();

        }
        else if (
            Main.netMode ==
            NetmodeID.MultiplayerClient
        )
        {

            ModPacket packet =
                Mod.GetPacket();



            packet.Write(
                PickupPacket
            );


            packet.Write(
                (byte)player.whoAmI
            );


            packet.Write(
                (short)Item.whoAmI
            );


            packet.Write(
                (byte)VisualType
            );



            packet.Send();



            Item.TurnToAir();

        }



        return false;

    }



    public override void GrabRange(
        Player player,
        ref int grabRange)
    {

        grabRange =
            Math.Max(
                grabRange,
                300
            );

    }



    public override void Update(
        ref float gravity,
        ref float maxFallSpeed)
    {

        gravity = 0f;

        maxFallSpeed = 0f;

        Item.velocity *= 0.92f;

    }



    public override void PostUpdate()
    {

        if (Main.dedServ)
            return;



        Color color =
            GetVisualColor(
                VisualType
            );



        Lighting.AddLight(
            Item.Center,
            color.ToVector3() * 0.5f
        );



        if (Main.rand.NextBool(3))
        {

            Dust dust =
                Dust.NewDustPerfect(
                    Item.Center +
                    Main.rand.NextVector2Circular(
                        5f,
                        5f
                    ),
                    DustID.WhiteTorch,
                    Vector2.Zero,
                    100,
                    color,
                    0.9f
                );


            dust.noGravity = true;

        }

    }



    public override bool PreDrawInWorld(
        SpriteBatch spriteBatch,
        Color lightColor,
        Color alphaColor,
        ref float rotation,
        ref float scale,
        int whoAmI)
    {

        Texture2D texture =
            ModContent.Request<Texture2D>(
                GetVisualTexturePath(
                    VisualType
                )
            ).Value;



        rotation +=
            Main.GlobalTimeWrappedHourly
            * 0.5f;



        spriteBatch.Draw(
            texture,
            Item.Center -
            Main.screenPosition,
            null,
            lightColor,
            rotation,
            texture.Size() / 2f,
            scale,
            SpriteEffects.None,
            0f
        );



        return false;

    }



    public override void NetSend(
        BinaryWriter writer)
    {

        writer.Write(
            (byte)VisualType
        );

    }



    public override void NetReceive(
        BinaryReader reader)
    {

        VisualType =
            (GemType)reader.ReadByte();

    }



    public static GemType ResolveResonanceType(
        GemType visualType)
    {

        if (
            visualType !=
            GemType.Prismatic
        )
        {

            return visualType;

        }



        return Main.rand.Next(9) switch
        {

            0 => GemType.Amethyst,

            1 => GemType.Topaz,

            2 => GemType.Sapphire,

            3 => GemType.Emerald,

            4 => GemType.Amber,

            5 => GemType.Ruby,

            6 => GemType.Diamond,

            7 => GemType.Opal,

            _ => GemType.Aquamarine

        };

    }



    public static void ApplyPickup(
        Player player,
        GemType type)
    {

        if (!player.active)
            return;



        GemContractPlayer contract =
            player.GetModPlayer<
                GemContractPlayer
            >();



        contract.AddLegendaryResonance(
            type
        );

        int lifeRestore =
            Math.Max(
                1,
                (int)(
                    player.statLifeMax2
                    * 0.05f
                )
            );



        player.Heal(
            lifeRestore
        );


        if (
            player.statManaMax2
            > 0
        )
        {

            int manaRestore =
                Math.Max(
                    1,
                    (int)(
                        player.statManaMax2
                        * 0.05f
                    )
                );



            int oldMana =
                player.statMana;



            player.statMana =
                Math.Min(
                    player.statManaMax2,
                    player.statMana
                    + manaRestore
                );



            int actualRestore =
                player.statMana -
                oldMana;



            if (actualRestore > 0)
            {

                player.ManaEffect(
                    actualRestore
                );

            }

        }


        var thoriumPlayer =
            player.GetThoriumPlayer();



        if (
            thoriumPlayer
                .bardResourceMax2
            > 0
        )
        {

            int inspirationRestore =
                Math.Max(
                    1,
                    (int)(
                        thoriumPlayer
                            .bardResourceMax2
                        * 0.05f
                    )
                );



            thoriumPlayer.bardResource =
                Math.Min(
                    thoriumPlayer
                        .bardResourceMax2,
                    thoriumPlayer.bardResource
                    + inspirationRestore
                );

        }

    }



    public static int SpawnRandom(
        IEntitySource source,
        Vector2 position)
    {

        int index =
            Item.NewItem(
                source,
                new Rectangle(
                    (int)position.X - 9,
                    (int)position.Y - 9,
                    18,
                    18
                ),
                ModContent.ItemType<
                    LegendaryResonancePickupItem
                >()
            );



        if (
            index < 0
            ||
            index >= Main.maxItems
        )
            return index;



        Main.item[index].velocity =
            new Vector2(
                Main.rand.NextFloat(
                    -1.5f,
                    1.5f
                ),
                Main.rand.NextFloat(
                    -2f,
                    -0.5f
                )
            );



        if (
            Main.netMode ==
            NetmodeID.Server
        )
        {

            NetMessage.SendData(
                MessageID.SyncItem,
                number: index
            );

        }



        return index;

    }



    private static string GetVisualTexturePath(
        GemType type)
    {

        return type switch
        {

            GemType.Amethyst =>
                "ThoriumAccessoryExpansion/Images/Items/LegendaryResonance/Amethyst",

            GemType.Topaz =>
                "ThoriumAccessoryExpansion/Images/Items/LegendaryResonance/Topaz",

            GemType.Sapphire =>
                "ThoriumAccessoryExpansion/Images/Items/LegendaryResonance/Sapphire",

            GemType.Emerald =>
                "ThoriumAccessoryExpansion/Images/Items/LegendaryResonance/Emerald",

            GemType.Amber =>
                "ThoriumAccessoryExpansion/Images/Items/LegendaryResonance/Amber",

            GemType.Ruby =>
                "ThoriumAccessoryExpansion/Images/Items/LegendaryResonance/Ruby",

            GemType.Diamond =>
                "ThoriumAccessoryExpansion/Images/Items/LegendaryResonance/Diamond",

            GemType.Opal =>
                "ThoriumAccessoryExpansion/Images/Items/LegendaryResonance/Opal",

            GemType.Aquamarine =>
                "ThoriumAccessoryExpansion/Images/Items/LegendaryResonance/Aquamarine",

            _ =>
                "ThoriumAccessoryExpansion/Images/Items/LegendaryResonance/Prismatic"

        };

    }



    private static Color GetVisualColor(
        GemType type)
    {

        return type switch
        {

            GemType.Amethyst =>
                new Color(
                    180,
                    80,
                    255
                ),

            GemType.Topaz =>
                new Color(
                    255,
                    190,
                    60
                ),

            GemType.Sapphire =>
                new Color(
                    70,
                    150,
                    255
                ),

            GemType.Emerald =>
                new Color(
                    70,
                    230,
                    110
                ),

            GemType.Amber =>
                new Color(
                    255,
                    175,
                    50
                ),

            GemType.Ruby =>
                new Color(
                    255,
                    60,
                    70
                ),

            GemType.Diamond =>
                Color.White,

            GemType.Opal =>
                new Color(
                    255,
                    140,
                    220
                ),

            GemType.Aquamarine =>
                new Color(
                    70,
                    220,
                    255
                ),

            _ =>
                Color.White

        };

    }

}