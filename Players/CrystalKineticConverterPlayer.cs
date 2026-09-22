using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;
using ThoriumMod.Utilities;
using ThoriumAccessoryExpansion.Buffs;

namespace ThoriumAccessoryExpansion.Players;

public class CrystalKineticConverterPlayer : ModPlayer
{
    public const int StorageMaximum = 1500;
    public const int ActiveDuration = 1800;
    public const float AllyRange = 800f;

    public bool HasCrystalKineticConverter;

    private int storedSonicDamage;

    public int StoredSonicDamage =>
        storedSonicDamage;

    public override void ResetEffects()
    {
        HasCrystalKineticConverter = false;
    }

    public void AddSonicDamage(int damage)
    {
        if (
            !HasCrystalKineticConverter ||
            damage <= 0
        )
        {
            return;
        }

        storedSonicDamage += damage;

        if (
            storedSonicDamage <
            StorageMaximum
        )
        {
            return;
        }

        storedSonicDamage = 0;

        ActivateConversion();
    }

    private void ActivateConversion()
    {
        for (int i = 0; i < Main.maxPlayers; i++)
        {
            Player target =
                Main.player[i];

            if (
                !target.active ||
                target.dead
            )
            {
                continue;
            }

            if (target == Player)
            {
                target.AddBuff(
                    ModContent.BuffType<
                        CrystalKineticConversionBuff
                    >(),
                    ActiveDuration
                );

                continue;
            }

            if (
                Player.team == 0 ||
                target.team == 0 ||
                target.team != Player.team
            )
            {
                continue;
            }

            if (
                Vector2.DistanceSquared(
                    Player.Center,
                    target.Center
                ) >
                AllyRange * AllyRange
            )
            {
                continue;
            }

            target.AddBuff(
                ModContent.BuffType<
                    CrystalKineticConversionBuff
                >(),
                ActiveDuration
            );
        }
    }

    public int CountNearbyTeammates()
    {
        if (Player.team == 0)
            return 0;

        int count = 0;

        for (int i = 0; i < Main.maxPlayers; i++)
        {
            if (i == Player.whoAmI)
                continue;

            Player teammate =
                Main.player[i];

            if (
                !teammate.active ||
                teammate.dead
            )
            {
                continue;
            }

            if (
                teammate.team != Player.team
            )
            {
                continue;
            }

            if (
                Vector2.DistanceSquared(
                    Player.Center,
                    teammate.Center
                ) <=
                AllyRange * AllyRange
            )
            {
                count++;
            }
        }

        return count;
    }

    public override void UpdateDead()
    {
        storedSonicDamage = 0;
    }
}