using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Utilities;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Bard.CrystalKineticConverter;

public class CrystalKineticConverter : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;

        Item.accessory = true;
        Item.rare = ItemRarityID.Cyan;
        Item.value = Item.sellPrice(gold: 5);
    }

    public override void AddRecipes()
    {
        Mod thorium =
            ModLoader.GetMod("ThoriumMod");

        int shockAbsorberType =
            thorium.Find<ModItem>(
                "ShockAbsorber"
            ).Type;

        int concertTicketsType =
            thorium.Find<ModItem>(
                "ConcertTickets"
            ).Type;

        CreateRecipe()
            .AddIngredient(shockAbsorberType)
            .AddIngredient(concertTicketsType)
            .AddIngredient(ItemID.CrystalShard, 10)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }

    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        ThoriumPlayer thoriumPlayer =
            player.GetThoriumPlayer();

        thoriumPlayer.inspirationRegenBonus +=
            0.10f;

        player.GetModPlayer<
            CrystalKineticConverterPlayer
        >().HasCrystalKineticConverter = true;
    }
}