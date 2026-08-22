using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Accessories.Ranged.HellfireGunAcc;

public class GlobalBulletCrit : GlobalProjectile
{
    public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
    {
        var owner = Main.player[projectile.owner];
        var gf = owner.GetModPlayer<GunFirePlayer>();

        if (owner.HeldItem.useAmmo != AmmoID.Bullet)
            return;

        if (gf.titanAcc)
            modifiers.CritDamage += GlobalGunFire.TitanCritBonus;
    }

    public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
    {
        var owner = Main.player[projectile.owner];
        var gf = owner.GetModPlayer<GunFirePlayer>();

        if (owner.HeldItem.useAmmo != AmmoID.Bullet)
            return;

        if (gf.gunfireAcc && owner.GetThoriumPlayer().hellfireEnergyOverload && gf.extraDamage > 0)
        {
            bool crit = gf.extraDamageCanCrit && hit.Crit;
            target.SimpleStrikeNPC(gf.extraDamage, hit.HitDirection, crit, projectile.knockBack);
        }
    }
}
