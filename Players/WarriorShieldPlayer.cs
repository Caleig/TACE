using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Players;

public class WarriorShieldPlayer : ModPlayer
{
    public float StoredDamage;
    private float ReleasedStoredDamage;
    public bool CrystallineRetaliationShield;
    public bool TectonicAccumulationShield;
    public bool BurstingBarrier;
    public bool VoidmetalRetaliationShield;
    public bool PiercingBarrier;
    private bool wasBelowHalfHealth;
    private int tectonicShieldCooldown;
    private int burstingShieldCooldown;
    private int burstingExplosionCooldown;

    public override void ResetEffects()
    {
        CrystallineRetaliationShield = false;
        TectonicAccumulationShield = false;
        BurstingBarrier = false;
        VoidmetalRetaliationShield = false;
        PiercingBarrier = false;
    }

    public override void UpdateDead()
    {
        StoredDamage = 0f;

        wasBelowHalfHealth = false;

        tectonicShieldCooldown = 0;
        burstingShieldCooldown = 0;
        burstingExplosionCooldown = 0;
    }

    public override void OnHurt(Player.HurtInfo info)
    {
        if (BurstingBarrier)
        {
            StoreDamage(info.Damage, 3f, 1200f);
        }
        else if (TectonicAccumulationShield)
        {
            StoreDamage(info.Damage, 2f, 600f);
        }
        else if (VoidmetalRetaliationShield)
        {
            StoreDamage(info.Damage, 2f, 450f);
        }
        else if (PiercingBarrier)
        {
            StoreDamage(info.Damage, 2f, 600f);
        }
        else if (CrystallineRetaliationShield)
        {
            StoreDamage(info.Damage, 2f, 450f);
        }
    }

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if ((BurstingBarrier || TectonicAccumulationShield) &&
            Player.statLife < Player.statLifeMax2 * 0.5f)
        {
            modifiers.FinalDamage *= 0.75f;
        }
        if (PiercingBarrier &&
            Player.statLife >= Player.statLifeMax2)
        {
            modifiers.FinalDamage *= 3f;

            int maxDamage =
                (int)(Player.statLifeMax2 * 0.5f);

            modifiers.SetMaxDamage(maxDamage);
        }
        if (BurstingBarrier &&
            burstingExplosionCooldown <= 0 &&
            StoredDamage > 0f)
        {
            modifiers.ModifyHurtInfo += PreventFatalDamage;
        }
    }

    private void PreventFatalDamage(ref Player.HurtInfo info)
    {
        if (info.Damage >= Player.statLife)
        {
            float storedDamage = StoredDamage;
            info.Damage = Player.statLife - 1;

            if (info.Damage < 0)
                info.Damage = 0;
            TriggerBurstingExplosion(storedDamage);
            int shieldAmount = (int)(storedDamage * 0.25f);

            if (shieldAmount > 0)
            {
                AddLifeShield(shieldAmount);
            }
            burstingExplosionCooldown = 120 * 60;
        }
    }

    public override void ModifyHitNPC(
        NPC target,
        ref NPC.HitModifiers modifiers)
    {
        ReleasedStoredDamage = 0f;

        if (StoredDamage > 0f)
        {
            if (CrystallineRetaliationShield ||
                TectonicAccumulationShield ||
                BurstingBarrier ||
                VoidmetalRetaliationShield ||
                PiercingBarrier)
            {
                ReleasedStoredDamage = StoredDamage;
                modifiers.FinalDamage.Base += StoredDamage;
                StoredDamage = 0f;
            }
        }
        if (PiercingBarrier &&
            Player.statLife < Player.statLifeMax2 * 0.75f)
        {
            modifiers.FinalDamage *= 1.15f;
        }
    }

    public override void PostUpdate()
    {
        if (tectonicShieldCooldown > 0)
            tectonicShieldCooldown--;
        if (burstingShieldCooldown > 0)
            burstingShieldCooldown--;
        if (burstingExplosionCooldown > 0)
            burstingExplosionCooldown--;
        if (!TectonicAccumulationShield &&
            !BurstingBarrier)
        {
            wasBelowHalfHealth = false;
            return;
        }

        bool belowHalfHealth =
            Player.statLife < Player.statLifeMax2 * 0.5f;
        if (belowHalfHealth &&
            !wasBelowHalfHealth)
        {
            if (BurstingBarrier)
            {
                if (burstingShieldCooldown <= 0)
                {
                    TriggerLowHealthShield(
                        ref burstingShieldCooldown,
                        60);
                }
            }
            else if (TectonicAccumulationShield)
            {
                if (tectonicShieldCooldown <= 0)
                {
                    TriggerLowHealthShield(
                        ref tectonicShieldCooldown,
                        60);
                }
            }
        }

        wasBelowHalfHealth = belowHalfHealth;
    }

    private void TriggerLowHealthShield(
        ref int cooldown,
        int cooldownSeconds)
    {
        int shieldAmount = (int)(StoredDamage * 0.25f);

        if (shieldAmount <= 0)
            return;

        int oldShield =
            Player.GetThoriumPlayer().shieldHealth;

        AddLifeShield(shieldAmount);

        int newShield =
            Player.GetThoriumPlayer().shieldHealth;

        int actualShield =
            newShield - oldShield;
        if (actualShield > 0)
        {
            cooldown = cooldownSeconds * 60;
        }
    }

    private void TriggerBurstingExplosion(float storedDamage)
    {
        SoundEngine.PlaySound(
            SoundID.Item14,
            Player.position);

        if (Main.myPlayer != Player.whoAmI)
            return;

        int projectileType =
            ModContent.ProjectileType<TheSeaMineProPulse>();

        int damage =
            (int)(storedDamage * 3f);

        if (damage < 1)
            damage = 1;

        const float projectileCount = 12f;

        for (int i = 0; i < projectileCount; i++)
        {
            Vector2 offset = Vector2.Zero;

            offset +=
                -Vector2.UnitY.RotatedBy(
                    i * (MathHelper.TwoPi / projectileCount))
                * new Vector2(8f, 8f);

            offset = offset.RotatedBy(
                Player.velocity.ToRotation());

            Vector2 velocity =
                Vector2.Normalize(offset) * 4f;

            Projectile.NewProjectile(
                Player.GetSource_Misc("BurstingBarrier"),
                Player.Center + offset,
                velocity,
                projectileType,
                damage,
                0f,
                Main.myPlayer);
        }
    }

    public override void OnHitNPC(
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {
        if (VoidmetalRetaliationShield)
        {
            if (damageDone > 0)
            {
                float originalDamage =
                    damageDone - ReleasedStoredDamage;

                if (originalDamage > 0f)
                {
                    float additionalStoredDamage =
                        originalDamage * 0.15f;

                    StoreDamage(
                        additionalStoredDamage,
                        1f,
                        450f);
                }
            }
        }
        if (PiercingBarrier &&
            Player.statLife < Player.statLifeMax2 * 0.75f &&
            damageDone > 0)
        {
            int healAmount =
                (int)(damageDone * 0.01f);

            if (healAmount < 1)
                healAmount = 1;

            Player.statLife += healAmount;

            if (Player.statLife > Player.statLifeMax2)
                Player.statLife = Player.statLifeMax2;
        }

        ReleasedStoredDamage = 0f;
    }

    private void StoreDamage(
        float amount,
        float multiplier,
        float maxStoredDamage)
    {
        StoredDamage += amount * multiplier;

        if (StoredDamage > maxStoredDamage)
            StoredDamage = maxStoredDamage;

        if (Main.myPlayer == Player.whoAmI)
        {
            CombatText.NewText(
                Player.getRect(),
                Color.Cyan,
                (int)StoredDamage);
        }
    }

    public void AddLifeShield(int amount)
    {
        if (amount <= 0)
            return;

        ThoriumPlayer thoriumPlayer =
            Player.GetThoriumPlayer();

        int available =
            ThoriumPlayer.ShieldHealthMax -
            thoriumPlayer.shieldHealth;

        int actualAmount =
            amount < available
                ? amount
                : available;

        if (actualAmount <= 0)
            return;

        thoriumPlayer.shieldHealth += actualAmount;

        Player.statLife += actualAmount;
    }
}