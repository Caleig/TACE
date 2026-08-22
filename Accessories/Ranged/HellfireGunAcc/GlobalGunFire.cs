using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.Donate;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Accessories.Ranged.HellfireGunAcc;

public class GlobalGunFire : GlobalItem
{
    public const int SlowGunUseTime = 30;
    public const float TitanSpeedMult = 1.3f;
    public const float TitanSlowGunBonus = 0.75f;
    public const float TitanCritBonus = 1f;

    public override bool AppliesToEntity(Item entity, bool lateInstantiation) =>
        entity.useAmmo == AmmoID.Bullet && entity.ModItem is not HellfireMinigun;

    public override bool? UseItem(Item item, Player player)
    {
        var gf = player.GetModPlayer<GunFirePlayer>();
        if (!gf.gunfireAcc || gf.heatGainPerShot <= 0)
            return base.UseItem(item, player);
        var tp = player.GetThoriumPlayer();
        if (!tp.hellfireEnergyOverload) 
        {
            SoundEngine.PlaySound(in item.UseSound, new Vector2?(player.Center));
            tp.hellfireEnergy += gf.heatGainPerShot;
        }
        return base.UseItem(item, player);
    }

    public override float UseTimeMultiplier(Item item, Player player) =>
        player.GetModPlayer<GunFirePlayer>().titanAcc ? TitanSpeedMult : 1f;

    public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
    {
        var gf = player.GetModPlayer<GunFirePlayer>();
        
        if (gf.titanAcc && item.useTime >= SlowGunUseTime)
            damage += TitanSlowGunBonus;
        
    }
}