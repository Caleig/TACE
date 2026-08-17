using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items;
using ThoriumMod.Items.Dragon;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

/* 枪械改件[绿龙]
 * 枪械改件[狱炎]＋15苍绿龙鳞＋10诅咒炎
 * 暴击率提高12%，射速提高8%
 * 武器攻击获得热量（每次1点，上限100）
 * 热量满时攻击消耗更少热量（1点）给予"诅咒炎"并额外造成10点固定伤害（无法暴击）
 * 热量耗尽后才能重新积累 */
public class GreenDragonGunMod : ThoriumItem
{
    public const int HeatGain = 1;
    public const int HeatConsume = 1;
    public const int HeatCap = 100;
    public const float FlatDamage = 10f;
    public const int HitDebuff = BuffID.CursedInferno;
    public const float SpeedBuff = 0.08f;
    public const int CritBonus = 12;

    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 34;
        Item.accessory = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<HellstoneGunMod>(), 1)
            .AddIngredient(ModContent.ItemType<GreenDragonScale>(), 15)
            .AddIngredient(ItemID.CursedFlame, 10)
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
        gf.critBonus = CritBonus;

        player.GetAttackSpeed(DamageClass.Ranged) += SpeedBuff;
        player.GetCritChance(DamageClass.Ranged) += CritBonus;
    }
}
