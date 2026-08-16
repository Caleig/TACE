using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

public class GunFirePlayer : ModPlayer
{
    /// <summary>
    /// 枪过热饰品
    /// </summary>
    public bool gunfireAcc;

    public override void ResetEffects()
    {
        gunfireAcc = false;
    }
}