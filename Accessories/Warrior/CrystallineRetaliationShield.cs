using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Warrior;

public class CrystallineRetaliationShield : WarriorShieldBase
{
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;

        Item.accessory = true;
        Item.defense = 5;
        Item.rare = ItemRarityID.LightRed;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<WarriorShieldPlayer>().CrystallineRetaliationShield = true;
    }
    public override void AddRecipes()
    {
        Mod thorium = ModLoader.GetMod("ThoriumMod");

        int championsRebuttalType =
            thorium.Find<ModItem>("ChampionsRebuttal").Type;

        int crystalGeodeType =
            thorium.Find<ModItem>("CrystalGeode").Type;

        CreateRecipe()
            .AddIngredient(championsRebuttalType)
            .AddIngredient(crystalGeodeType, 10)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}