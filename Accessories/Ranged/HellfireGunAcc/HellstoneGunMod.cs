using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace ThoriumAccessoryExpansion.Accessories.Ranged.HellfireGunAcc;

public class HellstoneGunMod : ThoriumItem
{
    public override void SetDefaults()
    {
        Item.width = 34;
        Item.height = 24;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var gf = player.GetModPlayer<GunFirePlayer>();
        gf.gunfireAcc = true;
        gf.heatGainPerShot = 1;
        gf.overloadBonus = 0f;          
        gf.extraDamage = 5;             
        gf.extraDamageCanCrit = false;  

        player.GetAttackSpeed(DamageClass.Ranged) += 0.05f;
        player.GetCritChance(DamageClass.Ranged) += 5f;
    }
}