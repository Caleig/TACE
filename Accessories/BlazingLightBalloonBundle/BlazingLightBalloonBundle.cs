using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.Donate;
using ThoriumMod.Items.Terrarium;
using ThoriumMod.Utilities;
namespace ThoriumAccessoryExpansion.Accessories.BlazingLightBalloonBundle
{
    public class BlazingLightBalloonBundle : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 1);
            Item.wingSlot = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wings = 0;
            player.wingsLogic = 0;
            player.wingTime = 0f;
            player.GetJumpState<SandstormInABottleJump>().Enable();
            player.GetJumpState<BlizzardInABottleJump>().Enable();
            player.GetJumpState<CloudInABottleJump>().Enable();
            player.GetJumpState<TsunamiInABottleJump>().Enable();
            player.jumpSpeedBoost += 1.6f;
            // 幸运马掌效果：免疫摔落伤害
            player.noFallDmg = true;
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            thoriumPlayer.accIncandescentAlacrity = true;
            player.noFallDmg = true;
            player.runAcceleration += 0.25f;
            player.jumpSpeedBoost = 2.5f;
            if (player.controlDown && !player.controlUp)
            {
                player.maxFallSpeed *= (player.wet ? 2.25f : 2.5f);

                if (thoriumPlayer.falling >= 0 && player.velocity.Y < player.maxFallSpeed && player.velocity.Y > 2)
                {
                    player.velocity.Y = player.velocity.Y + 0.2f;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<IncandescentAlacrity>(), 1)
                .AddIngredient(ModContent.ItemType<TerrariumCore>(), 5)
                .AddIngredient(5331)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}