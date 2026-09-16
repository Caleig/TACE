using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Buffs;
using ThoriumMod.Items;

namespace ThoriumAccessoryExpansion.Systems;

public class HeresyCovenantGlobalItem : GlobalItem
{
    public override void ModifyHitNPC(
        Item item,
        Player player,
        NPC target,
        ref NPC.HitModifiers modifiers)
    {
        if (
            item.DamageType !=
            ModContent.GetInstance<HealerDamage>()
        )
        {
            return;
        }

        CovenantPlayer cp =
            player.GetModPlayer<CovenantPlayer>();

        if (!cp.HeresyHasCovenant)
            return;

        bool hasDebuff =
            target.HasBuff(BuffID.ShadowFlame) ||
            target.HasBuff(
                ModContent.BuffType<LightCurse>()
            );

        if (hasDebuff)
        {
            modifiers.FinalDamage *= 1.15f;

            cp.HeresyLifeRegenTimer = 120;
        }
    }

    public override void OnHitNPC(
        Item item,
        Player player,
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {
        if (
            item.DamageType !=
            ModContent.GetInstance<HealerDamage>()
        )
        {
            return;
        }

        CovenantPlayer cp =
            player.GetModPlayer<CovenantPlayer>();

        if (!cp.HeresyHasCovenant)
            return;

        target.AddBuff(
            BuffID.ShadowFlame,
            300
        );

        if (
            item.ModItem is ThoriumItem thoriumItem &&
            thoriumItem.radiantLifeCost > 0
        )
        {
            int cost =
                thoriumItem.radiantLifeCost;

            int heal = cost / 2;

            if (heal > 0)
            {
                player.statLife += heal;

                if (
                    player.statLife >
                    player.statLifeMax2
                )
                {
                    player.statLife =
                        player.statLifeMax2;
                }

                CombatText.NewText(
                    player.Hitbox,
                    CombatText.HealLife,
                    heal
                );
            }
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