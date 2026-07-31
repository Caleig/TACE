using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Buffs;
using ThoriumMod.Buffs.Healer;

namespace ThoriumAccessoryExpansion.Accessories.MagicSheath
{
    public class MagicSwordPro : ModProjectile
    {
        private bool _fired = false;
        private Vector2 _targetPosition;
        private int _penetrateCount = 0;
        private float _orbitAngle = 0f;
        private float _orbitRadiusX = 60f;  // 椭圆X轴半径（水平）
        private float _orbitRadiusY = 20f;  // 椭圆Y轴半径（垂直），更大使剑在更高处
        private float _baseYOffset = 40f;   // 基础垂直偏移（让剑更高）
        private Vector2 _fireDirection;    // 发射时的初始方向，用于恒定飞行和追踪

        public bool Fired => _fired;

        public override void SetStaticDefaults()
        {
        }

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
            _penetrateCount = mp.GetExtraPenetrate();
            Projectile.penetrate = 1 + _penetrateCount;

            // 从 SwordIndices 中获取当前剑的索引位置
            int swordIndex = -1;
            for (int i = 0; i < mp.SwordIndices.Count; i++)
            {
                if (mp.SwordIndices[i] == Projectile.whoAmI)
                {
                    swordIndex = i;
                    break;
                }
            }

            if (swordIndex == -1)
                swordIndex = mp.SwordIndices.Count - 1;

            int totalSwords = mp.SwordIndices.Count;
            if (totalSwords <= 0) totalSwords = 1;

            float startAngle = -MathHelper.PiOver2;
            _orbitAngle = startAngle + (swordIndex * MathHelper.TwoPi / totalSwords);

            Projectile.netUpdate = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 2;

            // 魔法武器面板伤害+5
            player.GetDamage(DamageClass.Magic).Flat += 5;

            if (!_fired)
            {
                // ---- 悬浮模式：椭圆运动 ----
                _orbitAngle += 0.03f;
                if (_orbitAngle > MathHelper.TwoPi)
                    _orbitAngle -= MathHelper.TwoPi;

                float offsetX = _orbitRadiusX * (float)Math.Cos(_orbitAngle);
                float offsetY = _orbitRadiusY * (float)Math.Sin(_orbitAngle);

                Vector2 centerPos = player.Center - new Vector2(0, 50f + _baseYOffset);
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
                    Dust dust = Dust.NewDustDirect(
                        Projectile.position - new Vector2(4, 4),
                        Projectile.width + 8,
                        Projectile.height + 8,
                        DustID.MagicMirror,
                        0f, 0f, 100, default, 0.8f
                    );
                    dust.noGravity = true;
                    dust.velocity = -Projectile.velocity * 0.2f;
                }
            }
            else
            {
                // ---- 飞行模式：恒定速度，等级3时加权追踪，永不消失 ----
                float speed = 18f;

                // 安全获取当前方向
                Vector2 currentDir = _fireDirection;
                if (currentDir == Vector2.Zero)
                    currentDir = Projectile.velocity.SafeNormalize(Vector2.UnitY);

                Vector2 desiredDirection = currentDir;

                // 仅在泰拉级（SheathLevel == 3）启用加权平均追踪
                var mp = player.GetModPlayer<MagicSheathPlayer>();
                if (mp != null && mp.SheathLevel == 3)
                {
                    NPC target = FindClosestNPC(500f);
                    if (target != null)
                    {
                        Vector2 toTarget = target.Center - Projectile.Center;
                        if (toTarget.Length() > 0)
                            toTarget.Normalize();

                        // 加权平均：当前方向 70%，敌人方向 30%
                        float weight = 0.3f;
                        desiredDirection = (currentDir * (1 - weight) + toTarget * weight);
                        desiredDirection.Normalize();
                    }
                }

                // 平滑转向到目标方向，并保持恒定速度
                Projectile.velocity = (Projectile.velocity * 8f + desiredDirection * speed) / 9f;

                // 更新旋转与朝向
                if (Projectile.velocity.Length() > 0.5f)
                {
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                }
                Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;

                // 光照与粒子
                Lighting.AddLight(Projectile.Center, 0.4f, 0.2f, 0.8f);
                if (Main.rand.NextBool(2))
                {
                    Dust dust = Dust.NewDustDirect(
                        Projectile.position - new Vector2(2, 2),
                        Projectile.width + 4,
                        Projectile.height + 4,
                        DustID.MagicMirror,
                        0f, 0f, 100, default, 1f
                    );
                    dust.noGravity = true;
                    dust.velocity = -Projectile.velocity * 0.3f + Main.rand.NextVector2Circular(1f, 1f);
                }
            }
        }

        public void FireAtMouse()
        {
            if (_fired) return;
            _fired = true;
            _targetPosition = Main.MouseWorld;

            Vector2 dir = _targetPosition - Projectile.Center;
            if (dir.Length() > 0)
                dir.Normalize();
            Projectile.velocity = dir * 18f;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // 保存发射方向，用于后续飞行及追踪
            _fireDirection = dir;

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
                texture = ModContent.Request<Texture2D>("ThoriumAccessoryExpansion/Accessories/MagicSheath/MagicSwordPro_Terra").Value;
            else
                texture = ModContent.Request<Texture2D>("ThoriumAccessoryExpansion/Accessories/MagicSheath/MagicSwordPro").Value;

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

        // 在范围内寻找最近的敌对NPC
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