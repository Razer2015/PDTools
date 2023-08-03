﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using System.ComponentModel;

using PDTools.Utils;
using PDTools.Enums;

namespace PDTools.Structures.MGameParameter
{
    public class Reward
    {
        /// <summary>
        /// Credit prizes for each rank.
        /// </summary>
        public List<int> PrizeTable { get; set; } = new List<int>();

        /// <summary>
        /// Point/XP prizes for each rank (GT5).
        /// </summary>
        public List<int> PointTable { get; set; } = new List<int>();

        /// <summary>
        /// Star tables requirements (GT6).
        /// </summary>
        public List<FinishResult> StarTable { get; set; } = new List<FinishResult>();

        /// <summary>
        /// Present rewards.
        /// </summary>
        public List<EventPresent> Present { get; set; } = new List<EventPresent>();

        /// <summary>
        /// Defaults to 0.
        /// </summary>
        public int SpecialRewardCode { get; set; } = 0;

        /// <summary>
        /// Whether prizes are cummulative - i.e finishing 1st will give all rewards across all positions, otherwise if false, just 1st rewards. Defaults to false.
        /// </summary>
        public bool PrizeType { get; set; } = false;

        /// <summary>
        /// Defaults to 0.
        /// </summary>
        public short PPBase { get; set; } = 0;

        /// <summary>
        /// Defaults to 0.
        /// </summary>
        public short PercentAtPP100 { get; set; } = 0;

        /// <summary>
        /// Rewards can only be obtained once. Defaults to false.
        /// </summary>
        public bool IsOnce { get; set; } = false;

        /// <summary>
        /// How presents should be given. Defaults to <see cref="RewardPresentType.ORDER"/>
        /// </summary>
        public RewardPresentType PresentType { get; set; } = RewardPresentType.ORDER;

        /// <summary>
        /// GT6 Only. Participation/Entry present rewards.
        /// </summary>
        public List<EventPresent> EntryPresent { get; set; } = new List<EventPresent>();

        /// <summary>
        /// GT6 Only. How presents should be given (participation/entry). Defaults to <see cref="RewardEntryPresentType.FINISH"/>.
        /// </summary>
        public RewardEntryPresentType EntryPresentType { get; set; } = RewardEntryPresentType.FINISH;

        /// <summary>
        /// Used for custom car presents.
        /// </summary>
        public EntryBase TunedEntryPresent { get; set; }

        public void SetRewardPresent(int index, EventPresent present)
            => Present[index] = present;

        public void SetParticipatePresent(int index, EventPresent present)
            => EntryPresent[index] = present;

        public bool IsDefault()
        {
            var defaultReward = new Reward();
            return PrizeTable.Count == 0 &&
                PointTable.Count == 0 &&
                StarTable.Count == 0 &&
                Present.Count == 0 &&
                SpecialRewardCode == defaultReward.SpecialRewardCode &&
                PrizeType == defaultReward.PrizeType &&
                PPBase == defaultReward.PPBase &&
                PercentAtPP100 == defaultReward.PercentAtPP100 &&
                IsOnce == defaultReward.IsOnce &&
                PresentType == defaultReward.PresentType &&
                EntryPresent.Count == 0 &&
                EntryPresentType == defaultReward.EntryPresentType;
                // TODO TunedEntryPresent
        }

        public void WriteToXml(XmlWriter xml)
        {
            xml.WriteStartElement("point_table");
            for (int i = 0; i < PointTable.Count; i++)
                xml.WriteElementInt("point", PointTable[i]);
            xml.WriteEndElement();
            
            xml.WriteStartElement("prize_table");
            for (int i = 0; i < PrizeTable.Count; i++)
               xml.WriteElementInt("prize", PrizeTable[i]);
            xml.WriteEndElement();
            
            xml.WriteStartElement("star_table");
            for (int i = 0; i < StarTable.Count; i++)
                xml.WriteElementValue("star", StarTable[i].ToString());
            xml.WriteEndElement();

            if (Present.Count > 0)
            {
                xml.WriteStartElement("present");
                foreach (var present in Present)
                    present.WriteToXml(xml);
                xml.WriteEndElement();
            }
            
            xml.WriteElementInt("special_reward_code", SpecialRewardCode);
            xml.WriteElementBool("prize_type", PrizeType);
            xml.WriteElementInt("pp_base", PPBase);
            xml.WriteElementInt("percent_at_pp100", PercentAtPP100);
            xml.WriteElementBool("is_once", IsOnce);
            xml.WriteElementValue("present_type", PresentType.ToString());

            if (Present.Count > 0)
            {
                xml.WriteStartElement("entry_present");
                foreach (var present in Present)
                    present.WriteToXml(xml);
                xml.WriteEndElement();
            }
            
            xml.WriteElementValue("entry_present_type", EntryPresentType.ToString());
            
            TunedEntryPresent?.WriteToXml(xml);
        }

        public void ParseFromXml( XmlNode node)
        {
            foreach (XmlNode rewardNode in node.ChildNodes)
            {
                switch (rewardNode.Name)
                {
                    case "is_once":
                        IsOnce = rewardNode.ReadValueBool(); break;
                    case "percent_at_pp100":
                        PercentAtPP100 = rewardNode.ReadValueShort(); break;
                    case "pp_base":
                        PPBase = rewardNode.ReadValueShort(); break;

                    case "present_type":
                        PresentType = rewardNode.ReadValueEnum<RewardPresentType>(); break;

                    case "present":
                        foreach (XmlNode presentNode in node.SelectNodes("item"))
                        {
                            var present = new EventPresent();
                            present.ParseFromXml(presentNode);
                            Present.Add(present);
                        }
                        break;

                    case "entry_present_type":
                        EntryPresentType = rewardNode.ReadValueEnum<RewardEntryPresentType>(); break;

                    case "entry_present":
                        foreach (XmlNode presentNode in node.SelectNodes("item"))
                        {
                            var present = new EventPresent();
                            present.ParseFromXml(presentNode);
                            EntryPresent.Add(present);
                        }
                        break;

                    case "prize_type":
                        PrizeType = rewardNode.ReadValueBool(); break;

                    case "point_table":
                        foreach (XmlNode pointNode in rewardNode.SelectNodes("point"))
                            PointTable.Add(pointNode.ReadValueInt());
                        break;

                    case "prize_table":
                        int i = 0;
                        foreach (XmlNode prizeNode in rewardNode.SelectNodes("prize"))
                            PrizeTable.Add(prizeNode.ReadValueInt());
                        break;

                    case "star_table":
                        foreach (XmlNode starNode in rewardNode.SelectNodes("star"))
                            StarTable.Add(starNode.ReadValueEnum<FinishResult>());
                        break;

                    case "entry_base":
                        TunedEntryPresent = new EntryBase();
                        TunedEntryPresent.ReadFromXml(rewardNode);
                        break;
                }
            }
        }

        public void Serialize(ref BitStream bs)
        {
            bs.WriteUInt32(0xE6_E6_A1_07);
            bs.WriteUInt32(1_03);

            bs.WriteInt32(PrizeTable.Count);
            for (int i = 0; i < PrizeTable.Count; i++)
                bs.WriteInt32(PrizeTable[i]);

            bs.WriteInt32(PointTable.Count);
            for (int i = 0; i < PointTable.Count; i++)
                bs.WriteInt32(PointTable[i]);

            bs.WriteInt32(StarTable.Count);
            for (int i = 0; i < StarTable.Count; i++)
                bs.WriteInt32((int)StarTable[i]);

            bs.WriteInt32(Present.Count);
            foreach (var present in Present)
                present.Serialize(ref bs);

            bs.WriteInt32(SpecialRewardCode);
            bs.WriteBool2(PrizeType);
            bs.WriteInt16(PPBase);
            bs.WriteInt16(PercentAtPP100);
            bs.WriteBool(IsOnce);
            bs.WriteBool(false); // unk field_0x4b

            bs.WriteInt32(EntryPresent.Count);
            foreach (var present in EntryPresent)
                present.Serialize(ref bs);

            bs.WriteByte((byte)EntryPresentType);

            EntryBase entryBaseReward = TunedEntryPresent ?? new EntryBase();
            entryBaseReward.Serialize(ref bs);
        }
    }

    public class EventPresent
    {
        public GameItemType TypeID { get; set; }
        public GameItemCategory CategoryID { get; set; }
        public int Argument1 { get; set; } = 0;
        public int Argument2 { get; set; } = 0;
        public int Argument3 { get; set; } = 0;
        public int Argument4 { get; set; } = -1;

        // Only parsed as a blob in Seasonals Root
        public string FName { get; set; }

        public static EventPresent FromCar(string carLabel)
        {
            var present = new EventPresent();
            present.FName = carLabel;
            return present;
        }

        public static EventPresent FromCarParameter(MCarParameter carParameter)
        {
            throw new NotImplementedException("Implement this when MCarParameter serialization is completed");

            /*
            var present = new EventPresent();
            present.TypeID = GameItemType.SPECIAL;
            var data = carParameter.ExportToBlob();
            present.FName = Convert.ToBase64String(MiscUtils.ZlibCompress(data));
            return present;
            */
        }

        public static EventPresent FromPaint(int paintID)
        {
            var present = new EventPresent();
            present.TypeID = GameItemType.DRIVER_ITEM;
            present.Argument1 = paintID;
            return present;
        }

        public static EventPresent FromSuit(int suitID)
        {
            var present = new EventPresent();
            present.TypeID = GameItemType.NONE;
            present.Argument1 = 0;
            present.Argument4 = suitID;
            return present;
        }

        public void Serialize(ref BitStream bs)
        {
            bs.WriteUInt32(0x_E6_E6_D2_B3);
            bs.WriteUInt32(1_00);

            // type_id
            bs.WriteUInt32((uint)TypeID);
            bs.WriteInt32((int)CategoryID);
            bs.WriteInt32(Argument1);
            bs.WriteInt32(Argument2);
            bs.WriteInt32(Argument3);
            bs.WriteInt32(Argument4);
            bs.WriteNullStringAligned4(FName);

            // Blob Size
            bs.WriteInt32(0);
        }

        public void WriteToXml(XmlWriter xml)
        {
            xml.WriteStartElement("item");
            {
                xml.WriteAttributeString("type_id", TypeID.ToString());
                xml.WriteAttributeString("category_id", CategoryID.ToString());
                xml.WriteAttributeString("argument1", Argument1.ToString());
                xml.WriteAttributeString("argument2", Argument2.ToString());
                xml.WriteAttributeString("argument3", Argument3.ToString());
                xml.WriteAttributeString("argument4", Argument4.ToString());
                xml.WriteAttributeString("f_name", FName);
            }
            xml.WriteEndElement();
        }

        public void ParseFromXml(XmlNode node)
        {
            foreach (XmlAttribute attr in node.Attributes)
            {
                switch (attr.Name)
                {
                    case "type_id":
                        if (int.TryParse(attr.Value, out int type_id))
                            TypeID = (GameItemType)type_id;
                        break;

                    case "category_id":
                        if (int.TryParse(attr.Value, out int category_id))
                            CategoryID = (GameItemCategory)category_id;
                        break;

                    case "argument1":
                        if (int.TryParse(attr.Value, out int value))
                            Argument1 = value;
                        break;
                    case "argument2":
                        if (int.TryParse(attr.Value, out int value2))
                            Argument2 = value2;
                        break;
                    case "argument3":
                        if (int.TryParse(attr.Value, out int value3))
                            Argument3 = value3;
                        break;
                    case "argument4":
                        if (int.TryParse(attr.Value, out int value4))
                            Argument4 = value4;
                        break;

                    case "f_name":
                        FName = attr.Value;
                        break;

                }
            }
        }
    }


    public enum LicenseResultType
    {
        EMPTY,
        FAILURE,
        CLEAR,
        BRONZE,
        SILVER,
        GOLD
    }

    public enum LicenseDisplayModeType
    {
        NONE,
        PYLON_NUM,
        FUEL_DIST,
        FUEL_TIME,
        DRIFT_SCORE,

    }

    public enum LicenseConnectionType
    {
        OR,
        AND,
        XOR
    }

    public enum LicenseConditionType
    {
        EQUAL,
        NOTEQUAL,
        GREATER,
        LESS,
        GREATER_EQUAL,
        LESS_EQUAL,
    }

    public enum LicenseCheckType
    {
        RANK,
        OTHER_SUBMODE,
        TOTAL_TIME,
        LAP_TIME,
        BEST_LAP_TIME,
        LAP_COUNT,
        VELOCITY,
        V_POSITION,
        GADGET_COUNT,
        COURSE_OUT,
        HIT_COUNT,
        HIT_POWER,
        HIT_WALL,
        FUEL_AMOUNT,
        COMPLETE_FLAG,
        WRONG_WAY_COUNT,
        ROAD_DISTANCE,
        STANDING_TIME,
        COURSE_OUT_TIME,
        FUEL_CONSUMPTION,
        FLOATING_TIME,
        ILLEGAL,
    }

    [Flags]
    public enum FailCondition
    {
        NONE,
        COURSE_OUT,
        HIT_WALL_HARD,
        HIT_CAR_HARD,
        HIT_CAR,
        PYLON,
        HIT_WALL,
        SPIN_FULL,
        SPIN_HALF,
        WHEEL_SPIN,
        LOCK_BRAKE,
        SLIP_ANGLE,
        LESS_SPEED,
        MORE_SPEED,
        MORE_GFORCE,
        PENALTY_ROAD,
        LOW_MU_ROAD,
        SLALOM,
        WRONGWAY,
        WRONGWAY_LOOSE,
        MAX,
    }
}
