using Terraria;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Projectiles.Scythe;

public class RadiantHolyAnkhProjectile :
    SoulScytheProjectile
{
    public override void OnHitNPC(
        NPC target,
        NPC.HitInfo hit,
        int damageDone)
    {
        base.OnHitNPC(
            target,
            hit,
            damageDone
        );


        Mod thorium =
            ModLoader.GetMod("ThoriumMod");


        int holyGlare =
            thorium.Find<ModBuff>(
                "HolyGlare"
            ).Type;


        int tuned =
            thorium.Find<ModBuff>(
                "Tuned"
            ).Type;


        target.AddBuff(
            holyGlare,
            300
        );


        target.AddBuff(
            tuned,
            300
        );


        GrantSoulEssence();
    }


    private void GrantSoulEssence()
    {
        if (Projectile.localAI[0] != 0f)
            return;


        int originalProjectileType =
            (int)Projectile.ai[0];


        int scytheCharge =
            ThoriumMod.Items.HealerItems.ScytheItem
                .GetScytheChargeFromPro(
                    originalProjectileType
                );


        if (scytheCharge <= 0)
            return;


        Projectile.localAI[0] = 1f;


        Player player =
            Main.player[Projectile.owner];


        if (
            Main.netMode ==
            Terraria.ID.NetmodeID.MultiplayerClient
        )
        {
            return;
        }


        Mod thorium =
            ModLoader.GetMod("ThoriumMod");


        int soulEssenceBuff =
            thorium.Find<ModBuff>(
                "SoulEssence"
            ).Type;


        player.AddBuff(
            soulEssenceBuff,
            1800,
            true,
            false
        );


        ThoriumPlayer thoriumPlayer =
            player.GetThoriumPlayer();


        thoriumPlayer.soulEssence +=
            scytheCharge;


        player.GetModPlayer<
            SoulOfScythePlayer
        >().GiveSoulShield(
            scytheCharge
        );
    }
}