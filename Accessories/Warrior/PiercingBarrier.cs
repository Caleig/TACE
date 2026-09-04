using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Warrior;

public class PiercingBarrier : WarriorShieldBase
{
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;

        Item.accessory = true;
        Item.rare = ItemRarityID.LightRed;
    }

    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        player.GetModPlayer<WarriorShieldPlayer>()
            .PiercingBarrier = true;
        player.aggro -= 400;
    }
    public override void AddRecipes()
    {
        Mod thorium = ModLoader.GetMod("ThoriumMod");

        int gutWrenchersGauntletType =
            thorium.Find<ModItem>("GutWrenchersGauntlet").Type;

        CreateRecipe()
            .AddIngredient<VoidmetalRetaliationShield>()
            .AddIngredient(gutWrenchersGauntletType)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}