using Terraria.ModLoader;
using ThoriumMod.Utilities;

namespace ThoriumAccessoryExpansion.Accessories.Ranged.HellfireGunAcc;

public class GunFirePlayer : ModPlayer
{
    private const float ThoriumDrainPerSec = 60f / 11f;

    public bool gunfireAcc;
    public int heatGainPerShot;
    public float cooldownRate;
    public float overloadBonus;
    public int extraDamage;
    public bool extraDamageCanCrit;
    public bool titanAcc;
    public bool dreadQuiver;

    private float _comp;
    private int _lastHellfireEnergy;

    public override void ResetEffects()
    {
        gunfireAcc = false;
        heatGainPerShot = 0;
        cooldownRate = 0f;
        overloadBonus = 0f;
        extraDamage = 0;
        extraDamageCanCrit = false;
        titanAcc = false;
        dreadQuiver = false;
        _comp = 0f;
    }

    public override void PostUpdate()
    {
        var tp = Player.GetThoriumPlayer();

        if (gunfireAcc && !tp.hellfireEnergyOverload && tp.hellfireEnergy > 0)
        {
            if (tp.hellfireEnergy < _lastHellfireEnergy)
            {
                tp.hellfireEnergy = _lastHellfireEnergy;
            }
        }

        if (!gunfireAcc || cooldownRate <= 0f)
        {
            _comp = 0f;
            _lastHellfireEnergy = tp.hellfireEnergy;
            return;
        }
        if (tp.hellfireEnergy <= 0)
        {
            _comp = 0f;
            _lastHellfireEnergy = tp.hellfireEnergy;
            return;
        }
        _comp += (ThoriumDrainPerSec - cooldownRate) / 60f;
        int whole = (int)_comp;
        if (whole > 0)
        {
            _comp -= whole;
            tp.hellfireEnergy += whole;
        }

        _lastHellfireEnergy = tp.hellfireEnergy;
    }
}
