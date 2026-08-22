using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;          
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Buffs;
using ThoriumMod.Buffs.Healer;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath
{
    public class MagicSwordPro : ModProjectile
    {
        
        private float _orbitAngle = 0f;
        private const float OrbitRadiusX = 60f;
        private const float OrbitRadiusY = 20f;
        private const float BaseYOffset = 40f;

        public bool IsFired => (int)Projectile.ai[2] == 1;

        public override void SetStaticDefaults() { }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.timeLeft = 600;
            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.rotation = 0f;
            Projectile.alpha = 0;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player player = Main.player[Projectile.owner];
            var mp = player.GetModPlayer<MagicSheathPlayer>();
            Projectile.penetrate = 1 + (int)Projectile.ai[1];

            int index = (int)Projectile.ai[0];
            if (mp.SwordIndices.Count <= index)
            {
                while (mp.SwordIndices.Count <= index)
                    mp.SwordIndices.Add(-1);
            }
            mp.SwordIndices[index] = Projectile.whoAmI;

            
            int total = mp.SwordIndices.Count(idx => Main.projectile[idx].active && Main.projectile[idx].type == ModContent.ProjectileType<MagicSwordPro>());
            if (total <= 0) total = 1;
            float startAngle = -MathHelper.PiOver2;
            _orbitAngle = startAngle + (index * MathHelper.TwoPi / total);
            Projectile.netUpdate = true;
        }

        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            var mp = player.GetModPlayer<MagicSheathPlayer>();
            if (mp != null)
                mp.SwordIndices.Remove(Projectile.whoAmI);
        }

        private bool FindSheath()
        {
            Player player = Main.player[Projectile.owner];
            for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
            {
                int type = player.armor[i].type;
                if (type == ModContent.ItemType<SpiritMagicSheath>() ||
                    type == ModContent.ItemType<MagicSheath>() ||
                    type == ModContent.ItemType<TerraMagicSheath>())
                    return true;
            }
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !FindSheath())
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 2;

            
            player.GetDamage(DamageClass.Magic).Flat += 5;

            if (!IsFired)
            {
                
                _orbitAngle += 0.03f;
                if (_orbitAngle > MathHelper.TwoPi) _orbitAngle -= MathHelper.TwoPi;

                float offsetX = OrbitRadiusX * (float)Math.Cos(_orbitAngle);
                float offsetY = OrbitRadiusY * (float)Math.Sin(_orbitAngle);
                Vector2 centerPos = player.Center - new Vector2(0, 50f + BaseYOffset);
                Vector2 targetPos = centerPos + new Vector2(offsetX, offsetY);

                float speed = 12f;
                float distance = Vector2.Distance(Projectile.Center, targetPos);
                if (distance > 2000f)
                {
                    Projectile.position = targetPos - new Vector2(Projectile.width / 2f, Projectile.height / 2f);
                    Projectile.velocity = Vector2.Zero;
                }
                else if (distance > 5f)
                {
                    Vector2 dir = targetPos - Projectile.Center;
                    dir.Normalize();
                    Projectile.velocity = (Projectile.velocity * 15f + dir * speed) / 16f;
                }
                else
                {
                    Projectile.velocity *= 0.9f;
                }
                Projectile.rotation = 0f;
                Lighting.AddLight(Projectile.Center, 0.2f, 0.1f, 0.6f);

                if (Main.rand.NextBool(3))
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position - new Vector2(4, 4), Projectile.width + 8, Projectile.height + 8, DustID.MagicMirror, 0f, 0f, 100, default, 0.8f);
                    dust.noGravity = true;
                    dust.velocity = -Projectile.velocity * 0.2f;
                }
            }
            else
            {
                
                float speed = 18f;
                Vector2 currentDir = Projectile.velocity.SafeNormalize(Vector2.UnitY);
                Vector2 desiredDirection = currentDir;

                var mp = player.GetModPlayer<MagicSheathPlayer>();
                if (mp != null && mp.SheathLevel == 3)
                {
                    NPC target = FindClosestNPC(500f);
                    if (target != null)
                    {
                        Vector2 toTarget = target.Center - Projectile.Center;
                        if (toTarget.Length() > 0) toTarget.Normalize();
                        float weight = 0.3f;
                        desiredDirection = (currentDir * (1 - weight) + toTarget * weight);
                        desiredDirection.Normalize();
                    }
                }

                Projectile.velocity = (Projectile.velocity * 8f + desiredDirection * speed) / 9f;

                if (Projectile.velocity.Length() > 0.5f)
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;

                Lighting.AddLight(Projectile.Center, 0.4f, 0.2f, 0.8f);
                if (Main.rand.NextBool(2))
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position - new Vector2(2, 2), Projectile.width + 4, Projectile.height + 4, DustID.MagicMirror, 0f, 0f, 100, default, 1f);
                    dust.noGravity = true;
                    dust.velocity = -Projectile.velocity * 0.3f + Main.rand.NextVector2Circular(1f, 1f);
                }
            }
        }

        public void FireAtMouse()
        {
            if (IsFired) return;
            Projectile.ai[2] = 1;
            Vector2 target = Main.MouseWorld;
            Vector2 dir = target - Projectile.Center;
            if (dir.Length() > 0) dir.Normalize();
            Projectile.velocity = dir * 18f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.netUpdate = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            if (player == null || !player.active)
                return false;

            var mp = player.GetModPlayer<MagicSheathPlayer>();
            if (mp == null)
                return false;

            Texture2D texture;
            if (mp.SheathLevel == 3)
                texture = ModContent.Request<Texture2D>("ThoriumAccessoryExpansion/Accessories/Magic/MagicSheath/MagicSwordPro_Terra").Value;
            else
                texture = ModContent.Request<Texture2D>("ThoriumAccessoryExpansion/Accessories/Magic/MagicSheath/MagicSwordPro").Value;

            if (texture == null || texture.IsDisposed)
                return false;

            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            Rectangle sourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
            int frameHeight = texture.Height;
            if (frameHeight > 0)
            {
                sourceRect = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
                origin = new Vector2(texture.Width / 2f, frameHeight / 2f);
            }

            Color drawColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16));
            drawColor.A = 200;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.Additive,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                Main.Transform
            );

            Main.spriteBatch.Draw(
                texture,
                drawPosition,
                sourceRect,
                drawColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0f
            );

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null,
                Main.Transform
            );

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            var mp = player.GetModPlayer<MagicSheathPlayer>();

            if (mp.SheathLevel > 0)
            {
                int restorePercent = mp.GetManaRestorePercent();
                int restore = (int)(player.statManaMax2 * restorePercent / 100f);
                player.statMana += restore;
                if (player.statMana > player.statManaMax2)
                    player.statMana = player.statManaMax2;
                CombatText.NewText(player.Hitbox, CombatText.HealMana, restore);
            }

            var sp = player.GetModPlayer<ScrollPlayer>();
            if (mp.SheathLevel > 0 && sp.ActiveScrolls.Count > 0)
            {
                foreach (int scrollID in sp.ActiveScrolls)
                {
                    switch (scrollID)
                    {
                        case 0:
                            Explode(target.Center, damageDone);
                            target.AddBuff(BuffID.ShadowFlame, 600);
                            break;
                        case 1:
                            target.AddBuff(ModContent.BuffType<GraniteSurge>(), 600);
                            break;
                        case 2:
                            target.AddBuff(ModContent.BuffType<Sundered>(), 600);
                            break;
                        case 3:
                            target.AddBuff(ModContent.BuffType<HolyGlare>(), 600);
                            break;
                        case 4:
                            player.statLife += 5;
                            if (player.statLife > player.statLifeMax2)
                                player.statLife = player.statLifeMax2;
                            CombatText.NewText(player.Hitbox, CombatText.HealLife, 5);
                            break;
                    }
                }
            }
        }

        private void Explode(Vector2 position, int damage)
        {
            Projectile.NewProjectile(
                Projectile.GetSource_OnHit(null),
                position,
                Vector2.Zero,
                ModContent.ProjectileType<BlastPro>(),
                damage * 2,
                4f,
                Projectile.owner
            );
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }

        private NPC FindClosestNPC(float maxRange)
        {
            NPC closest = null;
            float minDist = maxRange;
            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy(this) && npc.active && !npc.friendly)
                {
                    float dist = Vector2.Distance(Projectile.Center, npc.Center);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closest = npc;
                    }
                }
            }
            return closest;
        }
    }
}