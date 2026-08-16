using Terraria;
using Terraria.DataStructures;
using ThoriumMod;
using ThoriumMod.Items;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

/*
 *枪械改件（狱岩）
枪械速度提高5%
枪械暴击率增加5%
缓慢使枪械过热
过热后造成少量额外伤害并匀速冷却
彻底冷却前不会再次积蓄热量
 */
public class Test : ThoriumItem
{
    // 暂时懒得写的
    public override void SetDefaults()
    {
        Item.accessory = true;
    }
    /// <summary>
    /// 主要用于更新饰品的
    /// </summary>
    /// <param name="player"></param>
    /// <param name="hideVisual"></param>
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<GunFirePlayer>().gunfireAcc = true;
    }
}