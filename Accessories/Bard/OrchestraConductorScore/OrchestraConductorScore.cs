using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Accessories.Bard.OrchestraConductorScore;

public class OrchestraConductorScore : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;

        Item.accessory = true;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(gold: 3);
    }

    public override void AddRecipes()
    {
        Mod thorium =
            ModLoader.GetMod("ThoriumMod");

        int bandKitType =
            thorium.Find<ModItem>("BandKit").Type;

        int conductorBatonType =
            thorium.Find<ModItem>("ConductorsBaton").Type;

        int bloomWeaveType =
            thorium.Find<ModItem>("BloomWeave").Type;

        CreateRecipe()
            .AddIngredient(bandKitType)
            .AddIngredient(conductorBatonType)
            .AddIngredient(bloomWeaveType, 8)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }

    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        player.GetDamage(
            BardDamage.Instance
        ) += 0.12f;

        player.GetAttackSpeed(
            BardDamage.Instance
        ) += 0.12f;

        player.GetThoriumPlayer()
            .inspirationRegenBonus += 0.12f;

        player.GetModPlayer<BardAccessoryPlayer>()
            .HasOrchestraConductorScore = true;
    }
}