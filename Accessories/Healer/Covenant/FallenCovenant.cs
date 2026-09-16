using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Accessories.Healer.Covenant;

public class FallenCovenant : CovenantAccessoryItem
{
    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.width = 24;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(gold: 2);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<BoneCovenant>()
            .AddIngredient<ClericEmblem>()
            .AddIngredient(ItemID.SoulofMight)
            .AddIngredient(ItemID.SoulofSight)
            .AddIngredient(ItemID.SoulofFright)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }

    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        ThoriumPlayer thoriumPlayer =
            player.GetThoriumPlayer();

        thoriumPlayer.healBonus -= 1;
        thoriumPlayer.darkIntent = true;
        thoriumPlayer.darkAura = true;

        CovenantPlayer cp =
            player.GetModPlayer<CovenantPlayer>();

        cp.FallenHasCovenant = true;

        if (
            cp.FallenRadianceStacks >=
            CovenantPlayer.GlobalMaxStacks
        )
        {
            player.GetDamage(
                ModContent.GetInstance<HealerDamage>()
            ) += 0.12f;
        }
    }
}