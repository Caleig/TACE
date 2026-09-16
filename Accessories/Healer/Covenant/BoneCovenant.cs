using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;

namespace ThoriumAccessoryExpansion.Accessories.Healer.Covenant;

public class BoneCovenant : CovenantAccessoryItem
{
    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.width = 24;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(silver: 20);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<CursedCovenant>()
            .AddIngredient(ItemID.Bone, 15)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }

    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        ThoriumPlayer thoriumPlayer =
            player.GetModPlayer<ThoriumPlayer>();

        thoriumPlayer.healBonus -= 1;
        thoriumPlayer.darkAura = true;
        thoriumPlayer.darkIntent = true;

        player.GetModPlayer<CovenantPlayer>()
            .BoneHasCovenant = true;
    }
}