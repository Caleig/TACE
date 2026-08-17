using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

/* 恐惧箭袋
 * 箭的伤害提升15%，箭的速度大大提高
 * 远程暴击率增加8%
 * 25%的概率不消耗箭
 * 复制15%的箭矢伤害
 * 敌人不太可能瞄准你 */
public class DreadQuiver : ThoriumItem
{
    public const float ArrowSpeedMult = 1.3f; // 箭速 +30%
    public const float CopyDamage = 0.15f;    // 命中追加 15% 伤害
    public const int AggroReduction = 400;    // 仇恨降低

    public override void SetDefaults()
    {
        Item.width = 42;
        Item.height = 46;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<GunFirePlayer>().dreadQuiver = true;

        player.GetDamage(DamageClass.Ranged) += 0.15f;
        player.GetCritChance(DamageClass.Ranged) += 8f;
        player.aggro -= AggroReduction;
    }
}

/// <summary>
/// 恐惧箭袋：箭矢伤害提升 + 箭速大大提高（发射瞬间缩放初速）
/// </summary>
public class QuiverArrowGlobal : GlobalProjectile
{
    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.arrow;

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (Main.player[projectile.owner].GetModPlayer<GunFirePlayer>().dreadQuiver)
            projectile.velocity *= DreadQuiver.ArrowSpeedMult;
    }

    public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (Main.player[projectile.owner].GetModPlayer<GunFirePlayer>().dreadQuiver)
            modifiers.FlatBonusDamage += (int)(projectile.damage * DreadQuiver.CopyDamage);
    }
}

/// <summary>
/// 恐惧箭袋：25% 概率不消耗箭
/// </summary>
public class QuiverAmmoGlobal : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.ammo == AmmoID.Arrow;

    public override bool CanBeConsumedAsAmmo(Item weapon, Item ammo, Player player) =>
        player.GetModPlayer<GunFirePlayer>().dreadQuiver && Main.rand.NextFloat() < 0.25f
            ? false
            : base.CanBeConsumedAsAmmo(weapon, ammo, player);
}
