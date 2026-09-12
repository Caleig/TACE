using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Projectiles.Scythe;

public class AnubisScytheProjectile :
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


        target.AddBuff(
            holyGlare,
            300
        );


        if (
            target.life <= 0 &&
            Main.netMode !=
            NetmodeID.MultiplayerClient
        )
        {
            Main.player[Projectile.owner]
                .Heal(3);
        }
    }
}