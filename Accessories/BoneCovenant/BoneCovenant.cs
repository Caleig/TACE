using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.CursedCovenant;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;

namespace ThoriumAccessoryExpansion.Accessories.BoneCovenant
{
    public class BoneCovenant : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(silver: 20);
        }

        public override void AddRecipes()
        {
            int cursedCovenantType = ModContent.ItemType<CursedCovenant.CursedCovenant>();
            CreateRecipe()
                .AddIngredient(cursedCovenantType, 1)
                .AddIngredient(ItemID.Bone, 15)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            thoriumPlayer.healBonus -= 1;
            player.GetModPlayer<ThoriumPlayer>().darkAura = true;
            thoriumPlayer.darkIntent = true;

            player.GetModPlayer<CovenantPlayer>().BoneHasCovenant = true;
        }
    }
}