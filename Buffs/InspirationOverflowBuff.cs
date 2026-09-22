using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace ThoriumAccessoryExpansion.Buffs;

public class InspirationOverflowBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;
    }

    public override void Update(
        Player player,
        ref int buffIndex)
    {
        player.GetDamage(
            BardDamage.Instance
        ) += 0.25f;
    }
}