using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Projectiles.Scythe;

public class StyxBloodAnkhProjectile :
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
    }
}