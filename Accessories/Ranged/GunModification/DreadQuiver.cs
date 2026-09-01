using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Ranged.GunModification;

public class DreadQuiver
    : GunModificationItem
{
    public const float ArrowSpeedMult = 1.3f;
    public const float CopyDamage = 0.15f;
    public const int AggroReduction = 400;


    public override void SetDefaults()
    {
        Item.width = 42;
        Item.height = 46;
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

        modification.HasDreadQuiver = true;

        player.GetDamage(
            DamageClass.Ranged
        ) += 0.15f;

        player.GetCritChance(
            DamageClass.Ranged
        ) += 8f;

        player.aggro -=
            AggroReduction;
    }
    public override void AddRecipes()
    {
        Mod thorium = ModLoader.GetMod("ThoriumMod");

        int dreadSoulType =
            thorium.Find<ModItem>("DreadSoul").Type;

        CreateRecipe()
            .AddIngredient(dreadSoulType, 10)
            .AddIngredient(ItemID.StalkersQuiver, 1)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}