using Terraria;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Systems
{
    public class MagicContractGlobalItem : GlobalItem
    {

        public override void ModifyWeaponDamage(
            Item item,
            Player player,
            ref StatModifier damage)
        {

            GemContractPlayer contract =
                player.GetModPlayer<GemContractPlayer>();


            if (!contract.magicContractActive)
                return;


            if (item.DamageType != DamageClass.Magic)
                return;

            StatModifier magic =
                player.GetTotalDamage(
                    DamageClass.Magic
                );

            StatModifier summon =
                player.GetTotalDamage(
                    DamageClass.Summon
                );

            StatModifier target =
                new StatModifier(
                    summon.Additive,
                    summon.Multiplicative * 0.65f,
                    summon.Flat,
                    summon.Base / 0.65f
                );

            StatModifier replacement =
                new StatModifier(
                    target.Additive
                    - magic.Additive
                    + 1f,

                    magic.Multiplicative != 0f
                        ? target.Multiplicative
                            / magic.Multiplicative
                        : target.Multiplicative,

                    target.Flat
                    - magic.Flat,

                    target.Base
                    - magic.Base
                );

            damage =
                damage.CombineWith(
                    replacement
                );

        }

        public static bool IsMagicContractWeapon(
            Item item,
            Player player)
        {

            GemContractPlayer contract =
                player.GetModPlayer<GemContractPlayer>();


            return contract.magicContractActive
                && item.DamageType == DamageClass.Magic;

        }

    }
}