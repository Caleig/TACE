using Terraria.ModLoader;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Accessories.HellfireGunAcc;

/// <summary>
/// 枪械发热系统状态：当前装备的发热件参数 + 冷却速度覆写
/// </summary>
public class GunFirePlayer : ModPlayer
{
    // Thorium 原版泄热：能量>0 时每 10 tick -1，约每秒 60/11 ≈ 5.4545
    private const float ThoriumDrainPerSec = 60f / 11f;

    public bool gunfireAcc;        // 发热类枪械改件已装备
    public int heatGainPerShot;    // 每枪蓄热量
    public float cooldownRate;     // 期望冷却（每秒泄热），0 = 用 Thorium 原速（匀速）
    public float overloadBonus;    // 过热冷却期间每枪伤害加成（0..1）
    public bool titanAcc;          // 泰坦改件
    public bool dreadQuiver;       // 恐惧箭袋

    private float _comp;           // 冷却补偿浮点累加器

    public override void ResetEffects()
    {
        gunfireAcc = false;
        heatGainPerShot = 0;
        cooldownRate = 0f;
        overloadBonus = 0f;
        titanAcc = false;
        dreadQuiver = false;
        _comp = 0f;
    }

    public override void PostUpdate()
    {
        // ponytail: 覆写冷却 = 把 Thorium 原速泄热与目标速率的差值补回去
        // 只支持比原速慢（需求全是"缓慢/极慢"）；浮点累加避免整数截断漂移
        if (!gunfireAcc || cooldownRate <= 0f)
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
        _comp += (ThoriumDrainPerSec - cooldownRate) / 60f;
        int whole = (int)_comp;
        if (whole > 0)
        {
            _comp -= whole;
            tp.hellfireEnergy += whole;
        }
    }
}
