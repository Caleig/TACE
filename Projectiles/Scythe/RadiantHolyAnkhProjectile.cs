using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Projectiles.Scythe;
public class RadiantHolyAnkhProjectile : SoulScytheProjectile
{
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);
        Mod thorium = ModLoader.GetMod("ThoriumMod");
        int holyGlare = thorium.Find<ModBuff>("HolyGlare").Type;
        int tuned = thorium.Find<ModBuff>("Tuned").Type;
        target.AddBuff(holyGlare, 300);
        target.AddBuff(tuned, 300);
        GrantSoulEssence();
    }

    private void GrantSoulEssence()
    {
        if (Main.netMode == NetmodeID.Server || Projectile.owner != Main.myPlayer)
            return;
        if (Projectile.localAI[0] != 0f)
            return;

        int scytheCharge = ScytheItem.GetScytheChargeFromPro((int)Projectile.ai[0]);
        if (scytheCharge <= 0)
            return;

        Projectile.localAI[0] = 1f;
        Player player = Main.player[Projectile.owner];
        int buff = ModLoader.GetMod("ThoriumMod").Find<ModBuff>("SoulEssence").Type;
        player.AddBuff(buff, 1800);
        player.GetThoriumPlayer().soulEssence += scytheCharge;
        player.GetModPlayer<SoulOfScythePlayer>().GiveSoulHealing(scytheCharge);
    }
}
