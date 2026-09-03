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
                    target,
                    GemType.Amethyst,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineAmethystContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.CrystallineAmethyst,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.topazContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.Topaz,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineTopazContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.CrystallineTopaz,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.sapphireContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.Sapphire,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineSapphireContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.CrystallineSapphire,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.emeraldContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.Emerald,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineEmeraldContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.CrystallineEmerald,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.amberContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.Amber,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineAmberContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.CrystallineAmber,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.rubyContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.Ruby,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineRubyContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.CrystallineRuby,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.diamondContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.Diamond,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineDiamondContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.CrystallineDiamond,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.opalContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.Opal,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineOpalContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.CrystallineOpal,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.aquamarineContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.Aquamarine,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.crystallineAquamarineContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.CrystallineAquamarine,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }

            if (contract.legendaryContract)
            {
                mark.AddGemMark(
                    target,
                    GemType.Prismatic,
                    300,
                    projectile.damage,
                    player.whoAmI
                );
            }
        }
    }
}