using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.NPCs.BossBuriedChampion;

namespace ThoriumAccessoryExpansion.Accessories.MagicSheath
{
    public class SpiritBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 1);
        }

        // 配方：由英灵遗骸掉落，无配方

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            
            player.GetModPlayer<MagicSheathPlayer>().HasSpiritBlade = true;
        }
    }
    public class SpiritBladeGlobalNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if(npc.type == ModContent.NPCType<BuriedChampion>())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SpiritBlade>())); // 10%掉落
            }
        }
    }
}