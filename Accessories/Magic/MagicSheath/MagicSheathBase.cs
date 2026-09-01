using Terraria;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath;

public abstract class MagicSheathBase : ModItem
{
    public abstract int SheathLevel { get; }


    public override bool CanEquipAccessory(
        Player player,
        int slot,
        bool modded)
    {
        for (
            int i = 3;
            i < player.armor.Length;
            i++
        )
        {
            if (i == slot)
                continue;


            if (
                player.armor[i].ModItem
                is MagicSheathBase
            )
            {
                return false;
            }
        }


        return true;
    }


    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        player
            .GetModPlayer<
                MagicSheathPlayer
            >()
            .SetSheathLevel(
                SheathLevel
            );
    }


    public abstract override void AddRecipes();
}