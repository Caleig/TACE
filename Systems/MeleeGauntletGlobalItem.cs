using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Systems;

public class MeleeGauntletGlobalItem : GlobalItem
{
    public override void ModifyItemScale(
        Item item,
        Player player,
        ref float scale)
    {
        if (!item.DamageType.CountsAsClass(DamageClass.Melee))
        {
            return;
        }

        MeleeGauntletPlayer gauntlet =
            player.GetModPlayer<MeleeGauntletPlayer>();

        if (!gauntlet.HasDarkMatterGauntlet)
        {
            return;
        }

        int tier = gauntlet.GetKineticTier();
        scale *= 1f + tier * 0.05f;
    }

    public override void ModifyHitNPC(
        Item item,
        Player player,
        NPC target,
        ref NPC.HitModifiers modifiers)
    {
        if (!item.DamageType.CountsAsClass(DamageClass.Melee))
        {
            return;
        }

        MeleeGauntletPlayer gauntlet =
            player.GetModPlayer<MeleeGauntletPlayer>();

        if (gauntlet.HasTitanBracer)
        {
            modifiers.CritDamage *= 1.25f;
        }
    }

    public override void OnHitNPC(
        Item item,
        Player player,
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {
        if (!item.DamageType.CountsAsClass(DamageClass.Melee))
        {
            return;
        }

        if (damageDone <= 0)
        {
            return;
        }

        MeleeGauntletPlayer gauntlet =
            player.GetModPlayer<MeleeGauntletPlayer>();
        gauntlet.RegisterMeleeHit();
        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            return;
        }
        if (
            gauntlet.HasSoulHunterGlove &&
            Main.rand.Next(100) < 15
        )
        {
            target.SimpleStrikeNPC(
                damageDone,
                hit.HitDirection,
                false,
                0f,
                DamageClass.Melee,
                false,
                0f
            );
        }
        if (
            gauntlet.HasTitanBracer &&
            Main.rand.Next(100) < 25
        )
        {
            target.SimpleStrikeNPC(
                damageDone,
                hit.HitDirection,
                false,
                0f,
                DamageClass.Melee,
                false,
                0f
            );
        }
    }
}