using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.ZRemoved;

namespace ThoriumAccessoryExpansion.Accessories.Healer.MichaelasAid
{
    public class SupremeWrath : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            
            Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.LifeCrystal);
        }
    }
}