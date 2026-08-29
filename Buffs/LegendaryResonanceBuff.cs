using Terraria;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;


namespace ThoriumAccessoryExpansion.Buffs;


public class LegendaryResonanceBuff : ModBuff
{

    public override void SetStaticDefaults()
    {

        Main.buffNoSave[Type] =
            true;

    }



    public override void Update(
        Player player,
        ref int buffIndex)
    {

        GemContractPlayer contract =
            player.GetModPlayer<
                GemContractPlayer
            >();



        if (!contract.HasLegendaryResonance)
        {

            player.DelBuff(
                buffIndex
            );

            buffIndex--;

            return;

        }



        player.buffTime[buffIndex] =
            contract.GetLongestLegendaryResonanceTime();

    }



    public override void ModifyBuffText(
        ref string buffName,
        ref string tip,
        ref int rare)
    {

        GemContractPlayer contract =
            Main.LocalPlayer.GetModPlayer<
                GemContractPlayer
            >();



        buffName =
            $"{DisplayName.Value} " +
            $"({contract.LegendaryResonanceCount}/9)";



        tip =
            contract.GetLegendaryResonanceTooltip();

    }

}