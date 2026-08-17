using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

/* 枪械改件（狱岩）
 * 枪械速度提高5%
 * 枪械暴击率增加5%
 * 缓慢使枪械过热
 * 过热后造成少量额外伤害并匀速冷却
 * 彻底冷却前不会再次积蓄热量 */
public class HellstoneGunMod : ThoriumItem
{
    public const int HeatGain = 1;           // 缓慢蓄热
    public const float OverloadBonus = 0.2f; // 少量额外伤害
    // 冷却：0 = Thorium 原速 -> "匀速冷却"

    public override void SetDefaults()
    {
        Item.width = 34;
        Item.height = 24;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var gf = player.GetModPlayer<GunFirePlayer>();
        gf.gunfireAcc = true;
        gf.heatGainPerShot = HeatGain;
        gf.overloadBonus = OverloadBonus;

        player.GetAttackSpeed(DamageClass.Ranged) += 0.05f;
        player.GetCritChance(DamageClass.Ranged) += 5f;
    }
}
