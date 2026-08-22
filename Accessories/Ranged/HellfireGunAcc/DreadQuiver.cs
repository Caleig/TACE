using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace ThoriumAccessoryExpansion.Accessories.Ranged.HellfireGunAcc;

public class DreadQuiver : ThoriumItem
{
    public const float ArrowSpeedMult = 1.3f;
    public const float CopyDamage = 0.15f;
    public const int AggroReduction = 400;

    public override void SetDefaults()
    {
        Item.width = 42;
        Item.height = 46;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<GunFirePlayer>().dreadQuiver = true;

        player.GetDamage(DamageClass.Ranged) += 0.15f;
        player.GetCritChance(DamageClass.Ranged) += 8f;
        player.aggro -= AggroReduction;
    }
}

public class QuiverArrowGlobal : GlobalProjectile
{
    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.arrow;

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (Main.player[projectile.owner].GetModPlayer<GunFirePlayer>().dreadQuiver)
            projectile.velocity *= DreadQuiver.ArrowSpeedMult;
    }

    public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Main.player[projectile.owner].GetModPlayer<GunFirePlayer>().dreadQuiver)
        {
            int extraDamage = (int)(projectile.damage * DreadQuiver.CopyDamage);
            if (extraDamage > 0)
            {
                target.SimpleStrikeNPC(extraDamage, hit.HitDirection, hit.Crit, projectile.knockBack, DamageClass.Ranged);
            }
        }
    }
}

public class QuiverAmmoGlobal : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.ammo == AmmoID.Arrow;

    public override bool CanBeConsumedAsAmmo(Item weapon, Item ammo, Player player) =>
        player.GetModPlayer<GunFirePlayer>().dreadQuiver && Main.rand.NextFloat() < 0.25f
            ? false
            : base.CanBeConsumedAsAmmo(weapon, ammo, player);
}
