using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Warrior;

public class VoidmetalRetaliationShield : WarriorShieldBase
{
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;

        Item.accessory = true;
        Item.defense = 3;
        Item.rare = ItemRarityID.LightRed;
    }

    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        player.GetModPlayer<WarriorShieldPlayer>()
            .VoidmetalRetaliationShield = true;
    }
    public override void AddRecipes()
    {
        Mod thorium = ModLoader.GetMod("ThoriumMod");

        int valadiumIngotType =
            thorium.Find<ModItem>("ValadiumIngot").Type;

        CreateRecipe()
            .AddIngredient<CrystallineRetaliationShield>()
            .AddIngredient(valadiumIngotType, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}