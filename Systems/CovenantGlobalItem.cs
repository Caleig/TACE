using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Buffs;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Buffs;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Systems;

public class CovenantGlobalItem : GlobalItem
{
    private static bool IsRadiantWeapon(Item item)
    {
        return item.DamageType ==
            ModContent.GetInstance<HealerDamage>();
    }

    public override void ModifyWeaponDamage(
        Item item,
        Player player,
        ref StatModifier damage)
    {
        if (!IsRadiantWeapon(item))
            return;

        CovenantPlayer cp =
            player.GetModPlayer<CovenantPlayer>();

        if (cp.BoneHasCovenant)
        {
            int bonus =
                (int)(
                    cp.FallenRadianceStacks *
                    10 /
                    CovenantPlayer.GlobalMaxStacks
                );

            damage.Flat += bonus;
            return;
        }

        if (cp.CursedHasCovenant)
        {
            int bonus =
                (int)(
                    cp.FallenRadianceStacks *
                    5 /
                    CovenantPlayer.GlobalMaxStacks
                );

            damage.Flat += bonus;
            return;
        }

        if (cp.FallenHasCovenant)
        {
            int bonus =
                (int)(
                    cp.FallenRadianceStacks *
                    12 /
                    CovenantPlayer.GlobalMaxStacks
                );

            damage.Flat += bonus;
            return;
        }

        if (cp.KarmaHasCovenant)
        {
            int bonus =
                (int)(
                    cp.FallenRadianceStacks *
                    15 /
                    CovenantPlayer.GlobalMaxStacks
                );

            damage.Flat += bonus;
        }
    }

    public override void ModifyWeaponCrit(
        Item item,
        Player player,
        ref float crit)
    {
        if (!IsRadiantWeapon(item))
            return;

        CovenantPlayer cp =
            player.GetModPlayer<CovenantPlayer>();

        if (
            cp.FallenHasCovenant &&
            cp.FallenRadianceStacks >=
            CovenantPlayer.GlobalMaxStacks
        )
        {
            crit += 5f;
        }

        if (
            cp.KarmaHasCovenant &&
            cp.FallenRadianceStacks >=
            CovenantPlayer.GlobalMaxStacks
        )
        {
            crit += 8f;
        }
    }

    public override bool Shoot(
        Item item,
        Player player,
        EntitySource_ItemUse_WithAmmo source,
        Vector2 position,
        Vector2 velocity,
        int type,
        int damage,
        float knockback)
    {
        if (!IsRadiantWeapon(item))
        {
            return base.Shoot(
                item,
                player,
                source,
                position,
                velocity,
                type,
                damage,
                knockback
            );
        }

        CovenantPlayer cp =
            player.GetModPlayer<CovenantPlayer>();

        int lifeCost = 0;
        int chargeGain = 0;

        if (cp.BoneHasCovenant)
        {
            lifeCost = 1;
            chargeGain = 6;
        }
        else if (cp.CursedHasCovenant)
        {
            lifeCost = 1;
            chargeGain = 12;
        }
        else if (cp.FallenHasCovenant)
        {
            lifeCost = 2;
        }
        else if (cp.KarmaHasCovenant)
        {
            lifeCost = 3;
        }

        if (
            lifeCost > 0 &&
            player.statLife > lifeCost
        )
        {
            player.statLife -= lifeCost;

            CombatText.NewText(
                new Rectangle(
                    (int)player.position.X,
                    (int)player.position.Y,
                    player.width,
                    player.height
                ),
                CombatText.DamagedFriendly,
                lifeCost,
                false
            );

            if (chargeGain > 0)
            {
                cp.FallenRadianceStacks =
                    (int)MathHelper.Clamp(
                        cp.FallenRadianceStacks +
                        chargeGain,
                        0,
                        CovenantPlayer.GlobalMaxStacks
                    );

                cp.FallenRadianceTimer = 3600;
            }

            player.AddBuff(
                ModContent.BuffType<LifeDrainCooldown>(),
                5
            );
        }

        return base.Shoot(
            item,
            player,
            source,
            position,
            velocity,
            type,
            damage,
            knockback
        );
    }

    public override void OnHitNPC(
        Item item,
        Player player,
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {
        if (!IsRadiantWeapon(item))
            return;

        CovenantPlayer cp =
            player.GetModPlayer<CovenantPlayer>();

        if (cp.BoneHasCovenant)
        {
            target.AddBuff(
                ModContent.BuffType<LightCurse>(),
                300
            );
        }
        else if (cp.CursedHasCovenant)
        {
            target.AddBuff(
                ModContent.BuffType<LightCurse>(),
                300
            );
        }
        else if (cp.FallenHasCovenant)
        {
            cp.FallenRadianceStacks =
                (int)MathHelper.Clamp(
                    cp.FallenRadianceStacks + 5,
                    0,
                    CovenantPlayer.GlobalMaxStacks
                );

            cp.FallenRadianceTimer = 3600;

            target.AddBuff(
                BuffID.ShadowFlame,
                300
            );
        }
        else if (cp.KarmaHasCovenant)
        {
            cp.FallenRadianceStacks =
                (int)MathHelper.Clamp(
                    cp.FallenRadianceStacks + 4,
                    0,
                    CovenantPlayer.GlobalMaxStacks
                );

            cp.FallenRadianceTimer = 3600;

            target.AddBuff(
                ModContent.BuffType<UnholyKarma>(),
                300
            );
        }

        base.OnHitNPC(
            item,
            player,
            target,
            hit,
            damageDone
        );
    }
}