using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Utilities;
using ThoriumAccessoryExpansion.Buffs;

namespace ThoriumAccessoryExpansion.Players;

public class BardAccessoryPlayer : ModPlayer
{
    public bool HasInspirationWirelessHeadphones;
    public bool HasOrchestraConductorScore;

    private int inspirationSpent;

    public override void ResetEffects()
    {
        HasInspirationWirelessHeadphones = false;
        HasOrchestraConductorScore = false;
    }

    public override void UpdateEquips()
    {
        if (!HasInspirationWirelessHeadphones)
        {
            inspirationSpent = 0;
            return;
        }

        int empowermentCount =
            CountActiveEmpowerments();

        Player.moveSpeed +=
            empowermentCount * 0.02f;
    }

    public override float UseSpeedMultiplier(
        Item item)
    {
        if (item.ModItem is not BardItem)
            return 1f;

        float speedBonus = 0f;
        if (HasInspirationWirelessHeadphones)
        {
            speedBonus +=
                CountActiveEmpowerments() * 0.01f;
        }
        if (HasOrchestraConductorScore)
        {
            speedBonus += 0.12f;
        }
        CrystalKineticConverterPlayer converter =
            Player.GetModPlayer<
                CrystalKineticConverterPlayer
            >();

        if (converter.HasCrystalKineticConverter)
        {
            speedBonus +=
                converter.CountNearbyTeammates() * 0.05f;
        }
        if (
            Player.HasBuff(
                ModContent.BuffType<
                    InspirationOverflowBuff
                >()
            )
        )
        {
            speedBonus += 0.25f;
        }

        return 1f + speedBonus;
    }

    public override void PostUpdate()
    {
        if (
            HasInspirationWirelessHeadphones &&
            inspirationSpent >= 70
        )
        {
            while (inspirationSpent >= 70)
            {
                inspirationSpent -= 70;

                Player.AddBuff(
                    ModContent.BuffType<
                        InspirationOverflowBuff
                    >(),
                    300
                );
            }
        }
    }

    public void AddInspirationSpent(
        int amount)
    {
        if (
            !HasInspirationWirelessHeadphones ||
            amount <= 0
        )
        {
            return;
        }

        inspirationSpent += amount;
    }

    private int CountActiveEmpowerments()
    {
        ThoriumPlayer thoriumPlayer =
            Player.GetThoriumPlayer();

        int count = 0;

        if (
            thoriumPlayer
                .GetEmpTimer<AquaticAbility>()
                .timer > 0
        )
        {
            count++;
        }

        if (
            thoriumPlayer
                .GetEmpTimer<AttackSpeed>()
                .timer > 0
        )
        {
            count++;
        }

        if (
            thoriumPlayer
                .GetEmpTimer<CriticalStrikeChance>()
                .timer > 0
        )
        {
            count++;
        }

        if (
            thoriumPlayer
                .GetEmpTimer<Damage>()
                .timer > 0
        )
        {
            count++;
        }

        if (
            thoriumPlayer
                .GetEmpTimer<DamageReduction>()
                .timer > 0
        )
        {
            count++;
        }

        if (
            thoriumPlayer
                .GetEmpTimer<Defense>()
                .timer > 0
        )
        {
            count++;
        }

        if (
            thoriumPlayer
                .GetEmpTimer<FlatDamage>()
                .timer > 0
        )
        {
            count++;
        }

        if (
            thoriumPlayer
                .GetEmpTimer<FlightTime>()
                .timer > 0
        )
        {
            count++;
        }

        if (
            thoriumPlayer
                .GetEmpTimer<InvincibilityFrames>()
                .timer > 0
        )
        {
            count++;
        }

        if (
            thoriumPlayer
                .GetEmpTimer<JumpHeight>()
                .timer > 0
        )
        {
            count++;
        }

        if (
            thoriumPlayer
                .GetEmpTimer<LifeRegeneration>()
                .timer > 0
        )
        {
            count++;
        }

        if (
            thoriumPlayer
                .GetEmpTimer<MovementSpeed>()
                .timer > 0
        )
        {
            count++;
        }

        if (
            thoriumPlayer
                .GetEmpTimer<ResourceConsumptionChance>()
                .timer > 0
        )
        {
            count++;
        }

        if (
            thoriumPlayer
                .GetEmpTimer<ResourceGrabRange>()
                .timer > 0
        )
        {
            count++;
        }

        if (
            thoriumPlayer
                .GetEmpTimer<ResourceMaximum>()
                .timer > 0
        )
        {
            count++;
        }

        if (
            thoriumPlayer
                .GetEmpTimer<ResourceRegen>()
                .timer > 0
        )
        {
            count++;
        }

        return count;
    }
}