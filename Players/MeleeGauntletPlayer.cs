using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Players;

public class MeleeGauntletPlayer : ModPlayer
{
    public bool HasDarkMatterGauntlet;
    public bool HasDemonBloodGauntlet;
    public bool HasSoulHunterGlove;
    public bool HasTitanBracer;

    public int DarkMatterKinetic;
    public int DarkMatterNoHitTimer;

    public override void ResetEffects()
    {
        HasDarkMatterGauntlet = false;
        HasDemonBloodGauntlet = false;
        HasSoulHunterGlove = false;
        HasTitanBracer = false;
    }

    public override void PostUpdate()
    {
        if (!HasDarkMatterGauntlet)
        {
            DarkMatterKinetic = 0;
            DarkMatterNoHitTimer = 0;
            return;
        }
        if (DarkMatterKinetic <= 0)
        {
            DarkMatterKinetic = 0;
            DarkMatterNoHitTimer = 0;
            return;
        }

        DarkMatterNoHitTimer++;
        if (DarkMatterNoHitTimer >= 300)
        {
            DarkMatterNoHitTimer = 0;

            DarkMatterKinetic -= 25;

            if (DarkMatterKinetic < 0)
            {
                DarkMatterKinetic = 0;
            }
        }
    }

    public void RegisterMeleeHit()
    {
        if (!HasDarkMatterGauntlet)
        {
            return;
        }
        if (DarkMatterKinetic < 100)
        {
            DarkMatterKinetic++;
        }
        DarkMatterNoHitTimer = 0;
    }

    public int GetKineticTier()
    {
        return DarkMatterKinetic / 25;
    }

    public override bool FreeDodge(Player.HurtInfo info)
    {
        if (!HasDemonBloodGauntlet)
        {
            return false;
        }
        if (info.Damage <= 0)
        {
            return false;
        }
        if (info.Damage >= Player.statLife)
        {
            return false;
        }
        if (Main.rand.Next(100) >= 20)
        {
            return false;
        }
        Player.Heal(info.Damage);
        return true;
    }
}