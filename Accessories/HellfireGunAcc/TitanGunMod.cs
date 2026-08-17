using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

/* 枪械改件（泰坦）
 * 枪械速度减少30%
 * 使使用者更脆弱
 * 极大幅度提升缓慢的枪械
 * 枪械暴击倍率提升到300% */
public class TitanGunMod : ThoriumItem
{
    public const float FragileEndurance = -0.2f; // 受伤 +20%

    public override void SetDefaults()
    {
        Item.width = 36;
        Item.height = 26;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<GunFirePlayer>().titanAcc = true;
        player.endurance += FragileEndurance;
        // 攻速 -30%（UseTimeMultiplier）、慢枪 +100%（ModifyWeaponDamage）、
        // 暴击 x3（GlobalBulletCrit）都在 GlobalGunFire.cs 里
    }
}
