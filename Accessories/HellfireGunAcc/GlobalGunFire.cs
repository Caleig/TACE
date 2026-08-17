using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.Donate;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

/// <summary>
/// 子弹枪械通用钩子：蓄热、泰坦攻速/慢枪加成、过热冷却期间伤害加成
/// </summary>
public class GlobalGunFire : GlobalItem
{
    public const int SlowGunUseTime = 30;       // 泰坦：慢枪 useTime 阈值
    public const float TitanSpeedMult = 1.3f;   // 泰坦：攻速 -30%
    public const float TitanSlowGunBonus = 1f;  // 泰坦：慢枪 +100% 伤害
    public const float TitanCritBonus = 0.5f;   // 泰坦：暴击 x1.5 -> 合计 x3（见 GlobalBulletCrit）
    public const float OverloadGate = 0f;       // (占位常量防误改)

    public override bool AppliesToEntity(Item entity, bool lateInstantiation) =>
        entity.useAmmo == AmmoID.Bullet && entity.ModItem is not HellfireMinigun;

    public override bool? UseItem(Item item, Player player)
    {
        var gf = player.GetModPlayer<GunFirePlayer>();
        if (!gf.gunfireAcc || gf.heatGainPerShot <= 0)
            return base.UseItem(item, player);
        var tp = player.GetThoriumPlayer();
        if (!tp.hellfireEnergyOverload) // 彻底冷却前不再蓄热
        {
            SoundEngine.PlaySound(in item.UseSound, new Vector2?(player.Center));
            tp.hellfireEnergy += gf.heatGainPerShot; // 上限 100，过热由 Thorium 触发
        }
        return base.UseItem(item, player);
    }

    public override float UseTimeMultiplier(Item item, Player player) =>
        player.GetModPlayer<GunFirePlayer>().titanAcc ? TitanSpeedMult : 1f;

    public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
    {
        var gf = player.GetModPlayer<GunFirePlayer>();
        if (gf.titanAcc && item.useTime >= SlowGunUseTime)
            damage += TitanSlowGunBonus; // 极大幅度提升缓慢的枪械
        if (gf.gunfireAcc && gf.overloadBonus > 0f && player.GetThoriumPlayer().hellfireEnergyOverload)
            damage += gf.overloadBonus; // 过热冷却期间的开枪附加伤害
    }
}

/// <summary>
/// 泰坦：子弹暴击倍率 x3（原版 x2 -> x3）
/// </summary>
public class GlobalBulletCrit : GlobalProjectile
{
    public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
    {
        // 只对"用子弹枪械打出的弹幕"生效：看发射者当前主手是否消耗子弹
        var owner = Main.player[projectile.owner];
        if (owner.GetModPlayer<GunFirePlayer>().titanAcc && owner.HeldItem.useAmmo == AmmoID.Bullet)
            modifiers.CritDamage += GlobalGunFire.TitanCritBonus;
    }
}
