using Terraria;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumAccessoryExpansion.NPCs;
using ThoriumAccessoryExpansion.Systems.Projectiles;

namespace ThoriumAccessoryExpansion.Systems
{
    public class GemContractProjectile : GlobalProjectile
    {
        public override void OnHitNPC(
            Projectile projectile,
            NPC target,
            NPC.HitInfo hit,
            int damageDone)
        {

            MagicContractGlobalProjectile magic =
                projectile.GetGlobalProjectile<MagicContractGlobalProjectile>();


            if (magic.gemProjectile)
                return;

            if (!magic.magicConverted)
                return;


            Player player =
                Main.player[projectile.owner];


            GemContractPlayer contract =
                player.GetModPlayer<GemContractPlayer>();


            if (!contract.HasAnyContract())
                return;


            GemMarkGlobalNPC mark =
                target.GetGlobalNPC<GemMarkGlobalNPC>();


            if (contract.amethystContract)
            {
                mark.AddGemMark(
                    GemType.Amethyst,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineAmethystContract)
            {
                mark.AddGemMark(
                    GemType.CrystallineAmethyst,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.topazContract)
            {
                mark.AddGemMark(
                    GemType.Topaz,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineTopazContract)
            {
                mark.AddGemMark(
                    GemType.CrystallineTopaz,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.sapphireContract)
            {
                mark.AddGemMark(
                    GemType.Sapphire,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineSapphireContract)
            {
                mark.AddGemMark(
                    GemType.CrystallineSapphire,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.emeraldContract)
            {
                mark.AddGemMark(
                    GemType.Emerald,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineEmeraldContract)
            {
                mark.AddGemMark(
                    GemType.CrystallineEmerald,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.amberContract)
            {
                mark.AddGemMark(
                    GemType.Amber,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineAmberContract)
            {
                mark.AddGemMark(
                    GemType.CrystallineAmber,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.rubyContract)
            {
                mark.AddGemMark(
                    GemType.Ruby,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineRubyContract)
            {
                mark.AddGemMark(
                    GemType.CrystallineRuby,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.diamondContract)
            {
                mark.AddGemMark(
                    GemType.Diamond,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineDiamondContract)
            {
                mark.AddGemMark(
                    GemType.CrystallineDiamond,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.opalContract)
            {
                mark.AddGemMark(
                    GemType.Opal,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineOpalContract)
            {
                mark.AddGemMark(
                    GemType.CrystallineOpal,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.aquamarineContract)
            {
                mark.AddGemMark(
                    GemType.Aquamarine,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineAquamarineContract)
            {
                mark.AddGemMark(
                    GemType.CrystallineAquamarine,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.legendaryContract)
            {
                mark.AddGemMark(
                    GemType.Prismatic,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }
        }
    }
}