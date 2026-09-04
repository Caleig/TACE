using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Warrior;

public class TectonicAccumulationShield : WarriorShieldBase
{
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;

        Item.accessory = true;
        Item.defense = 6;
        Item.rare = ItemRarityID.LightRed;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<WarriorShieldPlayer>()
            .TectonicAccumulationShield = true;
    }
    public override void AddRecipes()
    {
        Mod thorium = ModLoader.GetMod("ThoriumMod");

        int lodeStoneIngotType =
            thorium.Find<ModItem>("LodeStoneIngot").Type;

        CreateRecipe()
            .AddIngredient<CrystallineRetaliationShield>()
            .AddIngredient(lodeStoneIngotType, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}