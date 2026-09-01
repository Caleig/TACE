using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Ranged.GunModification;

public class TitanGunMod
    : GunModificationItem
{
    public const float FragileEndurance =
        -0.20f;


    public override void SetDefaults()
    {
        Item.width = 36;
        Item.height = 26;
        Item.accessory = true;
    }


    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        GunModificationPlayer modification =
            player.GetModPlayer<
                GunModificationPlayer
            >();

        modification.HasTitanGunMod = true;

        player.endurance +=
            FragileEndurance;
    }
    public override void AddRecipes()
    {
        Mod thorium = ModLoader.GetMod("ThoriumMod");

        int titanicBarType =
            thorium.Find<ModItem>("TitanicBar").Type;

        CreateRecipe()
            .AddIngredient(titanicBarType, 15)
            .AddIngredient(ItemID.IllegalGunParts, 1)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}