using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.CursedCovenant
{
    public class LifeDrainCooldown : ModBuff
    {
        public override void SetStaticDefaults()
        {
           /* DisplayName.SetDefault("生命反噬冷却");
            Description.SetDefault("防止圣约在单次攻击中多次触发");*/
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
}