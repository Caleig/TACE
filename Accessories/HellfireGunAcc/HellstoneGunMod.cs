using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

/* 枪械改件[狱岩]
 * 15狱岩锭＋1非法枪械部件
 * 暴击率提高5%，射速提高5%
 * 武器攻击获得热量（每次1点，上限100）
 * 热量满时射速+10%，攻击消耗2点热量给予"狱炎"并额外造成5点固定伤害（无法暴击）
 * 热量耗尽后才能重新积累 */
public class HellstoneGunMod : ThoriumItem
{
    public const int HeatGain = 1;
    public const int HeatConsume = 2;
    public const int HeatCap = 100;
    public const float FlatDamage = 5f;
    public const int HitDebuff = BuffID.OnFire3;
    public const float SpeedBuff = 0.05f;
    public const float SpeedBuffBoosted = 0.10f;
    public const int CritBonus = 5;

    public override void SetDefaults()
    {
        Item.width = 34;
        Item.height = 24;
        Item.accessory = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.HellstoneBar, 15)
            .AddIngredient(ItemID.IllegalGunParts, 1)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var gf = player.GetModPlayer<GunFirePlayer>();
        gf.gunfireAcc = true;
        gf.heatGain = HeatGain;
        gf.heatConsume = HeatConsume;
        gf.heatCap = HeatCap;
        gf.flatDamage = FlatDamage;
        gf.hitDebuff = HitDebuff;
        gf.speedBuff = SpeedBuff;
        gf.speedBuffBoosted = SpeedBuffBoosted;
        gf.critBonus = CritBonus;

        player.GetAttackSpeed(DamageClass.Ranged) += SpeedBuff + (gf.boosted ? SpeedBuffBoosted : 0f);
        player.GetCritChance(DamageClass.Ranged) += CritBonus;
    }
}
