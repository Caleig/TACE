using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumAccessoryExpansion.Buffs;
using ThoriumAccessoryExpansion.NPCs;


namespace ThoriumAccessoryExpansion.Players;


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

    public bool magicContractActive;



    private const int MaxLegendaryResonance = 9;

    private const int LegendaryResonanceDuration = 480;



    public const byte LegendaryResonanceSyncPacket = 201;



    private readonly GemType[] legendaryResonanceTypes =
        new GemType[MaxLegendaryResonance];


    private readonly int[] legendaryResonanceTimers =
        new int[MaxLegendaryResonance];



    private int legendaryResonanceCount;



    public int LegendaryResonanceCount =>
        legendaryResonanceCount;


    public bool HasLegendaryResonance =>
        legendaryResonanceCount > 0;



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



    public override void PreUpdate()
    {

        TickLegendaryResonance();

    }



    public override void PreUpdateBuffs()
    {

        if (!HasLegendaryResonance)
            return;



        int buffType =
            ModContent.BuffType<
                LegendaryResonanceBuff
            >();



        int buffTime =
            GetLongestLegendaryResonanceTime();



        int buffIndex =
            Player.FindBuffIndex(
                buffType
            );



        if (buffIndex < 0)
        {

            Player.AddBuff(
                buffType,
                buffTime
            );

        }
        else
        {

            Player.buffTime[buffIndex] =
                buffTime;

        }

    }



    private void TickLegendaryResonance()
    {

        if (legendaryResonanceCount <= 0)
            return;



        bool changed = false;



        for (
            int i = 0;
            i < legendaryResonanceCount;
            i++
        )
        {

            legendaryResonanceTimers[i]--;



            if (
                legendaryResonanceTimers[i]
                <= 0
            )
            {

                RemoveLegendaryResonanceAt(
                    i
                );



                i--;



                changed = true;

            }

        }



        if (
            changed
            &&
            Main.netMode ==
                NetmodeID.Server
        )
        {

            SendLegendaryResonancePacket(
                -1,
                Player.whoAmI
            );

        }

    }



    private void RemoveLegendaryResonanceAt(
        int index)
    {

        for (
            int i = index;
            i < legendaryResonanceCount - 1;
            i++
        )
        {

            legendaryResonanceTypes[i] =
                legendaryResonanceTypes[i + 1];


            legendaryResonanceTimers[i] =
                legendaryResonanceTimers[i + 1];

        }



        legendaryResonanceCount--;



        if (legendaryResonanceCount >= 0)
        {

            legendaryResonanceTypes[
                legendaryResonanceCount
            ] = GemType.None;


            legendaryResonanceTimers[
                legendaryResonanceCount
            ] = 0;

        }

    }



    public void AddLegendaryResonance(
        GemType type)
    {

        if (type == GemType.Prismatic)
        {

            type =
                GetRandomResonanceType();

        }



        if (type == GemType.None)
            return;



        if (
            legendaryResonanceCount
            < MaxLegendaryResonance
        )
        {

            int index =
                legendaryResonanceCount;



            legendaryResonanceTypes[index] =
                type;



            legendaryResonanceTimers[index] =
                LegendaryResonanceDuration;



            legendaryResonanceCount++;

        }
        else
        {

            for (
                int i = 0;
                i < MaxLegendaryResonance - 1;
                i++
            )
            {

                legendaryResonanceTypes[i] =
                    legendaryResonanceTypes[i + 1];


                legendaryResonanceTimers[i] =
                    legendaryResonanceTimers[i + 1];

            }



            int index =
                MaxLegendaryResonance - 1;



            legendaryResonanceTypes[index] =
                type;


            legendaryResonanceTimers[index] =
                LegendaryResonanceDuration;

        }



        Player.AddBuff(
            ModContent.BuffType<
                LegendaryResonanceBuff
            >(),
            GetLongestLegendaryResonanceTime()
        );



        if (
            Main.netMode ==
            NetmodeID.Server
        )
        {

            SendLegendaryResonancePacket(
                -1,
                Player.whoAmI
            );

        }

    }



    private GemType GetRandomResonanceType()
    {

        return Main.rand.Next(9) switch
        {

            0 => GemType.Amethyst,

            1 => GemType.Topaz,

            2 => GemType.Sapphire,

            3 => GemType.Emerald,

            4 => GemType.Amber,

            5 => GemType.Ruby,

            6 => GemType.Diamond,

            7 => GemType.Opal,

            _ => GemType.Aquamarine

        };

    }



    public int GetLegendaryResonanceStacks(
        GemType type)
    {

        int count = 0;



        for (
            int i = 0;
            i < legendaryResonanceCount;
            i++
        )
        {

            if (
                legendaryResonanceTypes[i]
                == type
            )
            {

                count++;

            }

        }



        return count;

    }



    public int GetLongestLegendaryResonanceTime()
    {

        int result = 0;



        for (
            int i = 0;
            i < legendaryResonanceCount;
            i++
        )
        {

            result =
                Math.Max(
                    result,
                    legendaryResonanceTimers[i]
                );

        }



        return result;

    }



    public override void ModifyMaxStats(
        out StatModifier health,
        out StatModifier mana)
    {

        base.ModifyMaxStats(
            out health,
            out mana
        );



        int amber =
            GetLegendaryResonanceStacks(
                GemType.Amber
            );



        if (amber > 0)
        {

            health +=
                amber * 0.02f;

        }

    }



    public override void PostUpdateEquips()
    {

        base.PostUpdateEquips();



        int amethyst =
            GetLegendaryResonanceStacks(
                GemType.Amethyst
            );


        int topaz =
            GetLegendaryResonanceStacks(
                GemType.Topaz
            );


        int emerald =
            GetLegendaryResonanceStacks(
                GemType.Emerald
            );


        int ruby =
            GetLegendaryResonanceStacks(
                GemType.Ruby
            );


        int opal =
            GetLegendaryResonanceStacks(
                GemType.Opal
            );



        if (amethyst > 0)
        {

            Player.GetDamage(
                DamageClass.Generic
            ) +=
                amethyst * 0.02f;

        }



        if (topaz > 0)
        {

            Player.GetCritChance(
                DamageClass.Generic
            ) +=
                topaz;

        }



        if (emerald > 0)
        {

            Player.GetArmorPenetration(
                DamageClass.Generic
            ) +=
                emerald;

        }



        if (ruby > 0)
        {

            Player.endurance =
                Math.Min(
                    1f,
                    Player.endurance
                    + ruby * 0.01f
                );

        }

    }



    public override void PostUpdateMiscEffects()
    {

        base.PostUpdateMiscEffects();



        int sapphire =
            GetLegendaryResonanceStacks(
                GemType.Sapphire
            );



        if (sapphire > 0)
        {

            Player.moveSpeed +=
                sapphire * 0.02f;

        }

    }



    public override void UpdateLifeRegen()
    {

        base.UpdateLifeRegen();



        int diamond =
            GetLegendaryResonanceStacks(
                GemType.Diamond
            );



        if (diamond > 0)
        {

            Player.lifeRegen +=
                diamond * 2;

        }

    }



    public override void ModifyHurt(
        ref Player.HurtModifiers modifiers)
    {

        int opal =
            GetLegendaryResonanceStacks(
                GemType.Opal
            );



        if (opal <= 0)
            return;



        modifiers.Knockback *=
            Math.Max(
                0f,
                1f - opal * 0.05f
            );

    }



    public override void UpdateDead()
    {

        base.UpdateDead();



        legendaryResonanceCount = 0;



        for (
            int i = 0;
            i < MaxLegendaryResonance;
            i++
        )
        {

            legendaryResonanceTypes[i] =
                GemType.None;


            legendaryResonanceTimers[i] =
                0;

        }

    }



    public string GetLegendaryResonanceTooltip()
    {

        if (!HasLegendaryResonance)
            return "";



        string result =
            Language.GetTextValue(
                "Mods.ThoriumAccessoryExpansion.Buffs.LegendaryResonanceBuff.Stacks",
                legendaryResonanceCount
            );



        int amethyst =
            GetLegendaryResonanceStacks(
                GemType.Amethyst
            );


        int topaz =
            GetLegendaryResonanceStacks(
                GemType.Topaz
            );


        int sapphire =
            GetLegendaryResonanceStacks(
                GemType.Sapphire
            );


        int emerald =
            GetLegendaryResonanceStacks(
                GemType.Emerald
            );


        int amber =
            GetLegendaryResonanceStacks(
                GemType.Amber
            );


        int ruby =
            GetLegendaryResonanceStacks(
                GemType.Ruby
            );


        int diamond =
            GetLegendaryResonanceStacks(
                GemType.Diamond
            );


        int opal =
            GetLegendaryResonanceStacks(
                GemType.Opal
            );


        int aquamarine =
            GetLegendaryResonanceStacks(
                GemType.Aquamarine
            );



        if (amethyst > 0)
        {

            result +=
                "\n" +
                Language.GetTextValue(
                    "Mods.ThoriumAccessoryExpansion.Buffs.LegendaryResonanceBuff.Amethyst",
                    amethyst,
                    amethyst * 2
                );

        }



        if (topaz > 0)
        {

            result +=
                "\n" +
                Language.GetTextValue(
                    "Mods.ThoriumAccessoryExpansion.Buffs.LegendaryResonanceBuff.Topaz",
                    topaz,
                    topaz
                );

        }



        if (sapphire > 0)
        {

            result +=
                "\n" +
                Language.GetTextValue(
                    "Mods.ThoriumAccessoryExpansion.Buffs.LegendaryResonanceBuff.Sapphire",
                    sapphire,
                    sapphire * 2
                );

        }



        if (emerald > 0)
        {

            result +=
                "\n" +
                Language.GetTextValue(
                    "Mods.ThoriumAccessoryExpansion.Buffs.LegendaryResonanceBuff.Emerald",
                    emerald,
                    emerald
                );

        }



        if (amber > 0)
        {

            result +=
                "\n" +
                Language.GetTextValue(
                    "Mods.ThoriumAccessoryExpansion.Buffs.LegendaryResonanceBuff.Amber",
                    amber,
                    amber * 2
                );

        }



        if (ruby > 0)
        {

            result +=
                "\n" +
                Language.GetTextValue(
                    "Mods.ThoriumAccessoryExpansion.Buffs.LegendaryResonanceBuff.Ruby",
                    ruby,
                    ruby
                );

        }



        if (diamond > 0)
        {

            result +=
                "\n" +
                Language.GetTextValue(
                    "Mods.ThoriumAccessoryExpansion.Buffs.LegendaryResonanceBuff.Diamond",
                    diamond
                );

        }



        if (opal > 0)
        {

            result +=
                "\n" +
                Language.GetTextValue(
                    "Mods.ThoriumAccessoryExpansion.Buffs.LegendaryResonanceBuff.Opal",
                    opal,
                    opal * 5
                );

        }



        if (aquamarine > 0)
        {

            result +=
                "\n" +
                Language.GetTextValue(
                    "Mods.ThoriumAccessoryExpansion.Buffs.LegendaryResonanceBuff.Aquamarine",
                    aquamarine,
                    aquamarine * 5
                );

        }



        return result;

    }


    public override void SyncPlayer(
        int toWho,
        int fromWho,
        bool newPlayer)
    {

        if (
            Main.netMode ==
            NetmodeID.MultiplayerClient
        )
            return;



        SendLegendaryResonancePacket(
            toWho,
            fromWho
        );

    }



    private void SendLegendaryResonancePacket(
        int toWho,
        int fromWho)
    {

        ModPacket packet =
            Mod.GetPacket();



        packet.Write(
            LegendaryResonanceSyncPacket
        );


        packet.Write(
            (byte)Player.whoAmI
        );


        packet.Write(
            (byte)legendaryResonanceCount
        );



        for (
            int i = 0;
            i < MaxLegendaryResonance;
            i++
        )
        {

            packet.Write(
                (byte)legendaryResonanceTypes[i]
            );


            packet.Write(
                (short)legendaryResonanceTimers[i]
            );

        }



        packet.Send(
            toWho,
            fromWho
        );

    }



    public void ReceiveLegendaryResonance(
        BinaryReader reader)
    {

        legendaryResonanceCount =
            Math.Clamp(
                (int)reader.ReadByte(),
                0,
                MaxLegendaryResonance
            );



        for (
            int i = 0;
            i < MaxLegendaryResonance;
            i++
        )
        {

            legendaryResonanceTypes[i] =
                (GemType)reader.ReadByte();


            legendaryResonanceTimers[i] =
                reader.ReadInt16();

        }

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
    public override void PostUpdateBuffs()
    {

        base.PostUpdateBuffs();



        if (!HasLegendaryResonance)
            return;



        int buffType =
            ModContent.BuffType<
                LegendaryResonanceBuff
            >();



        int buffIndex =
            Player.FindBuffIndex(
                buffType
            );



        if (buffIndex < 0)
            return;



        int lastBuffIndex = -1;



        for (
            int i = 0;
            i < Player.buffType.Length;
            i++
        )
        {

            if (
                Player.buffTime[i] > 0
            )
            {

                lastBuffIndex = i;

            }

        }



        if (
            lastBuffIndex < 0
            ||
            buffIndex == lastBuffIndex
        )
            return;



        int savedType =
            Player.buffType[buffIndex];


        int savedTime =
            Player.buffTime[buffIndex];



        if (buffIndex < lastBuffIndex)
        {

            for (
                int i = buffIndex;
                i < lastBuffIndex;
                i++
            )
            {

                Player.buffType[i] =
                    Player.buffType[i + 1];


                Player.buffTime[i] =
                    Player.buffTime[i + 1];

            }

        }



        Player.buffType[lastBuffIndex] =
            savedType;


        Player.buffTime[lastBuffIndex] =
            savedTime;

    }
}