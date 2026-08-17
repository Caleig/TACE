using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items;
using ThoriumMod.Items.MasterMode;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

/* 枪械改件[血肉]
 * 枪械改件[狱炎]＋15不明肉块＋10灵液
 * 暴击率提高8%，射速提高12%
 * 武器攻击获得热量（每次1点，上限100）
 * 热量满时攻击消耗更少热量（1点）给予"灵液"并额外造成6点固定伤害（无法暴击）
 * 热量耗尽后才能重新积累 */
public class FleshGunMod : ThoriumItem
{
    public const int HeatGain = 1;
    public const int HeatConsume = 1;
    public const int HeatCap = 100;
    public const float FlatDamage = 6f;
    public const int HitDebuff = BuffID.Ichor;
    public const float SpeedBuff = 0.12f;
    public const int CritBonus = 8;

    public override void SetDefaults()
    {
        Item.width = 42;
        Item.height = 28;
        Item.accessory = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<HellstoneGunMod>(), 1)
            .AddIngredient(ModContent.ItemType<RottenMeat>(), 15)
            .AddIngredient(ItemID.Ichor, 10)
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
