using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

/* 血肉扳机
 * 枪械速度提高20%
 * 更快使枪械过热
 * 过热后造成更多额外伤害并极慢的冷却
 * 彻底冷却前不会再次积蓄热量 */
public class FleshTrigger : ThoriumItem
{
    public const int HeatGain = 4;           // 更快蓄热
    public const float CooldownRate = 1f;    // 极慢冷却
    public const float OverloadBonus = 0.6f; // 更多额外伤害

    public override void SetDefaults()
    {
        Item.width = 54;
        Item.height = 34;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var gf = player.GetModPlayer<GunFirePlayer>();
        gf.gunfireAcc = true;
        gf.heatGainPerShot = HeatGain;
        gf.cooldownRate = CooldownRate;
        gf.overloadBonus = OverloadBonus;

        player.GetAttackSpeed(DamageClass.Ranged) += 0.2f;
    }
}
