using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.EternalBulwark
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