using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Players;
using ThoriumMod.Items.Donate;
using GunModificationPlayerState =
    ThoriumAccessoryExpansion.Players.GunModificationPlayer;

namespace ThoriumAccessoryExpansion.Systems;

public class GunModificationGlobalItem
    : GlobalItem
{
    private const float TitanUseTimeMultiplier = 1.3f;

    private const float TitanSlowGunBonus = 0.75f;

    private const int TitanSlowGunUseTime = 30;


    public override bool AppliesToEntity(
        Item entity,
        bool lateInstantiation)
    {
        return
            entity.useAmmo == AmmoID.Bullet &&
            entity.ModItem is not HellfireMinigun;
    }


    public override bool Shoot(
        Item item,
        Player player,
        EntitySource_ItemUse_WithAmmo source,
        Microsoft.Xna.Framework.Vector2 position,
        Microsoft.Xna.Framework.Vector2 velocity,
        int type,
        int damage,
        float knockback)
    {
        GunModificationPlayerState modification =
            player.GetModPlayer<
                GunModificationPlayerState
            >();


        if (
            modification.HasHeatModification
        )
        {
            if (
                !modification.IsOverloading
            )
            {
                modification.AddHeat(
                    modification.HeatGainPerAttack
                );
            }
            else
            {
                modification.BeginOverloadAttack();
            }
        }

        return base.Shoot(
            item,
            player,
            source,
            position,
            velocity,
            type,
            damage,
            knockback
        );
    }


    public override float UseTimeMultiplier(
        Item item,
        Player player)
    {
        GunModificationPlayerState modification =
            player.GetModPlayer<
                GunModificationPlayerState
            >();

        if (
            modification.HasHellstoneGunMod &&
            modification.IsOverloading
        )
        {
            return 1f / 1.10f;
        }

        if (
            modification.HasTitanGunMod
        )
        {
            return TitanUseTimeMultiplier;
        }


        return 1f;
    }


    public override void ModifyWeaponDamage(
        Item item,
        Player player,
        ref StatModifier damage)
    {
        GunModificationPlayerState modification =
            player.GetModPlayer<
                GunModificationPlayerState
            >();


        if (
            modification.HasTitanGunMod &&
            item.useTime <
                TitanSlowGunUseTime
        )
        {
            damage += TitanSlowGunBonus;
        }
    }
}