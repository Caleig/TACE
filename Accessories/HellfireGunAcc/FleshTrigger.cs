using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items;
using ThoriumMod.Items.DemonBlood;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

/* 血肉扳机
 * 枪械改件[血肉]/[绿龙]＋10魔血碎块
 * 射速提高20%
 * 武器攻击获得热量（每次2点，上限150）
 * 热量满时攻击消耗更少热量（1点）并额外造成10点固定伤害（可暴击）
 * 热量耗尽后才继续积累
 * 视为枪械改件 */
public class FleshTrigger : ThoriumItem
{
    public const int HeatGain = 2;
    public const int HeatConsume = 1;
    public const int HeatCap = 150;
    public const float FlatDamage = 10f; // 可暴击
    public const float SpeedBuff = 0.20f;

    public override void SetDefaults()
    {
        Item.width = 54;
        Item.height = 34;
        Item.accessory = true;
    }

    public override void AddRecipes()
    {
        // 枪械改件[血肉] 或 [绿龙] + 10魔血碎块
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<FleshGunMod>(), 1)
            .AddIngredient(ModContent.ItemType<DemonBloodShard>(), 10)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<GreenDragonGunMod>(), 1)
            .AddIngredient(ModContent.ItemType<DemonBloodShard>(), 10)
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
        gf.flatCrits = true; // 可暴击

        player.GetAttackSpeed(DamageClass.Ranged) += SpeedBuff;
    }
}
