using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items;
using ThoriumMod.Items.Titan;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

/* 枪械改件[泰坦]
 * 15泰坦锭＋1非法枪械部件
 * 受到伤害提高25%
 * 攻速降低30%
 * 初始使用时间低于慢的枪械类武器伤害提升75%
 * 暴击将造成额外伤害（300%） */
public class TitanGunMod : ThoriumItem
{
    public const float FragileEndurance = -0.25f; // 受伤 +25%

    public override void SetDefaults()
    {
        Item.width = 36;
        Item.height = 26;
        Item.accessory = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<TitanicBar>(), 15)
            .AddIngredient(ItemID.IllegalGunParts, 1)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<GunFirePlayer>().titanAcc = true;
        player.endurance += FragileEndurance;
        // 攻速 -30%（UseTimeMultiplier）、快枪 +75%（ModifyWeaponDamage）、
        // 暴击 x3（GlobalBulletCrit）都在 GlobalGunFire.cs 里
    }
}
