using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.Healer.CursedCovenant
{
    public class LifeDrainCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
           
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
}