using Terraria.ModLoader;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

/// <summary>
/// 枪械热系统状态：蹭 Thorium hellfireEnergy 当热量计（白嫖热条 UI），
/// 充满=增益态；Thorium 被动泄热用补偿抵消，热量只随攻击增减。
/// </summary>
public class GunFirePlayer : ModPlayer
{
    // Thorium 原版泄热：能量>0 时每 10 tick -1，约每秒 60/11 ≈ 5.4545
    private const float ThoriumDrainPerSec = 60f / 11f;

    public bool gunfireAcc;        // 发热类枪械改件已装备
    public int heatGain;           // 蓄热阶段每攻击 +N
    public int heatConsume;        // 增益阶段每攻击 -N
    public int heatCap;            // 容量（100 / 扳机150）
    public bool boosted;           // 热量充满，增益态
    public float flatDamage;       // 增益态固定伤害（不暴击，走 FlatBonusDamage）
    public bool flatCrits;         // 扳机：固定伤害可暴击（弹幕伤害前置）
    public int hitDebuff;          // 增益态命中施加的减益 BuffID（0=无）
    public float speedBuff;        // 常驻射速加成
    public float speedBuffBoosted; // 增益态额外射速加成（仅狱岩+10%）
    public int critBonus;          // 暴击率加成
    public bool titanAcc;          // 泰坦改件
    public bool dreadQuiver;       // 恐惧箭袋

    private float _comp;           // 泄热补偿累加器

    public override void ResetEffects()
    {
        gunfireAcc = false;
        heatGain = 0;
        heatConsume = 0;
        heatCap = 0;
        // 下面这里不应该持续设置false,不然会无法做到实际生效
        // boosted = false;
        flatDamage = 0f;
        flatCrits = false;
        hitDebuff = 0;
        speedBuff = 0f;
        speedBuffBoosted = 0f;
        critBonus = 0;
        titanAcc = false;
        dreadQuiver = false;
        // _comp 是跨帧累加器，不能在这里清零（否则补偿永远整不过去）
        // 失活/能量归零时在 PostUpdate 里自行复位
    }

    public override void PostUpdate()
    {
        // ponytail: 抵消 Thorium 被动泄热，热量只由攻击增减（蹭它的热条 UI）
        if (!gunfireAcc)
        {
            _comp = 0f;
            return;
        }
        var tp = Player.GetThoriumPlayer();
        if (tp.hellfireEnergy <= 0)
        {
            _comp = 0f;
            return;
        }
        _comp += ThoriumDrainPerSec / 60f;
        int whole = (int)_comp;
        if (whole > 0)
        {
            _comp -= whole;
            tp.hellfireEnergy += whole;
        }
    }
}
