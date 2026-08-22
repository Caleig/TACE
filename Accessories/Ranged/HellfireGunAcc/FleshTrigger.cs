using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace ThoriumAccessoryExpansion.Accessories.Ranged.HellfireGunAcc;

public class FleshTrigger : ThoriumItem
{
    public override void SetDefaults()
    {
        Item.width = 54;
        Item.height = 34;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var gf = player.GetModPlayer<GunFirePlayer>();
        gf.gunfireAcc = true;
        gf.heatGainPerShot = 4;
        gf.cooldownRate = 1f;
        gf.overloadBonus = 0f;
        gf.extraDamage = 10;
        gf.extraDamageCanCrit = true;   

        player.GetAttackSpeed(DamageClass.Ranged) += 0.2f;
    }
}