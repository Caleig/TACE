using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Warrior;

public class BurstingBarrier : WarriorShieldBase
{
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;

        Item.accessory = true;
        Item.defense = 7;
        Item.rare = ItemRarityID.LightRed;
    }

    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        player.GetModPlayer<WarriorShieldPlayer>()
            .BurstingBarrier = true;
    }
    public override void AddRecipes()
    {
        Mod thorium = ModLoader.GetMod("ThoriumMod");

        int blastShieldType =
            thorium.Find<ModItem>("BlastShield").Type;

        CreateRecipe()
            .AddIngredient<TectonicAccumulationShield>()
            .AddIngredient(blastShieldType)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}