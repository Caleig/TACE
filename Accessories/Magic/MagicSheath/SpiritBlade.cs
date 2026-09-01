using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath;
using ThoriumMod.NPCs.BossBuriedChampion;

namespace ThoriumAccessoryExpansion.Accessories.Magic.MagicSheath;

public class SpiritBlade : MagicSwordEnhancementItem
{
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;

        Item.accessory = true;

        Item.rare =
            ItemRarityID.LightRed;

        Item.value =
            Item.sellPrice(gold: 1);
    }

    public override void UpdateAccessory(
        Player player,
        bool hideVisual)
    {
        player
            .GetModPlayer<
                MagicSwordEnhancementPlayer
            >()
            .HasSpiritBlade = true;
    }

    public override void AddRecipes()
    {
    }
}


public class SpiritBladeGlobalNPC : GlobalNPC
{
    public override void ModifyNPCLoot(
        NPC npc,
        NPCLoot npcLoot)
    {
        if (
            npc.type ==
            ModContent.NPCType<BuriedChampion>()
        )
        {
            npcLoot.Add(
                ItemDropRule.Common(
                    ModContent.ItemType<SpiritBlade>()
                )
            );
        }
    }
}