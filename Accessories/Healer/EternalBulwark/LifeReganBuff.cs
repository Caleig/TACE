using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace ThoriumAccessoryExpansion.Accessories.Healer.EternalBulwark
{
    public class LifeReganBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            
            player.endurance += 0.15f;

            
            
            player.lifeRegen += 5 + thoriumPlayer.healBonus;

            
            Lighting.AddLight(player.Center, 0.3f, 0.8f, 0.6f);
        }
    }
}