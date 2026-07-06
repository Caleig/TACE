using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Accessories.FallenCovenant;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Projectiles.Healer;

namespace ThoriumAccessoryExpansion.Accessories.KarmaCovenant
{
    public class KarmaCovenant : ModItem
    {
        public override LocalizedText Tooltip
        {
            get
            {
                return base.Tooltip.WithFormatArgs(new object[]
                {
                    KarmicHolder.LifeStep,
                    KarmicHolder.TotalLife,
                    KarmicHolder.DamageStep,
                    KarmicHolder.LifeStep
                });
            }
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(gold: 5);
            Item.defense = 3;
        }

        public override void AddRecipes()
        {
            int fallenCovenantType = ModContent.ItemType<FallenCovenant.FallenCovenant>();
            int darkHeartType = ModContent.Find<ModItem>("ThoriumMod", "DarkHeart")?.Type ?? ItemID.DirtBlock;

            CreateRecipe()
                .AddIngredient(fallenCovenantType, 1)
                .AddIngredient(darkHeartType, 1)
                .AddIngredient(ModContent.ItemType<BloomWeave>(), 10)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            ThoriumPlayer thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
            thoriumPlayer.healBonus -= 1;
            player.GetModPlayer<ThoriumPlayer>().darkIntent = true;
            player.GetModPlayer<ThoriumPlayer>().darkAura = true;
            player.GetModPlayer<CovenantPlayer>().KarmaHasCovenant = true;
            thoriumPlayer.karmicHolder = true;
            if (player.whoAmI == Main.myPlayer && thoriumPlayer.healStreak >= 0)
            {
                int type = ModContent.ProjectileType<KarmicHolderPro>();
                if (player.ownedProjectileCounts[type] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_Accessory(base.Item, null), player.Center, Vector2.Zero, type, 0, 0f, player.whoAmI, 0f, 0f, 0f);
                }
            }
        }
        // Token: 0x0400119A RID: 4506
        public static readonly int LifeStep = 100;

        // Token: 0x0400119B RID: 4507
        public static readonly int TotalLife = 500;

        // Token: 0x0400119C RID: 4508
        public static readonly int DamageStep = 8;
    }
}