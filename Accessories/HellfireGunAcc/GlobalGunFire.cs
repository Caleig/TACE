using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.Donate;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

/// <summary>
/// 子弹枪械通用钩子：热状态机（蓄热/消耗）、泰坦攻速/慢枪加成、增益态伤害
/// </summary>
public class GlobalGunFire : GlobalItem
{
    public const int SlowGunUseTime = 30;       // 泰坦：慢枪判定阈值
    public const float TitanSpeedMult = 1.3f;   // 泰坦：攻速 -30%
    public const float TitanFastGunBonus = 0.75f; // 泰坦：useTime<30 的枪 +75% 伤害
    public const float TitanCritBonus = 0.5f;   // 泰坦：暴击 x1.5 -> 合计 x3

    public override bool AppliesToEntity(Item entity, bool lateInstantiation) =>
        entity.useAmmo == AmmoID.Bullet && entity.ModItem is not HellfireMinigun;

    public override bool? UseItem(Item item, Player player)
    {
        var gf = player.GetModPlayer<GunFirePlayer>();
        if (!gf.gunfireAcc)
            return base.UseItem(item, player);

        var tp = player.GetThoriumPlayer();
        if (!gf.boosted)
        {
            // 蓄热阶段：每次攻击 +heatGain
            tp.hellfireEnergy += gf.heatGain;
            if (tp.hellfireEnergy >= gf.heatCap)
            {
                tp.hellfireEnergy = gf.heatCap;
                gf.boosted = true; // 热量满 -> 增益态
            }
        }
        else
        {
            // 增益阶段：每次攻击消耗热量，耗尽后才重新积累
            tp.hellfireEnergy -= gf.heatConsume;
            if (tp.hellfireEnergy <= 0)
            {
                tp.hellfireEnergy = 0;
                gf.boosted = false;
            }
        }
        return base.UseItem(item, player);
    }

    public override float UseTimeMultiplier(Item item, Player player) =>
        player.GetModPlayer<GunFirePlayer>().titanAcc ? TitanSpeedMult : 1f;

    public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
    {
        var gf = player.GetModPlayer<GunFirePlayer>();
        if (gf.titanAcc && item.useTime < SlowGunUseTime)
            damage += TitanFastGunBonus; // 初始使用时间低于慢的枪械类 +75%
    }
}

/// <summary>
/// 泰坦：子弹暴击倍率 x3（原版 x2 -> x3）
/// </summary>
public class GlobalBulletCrit : GlobalProjectile
{
    public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
    {
        var owner = Main.player[projectile.owner];
        if (owner.GetModPlayer<GunFirePlayer>().titanAcc && owner.HeldItem.useAmmo == AmmoID.Bullet)
            modifiers.CritDamage += GlobalGunFire.TitanCritBonus;
    }
}

/// <summary>
/// 发热改件增益态：固定伤害（不暴击/可暴击）+ 命中减益
/// </summary>
public class GlobalBulletHeat : GlobalProjectile
{
    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        // 扳机：可暴击固定伤害 -> 弹幕伤害前置（先于暴击计算）
        var gf = Main.player[projectile.owner].GetModPlayer<GunFirePlayer>();
        if (gf.gunfireAcc && gf.flatCrits && gf.boosted && Main.player[projectile.owner].HeldItem.useAmmo == AmmoID.Bullet)
            projectile.damage += (int)gf.flatDamage;
    }

    public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
    {
        var owner = Main.player[projectile.owner];
        var gf = owner.GetModPlayer<GunFirePlayer>();
        // 不暴击固定伤害：FlatBonusDamage 在暴击乘法之后，天然不吃暴击
        if (gf.gunfireAcc && !gf.flatCrits && gf.boosted && gf.flatDamage > 0 && owner.HeldItem.useAmmo == AmmoID.Bullet)
            modifiers.FlatBonusDamage += gf.flatDamage;
    }

    public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
        var owner = Main.player[projectile.owner];
        var gf = owner.GetModPlayer<GunFirePlayer>();
        if (gf.gunfireAcc && gf.boosted && gf.hitDebuff > 0 && owner.HeldItem.useAmmo == AmmoID.Bullet)
            target.AddBuff(gf.hitDebuff, 300); // 5 秒
    }
}
