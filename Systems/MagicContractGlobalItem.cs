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
                player.GetTotalDamage(DamageClass.Magic);


            StatModifier summon =
                player.GetTotalDamage(DamageClass.Summon);



            float magicMultiplier =
                magic.Additive *
                magic.Multiplicative;


            float summonMultiplier =
                summon.Additive *
                summon.Multiplicative;



            if (magicMultiplier != 0)
            {
                damage *= summonMultiplier / magicMultiplier;
            }



            damage *= 0.65f;

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