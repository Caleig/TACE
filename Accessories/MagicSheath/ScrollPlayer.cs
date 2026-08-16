using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.MagicSheath
{
    public class ScrollPlayer : ModPlayer
    {
        public List<int> ActiveScrolls = new List<int>();

        public override void ResetEffects() { }

        public bool IsScrollActive(int typeID) => ActiveScrolls.Contains(typeID);

        public void ToggleScroll(int typeID)
        {
            if (ActiveScrolls.Contains(typeID))
                ActiveScrolls.Remove(typeID);
            else
            {
                if (ActiveScrolls.Count >= 2)
                    ActiveScrolls.RemoveAt(0);
                ActiveScrolls.Add(typeID);
            }
            if (Main.netMode == NetmodeID.Server)
                SendScrollData();
        }

        private void SendScrollData()
        {
            if (Main.netMode != NetmodeID.Server) return;
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)MessageType.SyncScrolls);
            packet.Write((byte)Player.whoAmI);
            packet.Write((byte)ActiveScrolls.Count);
            foreach (int id in ActiveScrolls)
                packet.Write((byte)id);
            packet.Send();
        }

        public void ReceiveScrollData(BinaryReader reader)
        {
            int count = reader.ReadByte();
            ActiveScrolls.Clear();
            for (int i = 0; i < count; i++)
                ActiveScrolls.Add(reader.ReadByte());
        }

        public enum MessageType : byte
        {
            SyncScrolls
        }
    }
}