using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.Healer.BoneCovenant;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;

namespace ThoriumAccessoryExpansion.Accessories.Healer.FallenCovenant
{
    public class FallenCovenant : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.sellPrice(gold: 2);
        }

        public override void AddRecipes()
        {
            int boneCovenantType = ModContent.ItemType<BoneCovenant.BoneCovenant>();

            CreateRecipe()
                .AddIngredient(boneCovenantType, 1)
                .AddIngredient(ModContent.ItemType<ClericEmblem>(), 1)
                .AddIngredient(ItemID.SoulofMight, 1)
                .AddIngredient(ItemID.SoulofSight, 1)
                .AddIngredient(ItemID.SoulofFright, 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            thoriumPlayer.healBonus -= 1;
            player.GetModPlayer<ThoriumPlayer>().darkIntent = true;
            player.GetModPlayer<ThoriumPlayer>().darkAura = true;

            player.GetModPlayer<CovenantPlayer>().FallenHasCovenant = true;
        }
    }
}