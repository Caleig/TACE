using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;
using Terraria.ID;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Accessories.Summon.GemContract
{
    public class AmethystContract : GemContractBase
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.accessory = true;

            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 5);
        }


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            GemContractPlayer contract =
                player.GetModPlayer<GemContractPlayer>();

            contract.amethystContract = true;
            contract.magicContractActive = true;
        }
    }
}