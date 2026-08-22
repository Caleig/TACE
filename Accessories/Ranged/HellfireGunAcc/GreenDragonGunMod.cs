using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace ThoriumAccessoryExpansion.Accessories.Ranged.HellfireGunAcc;

public class GreenDragonGunMod : ThoriumItem
{
    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 34;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var gf = player.GetModPlayer<GunFirePlayer>();
        gf.gunfireAcc = true;
        gf.heatGainPerShot = 1;
        gf.cooldownRate = 2f;           
        gf.overloadBonus = 0f;
        gf.extraDamage = 10;
        gf.extraDamageCanCrit = false;

        player.GetAttackSpeed(DamageClass.Ranged) += 0.08f;
        player.GetCritChance(DamageClass.Ranged) += 12f;
    }
}