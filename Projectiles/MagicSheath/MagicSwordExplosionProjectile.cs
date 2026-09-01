using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Projectiles.MagicSheath;

public class MagicSwordExplosionProjectile
    : ModProjectile
{
    public override string Texture =>
        "ThoriumAccessoryExpansion/Accessories/Magic/MagicSheath/BlastPro";

    public override void SetDefaults()
    {
        Projectile.width = 120;
        Projectile.height = 120;

        Projectile.timeLeft = 1;

        Projectile.friendly = true;
        Projectile.hostile = false;

        Projectile.tileCollide = false;

        Projectile.penetrate = -1;

        Projectile.DamageType =
            DamageClass.Magic;

        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;
    }

    public override void AI()
    {
        if (Main.dedServ)
            return;

        for (int i = 0; i < 25; i++)
        {
            Dust dust =
                Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Torch,
                    0f,
                    0f,
                    100,
                    default,
                    1.4f
                );

            dust.noGravity = true;

            dust.velocity =
                Main.rand.NextVector2Circular(
                    6f,
                    6f
                );
        }

        Lighting.AddLight(
            Projectile.Center,
            0.8f,
            0.35f,
            0.1f
        );
    }
}