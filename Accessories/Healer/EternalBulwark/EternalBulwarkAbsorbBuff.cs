using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.Healer.EternalBulwark
{
    public class EternalBulwarkAbsorbBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}