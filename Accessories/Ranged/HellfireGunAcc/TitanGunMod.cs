using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;

namespace ThoriumAccessoryExpansion.Accessories.Ranged.HellfireGunAcc;


public class TitanGunMod : ThoriumItem
{
    public const float FragileEndurance = -0.2f; 

    public override void SetDefaults()
    {
        Item.width = 36;
        Item.height = 26;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<GunFirePlayer>().titanAcc = true;
        player.endurance += FragileEndurance;
        
        
    }
}
