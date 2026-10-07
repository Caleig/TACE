using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;
using ThoriumAccessoryExpansion.Players;

namespace ThoriumAccessoryExpansion.Systems;

public class CrystalKineticConverterGlobalItem
    : GlobalItem
{
    public override void OnHitNPC(
        Item item,
        Player player,
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {
        if (item.ModItem is not BardItem)
            return;

        if (damageDone <= 0)
            return;

        player.GetModPlayer<
            CrystalKineticConverterPlayer
        >().AddSonicDamage(damageDone);
    }
}