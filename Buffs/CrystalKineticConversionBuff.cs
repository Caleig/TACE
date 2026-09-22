using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Buffs;

public class CrystalKineticConversionBuff
    : ModBuff
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
            DamageClass.Generic
        ) += 0.15f;

        player.GetArmorPenetration(
            DamageClass.Generic
        ) += 10f;
    }
}