using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.MagicItems;
using ThoriumMod.Items.NPCItems;

namespace ThoriumAccessoryExpansion.Accessories.Healer.HeresyCovenant;

public class HeresyCovenant : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;

        Item.accessory = true;

        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(gold: 2);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<DemonTongue>())
            .AddIngredient(ModContent.ItemType<DarkEffigy>())
            .AddIngredient(ModContent.ItemType<DarkIntent>())
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
        thoriumPlayer.darkIntent = true;
        thoriumPlayer.darkAura = true;

        player.aggro += 400;

        player.GetDamage(
            ModContent.GetInstance<HealerDamage>()
        ) += 0.20f;

        player.GetCritChance(
            ModContent.GetInstance<HealerDamage>()
        ) += 15f;

        player.GetModPlayer<CovenantPlayer>()
            .HeresyHasCovenant = true;
    }
}