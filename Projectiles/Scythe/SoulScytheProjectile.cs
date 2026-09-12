using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Projectiles.Scythe;

public class SoulScytheProjectile : ModProjectile
{
    public override string Texture =>
        "Terraria/Images/Projectile_" + ProjectileID.MagicMissile;

    public override void SetDefaults()
    {
        Projectile.width = 48;
        Projectile.height = 48;

        Projectile.friendly = true;
        Projectile.hostile = false;

        Projectile.DamageType =
            ThoriumMod.HealerDamage.Instance;

        Projectile.penetrate = 3;
        Projectile.timeLeft = 90;
        Projectile.tileCollide = false;

        Projectile.ignoreWater = true;
        Projectile.light = 0.8f;
        Projectile.extraUpdates = 1;
    }

    public override void AI()
    {
        Projectile.rotation += 0.25f;
        Projectile.velocity *= 1.02f;
        if (Projectile.velocity.LengthSquared() > 20f * 20f)
        {
            Projectile.velocity =
                Vector2.Normalize(Projectile.velocity) * 20f;
        }
        Lighting.AddLight(
            Projectile.Center,
            0.8f,
            0.8f,
            0.8f);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        int originalProjectileType =
            (int)Projectile.ai[0];

        if (originalProjectileType <= 0)
            return true;

        Texture2D texture;

        ModProjectile originalModProjectile =
            ProjectileLoader.GetProjectile(
                originalProjectileType);

        if (originalModProjectile != null)
        {
            Asset<Texture2D> asset =
                ModContent.Request<Texture2D>(
                    originalModProjectile.Texture);

            texture = asset.Value;
        }
        else
        {
            texture =
                TextureAssets.Projectile[
                    originalProjectileType].Value;
        }

        Rectangle frame = texture.Frame();

        Vector2 origin =
            frame.Size() / 2f;

        Main.EntitySpriteDraw(
            texture,
            Projectile.Center - Main.screenPosition,
            frame,
            Color.White,
            Projectile.rotation,
            origin,
            Projectile.scale,
            SpriteEffects.None);

        return false;
    }
}