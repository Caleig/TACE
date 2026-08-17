using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items;
using ThoriumMod.Items.Dread;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

/* 恐惧箭袋
 * 10恐惧之灵＋潜伏者箭袋
 * 箭伤害提高15%，暴击率提高8%
 * 25%不消耗箭，箭的速度大大提高
 * 弓箭命中复制15%的远程伤害
 * 敌人不易以你为目标 */
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

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<DreadSoul>(), 10)
            .AddIngredient(ItemID.StalkersQuiver, 1)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
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
/// 恐惧箭袋：箭速提升 + 命中复制 15% 伤害
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
