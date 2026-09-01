using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Players;

public class GunModificationPlayer : ModPlayer
{
    public bool HasHellstoneGunMod;
    public bool HasGreenDragonGunMod;
    public bool HasFleshGunMod;
    public bool HasFleshTrigger;
    public bool HasTitanGunMod;
    public bool HasDreadQuiver;


    private int heat;

    private bool heatOverloaded;

    private bool pendingOverloadAttack;

    private ulong pendingOverloadTick;


    public int Heat =>
        heat;


    public int HeatMaximum
    {
        get
        {
            if (HasFleshTrigger)
                return 150;

            if (HasHeatModification)
                return 100;

            return 0;
        }
    }


    public bool HasHeatModification =>
        HasHellstoneGunMod ||
        HasGreenDragonGunMod ||
        HasFleshGunMod ||
        HasFleshTrigger;


    public bool IsOverloading =>
        HasHeatModification &&
        heatOverloaded;


    public int HeatGainPerAttack
    {
        get
        {
            if (HasFleshTrigger)
                return 2;

            return HasHeatModification
                ? 1
                : 0;
        }
    }


    public int HeatOverloadCost
    {
        get
        {
            if (HasHellstoneGunMod)
                return 2;

            if (
                HasGreenDragonGunMod ||
                HasFleshGunMod ||
                HasFleshTrigger
            )
            {
                return 1;
            }

            return 0;
        }
    }


    public int HeatOverloadDamage
    {
        get
        {
            if (HasGreenDragonGunMod)
                return 10;

            if (HasFleshTrigger)
                return 10;

            if (HasFleshGunMod)
                return 6;

            if (HasHellstoneGunMod)
                return 5;

            return 0;
        }
    }


    public bool HeatOverloadCanCrit =>
        HasFleshTrigger;


    public override void ResetEffects()
    {
        HasHellstoneGunMod = false;
        HasGreenDragonGunMod = false;
        HasFleshGunMod = false;
        HasFleshTrigger = false;
        HasTitanGunMod = false;
        HasDreadQuiver = false;
    }


    public override void PostUpdate()
    {
        if (!HasHeatModification)
        {
            heat = 0;
            heatOverloaded = false;

            pendingOverloadAttack = false;
            pendingOverloadTick = 0;

            return;
        }
        if (
            pendingOverloadAttack &&
            pendingOverloadTick !=
                Main.GameUpdateCount
        )
        {
            pendingOverloadAttack = false;
            pendingOverloadTick = 0;
        }
        if (
            !heatOverloaded &&
            heat >= HeatMaximum
        )
        {
            heat = HeatMaximum;
            heatOverloaded = true;
        }
        if (
            heat > HeatMaximum
        )
        {
            heat = HeatMaximum;
        }
        if (
            heatOverloaded &&
            heat <= 0
        )
        {
            heat = 0;
            heatOverloaded = false;
        }
    }

    public void AddHeat(int amount)
    {
        if (
            !HasHeatModification ||
            heatOverloaded ||
            amount <= 0
        )
        {
            return;
        }


        heat =
            System.Math.Min(
                HeatMaximum,
                heat + amount
            );


        if (
            heat >= HeatMaximum
        )
        {
            heat = HeatMaximum;
            heatOverloaded = true;
        }
    }

    public void BeginOverloadAttack()
    {
        if (!IsOverloading)
            return;


        pendingOverloadAttack = true;

        pendingOverloadTick =
            Main.GameUpdateCount;
    }

    public bool IsPendingOverloadProjectile()
    {
        return
            pendingOverloadAttack &&
            pendingOverloadTick ==
                Main.GameUpdateCount;
    }

    public bool TryConsumeHeatForHit()
    {
        if (
            !IsOverloading ||
            HeatOverloadCost <= 0
        )
        {
            return false;
        }


        if (
            heat <= 0
        )
        {
            heat = 0;
            heatOverloaded = false;

            return false;
        }


        heat =
            System.Math.Max(
                0,
                heat - HeatOverloadCost
            );


        if (
            heat <= 0
        )
        {
            heat = 0;
            heatOverloaded = false;
        }


        return true;
    }


    public override void UpdateDead()
    {
        heat = 0;

        heatOverloaded = false;

        pendingOverloadAttack = false;
        pendingOverloadTick = 0;
    }
}