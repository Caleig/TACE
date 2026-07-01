using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace ThoriumAccessoryExpansion.Accessories.EternalBulwark
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
            // 提供减伤 25%
            player.endurance += 0.15f;

            // 提供生命恢复 (5 + 额外治疗量) / 秒
            
            player.lifeRegen += 5 + thoriumPlayer.healBonus;

            // 发光效果（可选）
            Lighting.AddLight(player.Center, 0.3f, 0.8f, 0.6f);
        }
    }
}