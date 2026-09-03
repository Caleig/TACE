using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace ThoriumAccessoryExpansion.Systems.Projectiles
{
    public class MagicContractGlobalProjectile : GlobalProjectile
    {
        public bool magicConverted;
        public bool gemProjectile;

        public override bool InstancePerEntity => true;

        public override void OnSpawn(
            Projectile projectile,
            IEntitySource source)
        {
            Player player = Main.player[projectile.owner];

            if (!player.active)
                return;
            if (source is EntitySource_Parent parent)
            {
                if (parent.Entity is Projectile parentProjectile)
                {
                    MagicContractGlobalProjectile parentData =
                        parentProjectile.GetGlobalProjectile<MagicContractGlobalProjectile>();

                    if (parentData.magicConverted)
                    {
                        magicConverted = true;
                        projectile.DamageType = DamageClass.Summon;
                        return;
                    }
                }
            }
            if (source is EntitySource_ItemUse_WithAmmo itemSource)
            {
                if (MagicContractGlobalItem.IsMagicContractWeapon(
                        itemSource.Item,
                        player))
                {
                    magicConverted = true;
                    projectile.DamageType = DamageClass.Summon;
                    return;
                }
            }
            if (!MagicContractGlobalItem.IsMagicContractWeapon(
                    player.HeldItem,
                    player))
                return;

            if (projectile.DamageType != DamageClass.Magic)
                return;

            magicConverted = true;
            projectile.DamageType = DamageClass.Summon;
        }

        public override void AI(
            Projectile projectile)
        {
            if (magicConverted)
            {
                projectile.DamageType = DamageClass.Summon;
            }
        }
    }
}