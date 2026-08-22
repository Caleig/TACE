using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.Healer.KarmaCovenant
{
    public class UnholyKarma : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
    }
}