using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Players;

public class MagicSwordEnhancementPlayer : ModPlayer
{
    public bool HasSpiritBlade;

    public bool HasBlazingScroll;
    public bool HasEnergyScroll;
    public bool HasGeodeScroll;
    public bool HasHolyScroll;
    public bool HasSoulScroll;


    public override void ResetEffects()
    {
        HasSpiritBlade = false;

        HasBlazingScroll = false;
        HasEnergyScroll = false;
        HasGeodeScroll = false;
        HasHolyScroll = false;
        HasSoulScroll = false;
    }


    public override void PostUpdateEquips()
    {
        base.PostUpdateEquips();

        if (HasSpiritBlade)
        {
            Player.GetDamage(
                DamageClass.Magic
            ).Flat += 5;
        }

        if (HasBlazingScroll)
        {
            Player.GetDamage(
                DamageClass.Magic
            ) += 0.12f;
        }

        if (HasEnergyScroll)
        {
            Player.statManaMax2 += 40;
        }

        if (HasGeodeScroll)
        {
            Player.GetArmorPenetration(
                DamageClass.Generic
            ) += 8;
        }

        if (HasHolyScroll)
        {
            Player.GetCritChance(
                DamageClass.Magic
            ) += 8;
        }

        if (HasSoulScroll)
        {
            Player.GetDamage(
                DamageClass.Magic
            ) += 0.10f;

            Player.lifeRegen += 4;
        }
    }


    public override void ModifyManaCost(
        Item item,
        ref float reduce,
        ref float mult)
    {
        base.ModifyManaCost(
            item,
            ref reduce,
            ref mult
        );

        if (
            HasEnergyScroll &&
            item.DamageType == DamageClass.Magic
        )
        {
            mult *= 0.95f;
        }
    }
}