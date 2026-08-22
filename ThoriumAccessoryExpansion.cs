using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath;

namespace ThoriumAccessoryExpansion
{
	
	public class ThoriumAccessoryExpansion : Mod
	{
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            byte msgType = reader.ReadByte();
            switch ((ScrollPlayer.MessageType)msgType)
            {
                case ScrollPlayer.MessageType.SyncScrolls:
                    int playerId = reader.ReadByte();
                    if (Main.netMode == NetmodeID.Server)
                    {
                        
                        ModPacket packet = GetPacket();
                        packet.Write((byte)msgType);
                        packet.Write((byte)playerId);
                        byte[] remaining = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
                        packet.Write(remaining);
                        packet.Send(-1, playerId);
                        break;
                    }
                    
                    Player player = Main.player[playerId];
                    var sp = player.GetModPlayer<ScrollPlayer>();
                    sp.ReceiveScrollData(reader);
                    break;
            }
        }
    }
}
