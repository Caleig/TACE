using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Items.LegendaryResonance;
using ThoriumAccessoryExpansion.NPCs;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion;

public class ThoriumAccessoryExpansion : Mod
{
    public override void HandlePacket(
        BinaryReader reader,
        int whoAmI)
    {
        byte msgType =
            reader.ReadByte();

        if (
            msgType ==
            LegendaryResonancePickupItem
                .PickupPacket
        )
        {
            HandleLegendaryPickup(
                reader,
                whoAmI
            );

            return;
        }

        if (
            msgType ==
            GemContractPlayer
                .LegendaryResonanceSyncPacket
        )
        {
            int playerId =
                reader.ReadByte();


            if (
                playerId < 0 ||
                playerId >= Main.maxPlayers
            )
            {
                return;
            }


            Player player =
                Main.player[playerId];


            if (!player.active)
                return;


            GemContractPlayer contract =
                player.GetModPlayer<
                    GemContractPlayer
                >();


            contract.ReceiveLegendaryResonance(
                reader
            );


            return;
        }
    }


    private void HandleLegendaryPickup(
        BinaryReader reader,
        int whoAmI)
    {
        int playerId =
            reader.ReadByte();


        short itemId =
            reader.ReadInt16();

        reader.ReadByte();

        if (
            Main.netMode !=
            NetmodeID.Server
        )
        {
            return;
        }

        if (
            playerId != whoAmI
        )
        {
            return;
        }


        if (
            playerId < 0 ||
            playerId >= Main.maxPlayers
        )
        {
            return;
        }


        if (
            itemId < 0 ||
            itemId >= Main.maxItems
        )
        {
            return;
        }


        Player picker =
            Main.player[playerId];


        if (!picker.active)
            return;


        Item item =
            Main.item[itemId];


        if (!item.active)
            return;


        if (
            item.type !=
            ModContent.ItemType<
                LegendaryResonancePickupItem
            >()
        )
        {
            return;
        }


        LegendaryResonancePickupItem pickup =
            item.ModItem as
            LegendaryResonancePickupItem;


        if (pickup == null)
            return;

        if (
            Vector2.Distance(
                picker.Center,
                item.Center
            ) > 100f
        )
        {
            return;
        }


        GemType resonanceType =
            LegendaryResonancePickupItem
                .ResolveResonanceType(
                    pickup.VisualType
                );

        item.TurnToAir();


        NetMessage.SendData(
            MessageID.SyncItem,
            number: itemId
        );

        LegendaryResonancePickupItem
            .ApplyPickup(
                picker,
                resonanceType
            );


        NetMessage.SendData(
            MessageID.SyncPlayer,
            number: picker.whoAmI
        );

        for (
            int i = 0;
            i < Main.maxPlayers;
            i++
        )
        {
            if (i == playerId)
                continue;


            Player other =
                Main.player[i];


            if (!other.active)
                continue;


            if (other.dead)
                continue;


            if (
                Vector2.Distance(
                    picker.Center,
                    other.Center
                ) > 700f
            )
            {
                continue;
            }

            if (
                picker.team != 0 &&
                other.team != picker.team
            )
            {
                continue;
            }


            LegendaryResonancePickupItem
                .ApplyPickup(
                    other,
                    resonanceType
                );


            NetMessage.SendData(
                MessageID.SyncPlayer,
                number: other.whoAmI
            );
        }
    }
}