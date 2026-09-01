using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Projectiles.MagicSheath;

public class MagicSwordProjectile : ModProjectile
{
    private const float OrbitRadiusX = 60f;
    private const float OrbitRadiusY = 20f;
    private const float OrbitHeight = 90f;

    public bool IsFired =>
        Projectile.ai[0] == 1f;

    public override string Texture =>
        "ThoriumAccessoryExpansion/Accessories/Magic/MagicSheath/MagicSwordPro";

    public override void SetDefaults()
    {
        Projectile.width = 16;
        Projectile.height = 16;

        Projectile.timeLeft = 2;

        Projectile.friendly = true;
        Projectile.hostile = false;

        Projectile.tileCollide = true;

        Projectile.DamageType =
            DamageClass.Magic;

        Projectile.penetrate = 1;

        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 10;
    }

    public override void OnSpawn(
        Terraria.DataStructures.IEntitySource source)
    {
        Projectile.penetrate =
            1 +
            (int)Projectile.ai[1];
    }

    public override void AI()
    {
        if (
            Projectile.owner < 0 ||
            Projectile.owner >= Main.maxPlayers
        )
        {
            Projectile.Kill();
            return;
        }

        Player player =
            Main.player[Projectile.owner];

        if (
            !player.active ||
            player.dead
        )
        {
            Projectile.Kill();
            return;
        }

        MagicSheathPlayer sheath =
            player.GetModPlayer<
                MagicSheathPlayer
            >();

        if (sheath.SheathLevel <= 0)
        {
            Projectile.Kill();
            return;
        }

        Projectile.timeLeft = 2;

        if (!IsFired)
        {
            UpdateOrbit(
                player
            );
        }
        else
        {
            UpdateFlight(
                player,
                sheath
            );
        }
    }

    private void UpdateOrbit(
        Player player)
    {
        int totalSwords =
            GetOwnedSwordCount(
                player
            );

        if (totalSwords <= 0)
            return;

        int ordinal =
            GetSwordOrdinal(
                player
            );

        float angle =
            Main.GameUpdateCount *
            0.03f;

        angle +=
            ordinal *
            MathHelper.TwoPi /
            totalSwords;

        angle -=
            MathHelper.PiOver2;

        Vector2 center =
            player.Center -
            new Vector2(
                0f,
                OrbitHeight
            );

        Vector2 target =
            center +
            new Vector2(
                OrbitRadiusX *
                    (float)System.Math.Cos(
                        angle
                    ),
                OrbitRadiusY *
                    (float)System.Math.Sin(
                        angle
                    )
            );

        float distance =
            Vector2.Distance(
                Projectile.Center,
                target
            );

        if (distance > 2000f)
        {
            Projectile.Center =
                target;

            Projectile.velocity =
                Vector2.Zero;

            return;
        }

        if (distance > 5f)
        {
            Vector2 direction =
                Projectile.DirectionTo(
                    target
                );

            Projectile.velocity =
                (
                    Projectile.velocity * 15f +
                    direction * 12f
                ) / 16f;
        }
        else
        {
            Projectile.velocity *= 0.9f;
        }

        Projectile.rotation = 0f;

        Lighting.AddLight(
            Projectile.Center,
            0.2f,
            0.1f,
            0.6f
        );

        if (Main.rand.NextBool(3))
        {
            Dust dust =
                Dust.NewDustDirect(
                    Projectile.position -
                    new Vector2(4f),
                    Projectile.width + 8,
                    Projectile.height + 8,
                    DustID.MagicMirror,
                    0f,
                    0f,
                    100,
                    default,
                    0.8f
                );

            dust.noGravity = true;

            dust.velocity =
                -Projectile.velocity * 0.2f;
        }
    }

    private void UpdateFlight(
    Player player,
    MagicSheathPlayer sheath)
    {
        float speed = 18f;

        Vector2 currentDirection =
            Projectile.velocity.SafeNormalize(
                Vector2.UnitY
            );

        Vector2 desiredDirection =
            currentDirection;


        NPC target =
            FindClosestNPC(
                500f
            );


        if (target != null)
        {
            Vector2 targetDirection =
                Projectile.DirectionTo(
                    target.Center
                );

            const float homingStrength =
                0.3f;

            desiredDirection =
                Vector2.Normalize(
                    Vector2.Lerp(
                        currentDirection,
                        targetDirection,
                        homingStrength
                    )
                );
        }


        Projectile.velocity =
            (
                Projectile.velocity * 8f +
                desiredDirection * speed
            ) / 9f;


        if (
            Projectile.velocity.LengthSquared()
            > 0.25f
        )
        {
            Projectile.rotation =
                Projectile.velocity.ToRotation() +
                MathHelper.PiOver2;

            Projectile.spriteDirection =
                Projectile.velocity.X >= 0f
                    ? 1
                    : -1;
        }


        Lighting.AddLight(
            Projectile.Center,
            0.4f,
            0.2f,
            0.8f
        );


        if (Main.rand.NextBool(2))
        {
            Dust dust =
                Dust.NewDustDirect(
                    Projectile.position -
                    new Vector2(2f),
                    Projectile.width + 4,
                    Projectile.height + 4,
                    DustID.MagicMirror,
                    0f,
                    0f,
                    100,
                    default,
                    1f
                );

            dust.noGravity = true;

            dust.velocity =
                -Projectile.velocity * 0.3f +
                Main.rand.NextVector2Circular(
                    1f,
                    1f
                );
        }
    }

    public void FireAt(
        Vector2 target)
    {
        if (
            IsFired ||
            Projectile.owner != Main.myPlayer
        )
        {
            return;
        }

        Vector2 direction =
            Projectile.DirectionTo(
                target
            );

        Projectile.ai[0] = 1f;

        Projectile.velocity =
            direction * 18f;

        Projectile.rotation =
            Projectile.velocity.ToRotation() +
            MathHelper.PiOver2;

        Projectile.netUpdate = true;
    }

    private int GetOwnedSwordCount(
        Player player)
    {
        int swordCount = 0;

        foreach (
            Projectile projectile
            in Main.projectile
        )
        {
            if (
                projectile.active &&
                projectile.owner ==
                    player.whoAmI &&
                projectile.type ==
                    Type
            )
            {
                swordCount++;
            }
        }

        return swordCount;
    }

    private int GetSwordOrdinal(
        Player player)
    {
        int ordinal = 0;

        foreach (
            Projectile projectile
            in Main.projectile
        )
        {
            if (
                !projectile.active ||
                projectile.owner !=
                    player.whoAmI ||
                projectile.type !=
                    Type
            )
            {
                continue;
            }

            if (
                projectile.whoAmI <
                Projectile.whoAmI
            )
            {
                ordinal++;
            }
        }

        return ordinal;
    }

    private NPC FindClosestNPC(
        float maxRange)
    {
        NPC closest = null;
        float closestDistance =
            maxRange;

        foreach (
            NPC npc
            in Main.npc
        )
        {
            if (
                !npc.CanBeChasedBy(
                    this
                )
            )
            {
                continue;
            }

            float distance =
                Vector2.Distance(
                    Projectile.Center,
                    npc.Center
                );

            if (
                distance <
                closestDistance
            )
            {
                closestDistance =
                    distance;

                closest = npc;
            }
        }

        return closest;
    }

    public override bool OnTileCollide(
        Vector2 oldVelocity)
    {
        Projectile.Kill();

        return false;
    }

    public override bool PreDraw(
        ref Color lightColor)
    {
        Player player =
            Main.player[
                Projectile.owner
            ];

        if (
            !player.active
        )
        {
            return false;
        }

        MagicSheathPlayer sheath =
            player.GetModPlayer<
                MagicSheathPlayer
            >();

        string texturePath =
            sheath.SheathLevel >= 3
                ? "ThoriumAccessoryExpansion/Accessories/Magic/MagicSheath/MagicSwordPro_Terra"
                : "ThoriumAccessoryExpansion/Accessories/Magic/MagicSheath/MagicSwordPro";

        Texture2D texture =
            ModContent.Request<Texture2D>(
                texturePath
            ).Value;

        Vector2 drawPosition =
            Projectile.Center -
            Main.screenPosition;

        Color drawColor =
            Lighting.GetColor(
                (int)(
                    Projectile.Center.X / 16f
                ),
                (int)(
                    Projectile.Center.Y / 16f
                )
            );

        Main.EntitySpriteDraw(
            texture,
            drawPosition,
            null,
            drawColor,
            Projectile.rotation,
            texture.Size() / 2f,
            Projectile.scale,
            Projectile.spriteDirection == 1
                ? SpriteEffects.None
                : SpriteEffects.FlipHorizontally,
            0
        );

        return false;
    }
}