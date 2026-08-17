using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

/* 枪械改件（绿龙）
 * 枪械速度提高8%
 * 枪械暴击率增加12%
 * 缓慢使枪械过热
 * 过热后造成更多额外伤害并缓慢冷却
 * 彻底冷却前不会再次积蓄热量 */
public class GreenDragonGunMod : ThoriumItem
{
    public const int HeatGain = 2;           // 缓慢蓄热
    public const float CooldownRate = 2f;    // 缓慢冷却（每秒泄热）
    public const float OverloadBonus = 0.45f; // 更多额外伤害

    public override void SetDefaults()
    {
        Item.width = 40;
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

        player.GetAttackSpeed(DamageClass.Ranged) += 0.08f;
        player.GetCritChance(DamageClass.Ranged) += 12f;
    }
}
