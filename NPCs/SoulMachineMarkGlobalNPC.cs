using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ThoriumAccessoryExpansion.NPCs;

public class SoulMachineMarkGlobalNPC : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public bool HasSoulMachineMark;

    public void ApplyMark(NPC npc)
    {
        HasSoulMachineMark = true;

        npc.netUpdate = true;
    }

    public void ConsumeMark(NPC npc)
    {
        HasSoulMachineMark = false;

        npc.netUpdate = true;
    }

    public override void SendExtraAI(
        NPC npc,
        BitWriter bitWriter,
        BinaryWriter binaryWriter)
    {
        bitWriter.WriteBit(HasSoulMachineMark);
    }

    public override void ReceiveExtraAI(
        NPC npc,
        BitReader bitReader,
        BinaryReader binaryReader)
    {
        HasSoulMachineMark = bitReader.ReadBit();
    }
}