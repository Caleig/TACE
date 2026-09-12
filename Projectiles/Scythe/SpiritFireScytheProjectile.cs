using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Projectiles.Scythe;

public class SpiritFireScytheProjectile :
    SoulScytheProjectile
{
    public override void OnHitNPC(
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {
        base.OnHitNPC(
            target,
            hit,
            damageDone
        );


        if (target.life > 0)
            return;


        if (
            Main.myPlayer !=
            Projectile.owner
        )
        {
            return;
        }


        Projectile.NewProjectile(
            Projectile.GetSource_FromThis(),
            target.Center,
            Vector2.Zero,
            ModContent.ProjectileType<
                LifeSpiritProjectile
            >(),
            0,
            0f,
            Projectile.owner
        );
    }
}