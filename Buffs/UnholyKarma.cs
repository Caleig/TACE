using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Buffs;

public class UnholyKarma : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        Main.pvpBuff[Type] = true;
        Main.buffNoSave[Type] = true;
    }
}