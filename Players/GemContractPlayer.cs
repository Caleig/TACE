using Terraria;
using Terraria.ModLoader;

namespace ThoriumAccessoryExpansion.Players
{
    public class GemContractPlayer : ModPlayer
    {
        public bool amethystContract;
        public bool topazContract;
        public bool sapphireContract;
        public bool emeraldContract;
        public bool amberContract;
        public bool rubyContract;
        public bool diamondContract;
        public bool opalContract;
        public bool aquamarineContract;
        public bool crystallineAmethystContract;
        public bool crystallineTopazContract;
        public bool crystallineSapphireContract;
        public bool crystallineEmeraldContract;
        public bool crystallineAmberContract;
        public bool crystallineRubyContract;
        public bool crystallineDiamondContract;
        public bool crystallineOpalContract;
        public bool crystallineAquamarineContract;

        public bool legendaryContract;


        public override void ResetEffects()
        {
            amethystContract = false;
            topazContract = false;
            sapphireContract = false;
            emeraldContract = false;
            amberContract = false;
            rubyContract = false;
            diamondContract = false;
            opalContract = false;
            aquamarineContract = false;
            crystallineAmethystContract = false;
            crystallineTopazContract = false;
            crystallineSapphireContract = false;
            crystallineEmeraldContract = false;
            crystallineAmberContract = false;
            crystallineRubyContract = false;
            crystallineDiamondContract = false;
            crystallineOpalContract = false;
            crystallineAquamarineContract = false;
            legendaryContract = false;
            magicContractActive = false;
        }
        public bool HasAnyContract()
        {
            return
                amethystContract ||
                topazContract ||
                sapphireContract ||
                emeraldContract ||
                amberContract ||
                rubyContract ||
                diamondContract ||
                opalContract ||
                aquamarineContract ||

                crystallineAmethystContract ||
                crystallineTopazContract ||
                crystallineSapphireContract ||
                crystallineEmeraldContract ||
                crystallineAmberContract ||
                crystallineRubyContract ||
                crystallineDiamondContract ||
                crystallineOpalContract ||
                crystallineAquamarineContract ||

                legendaryContract;
        }
        public bool magicContractActive;
    }
}