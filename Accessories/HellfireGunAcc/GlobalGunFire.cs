using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

public class GlobalGunFire : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation) => entity.useAmmo == AmmoID.Bullet;
    public override bool? UseItem(Item item, Player player)
    {
        // 如果这里没有触发饰品效果
        if(!player.GetModPlayer<GunFirePlayer>().gunfireAcc)
            return base.UseItem(item, player);
        // 这里压根没有写过联机同步(指原Mod)
        var thoriumPlayer = player.GetThoriumPlayer();
        if (!thoriumPlayer.hellfireEnergyOverload) // 这里的判断条件是没有触发过热
        {
            SoundEngine.PlaySound(in item.UseSound, new Vector2?(player.Center));
            thoriumPlayer.hellfireEnergy += 2; // 这里的上限是100 
        }
        return base.UseItem(item, player);
    }
}