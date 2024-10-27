using System;
using System.Collections.Generic;
using System.IO;
using PDTools.Enums;
using PDTools.Enums.PS3;
using PDTools.Utils;

namespace PDTools.Structures.PS3
{
    public class MGarageCar
    {

        public byte[]? RawData { get; set; }

        public bool CarExists { get; set; }

        public Country Country { get; set; }

        public Drivetrain Drivetrain { get; set; }

        public int Year { get; set; }
        
        public bool Favorites { get; set; }
        
        public Aspiration Aspiration { get; set; }

        public bool RealSpecHidden { get; set; }

        public ulong Unk1 { get; set; }

        public PARTS_TIRE FrontTire { get; set; }
        
        public PARTS_TIRE RearTire { get; set; }

        public bool CanHaveDirtTires { get; set; }

        public bool CanHaveSnowTires { get; set; }
        
        public Tuner Tuner { get; set; }
        
        public ulong Unk2 { get; set; }
        
        public uint Weight { get; set; }
        
        public uint RideCount { get; set; }
        
        public uint Pp1K { get; set; }

        public bool Invalid { get; set; }

        public bool Tuned { get; set; }
        
        public uint Power { get; set; } // Power in PS units
        
        public bool DLC { get; set; }
        
        public bool NOS { get; set; }

        public ulong Unk3 { get; set; }
        
        public uint RideOrder { get; set; }
        
        public uint GarageId { get; set; }
        
        public uint CarCode { get; set; }
        
        public uint MainColor { get; set; } // Some color thing, not sure what
        
        public uint AccentColor { get; set; } // Some color thing, not sure what

        public static MGarageCar Load(Span<byte> data)
        {
            var bs = new BitStream(BitStreamMode.Read, data);

            return Load(ref bs);
        }

        public static MGarageCar Load(ref BitStream bs)
        {
            var garageCar = new MGarageCar();

            garageCar.RawData = new byte[0x28];
            bs.ReadIntoByteArray(garageCar.RawData.Length, garageCar.RawData, BitStream.Byte_Bits);

            var internalBs = new BitStream(BitStreamMode.Read, garageCar.RawData);

            garageCar.CarExists = internalBs.ReadBoolBit();
            // if (!garageCar.CarExists) // 1 bit
            //     return garageCar;

            garageCar.Country = (Country)internalBs.ReadBits(7); // 8 bits
            garageCar.Drivetrain = (Drivetrain)internalBs.ReadBits(3); // 11 bits
            garageCar.Year = 1800 + internalBs.ReadByte(); // 19 bits - 2010 - 1800 = 210 or 1987 - 1800 = 87
            garageCar.Favorites = internalBs.ReadBoolBit(); // 20 bits
            garageCar.Aspiration = (Aspiration)internalBs.ReadBits(3); // 23 bits
            garageCar.RealSpecHidden = internalBs.ReadBoolBit(); // 24 bits
            
            garageCar.Unk1 = internalBs.ReadBits(BitStream.Byte_Bits * 0x3 + 4); // Unknown 52 bits
            
            garageCar.FrontTire = (PARTS_TIRE)internalBs.ReadBits(5);
            garageCar.RearTire = (PARTS_TIRE)internalBs.ReadBits(5);
            garageCar.CanHaveDirtTires = internalBs.ReadBoolBit();
            garageCar.CanHaveSnowTires = internalBs.ReadBoolBit(); // First 8 bytes fully read
            garageCar.Tuner = (Tuner)internalBs.ReadByte(); // 72 bits
            
            garageCar.Unk2 = internalBs.ReadBits(10); // 94 bits
            
            garageCar.Weight = (uint)internalBs.ReadBits(14);
            garageCar.RideCount = internalBs.ReadUInt16();
            garageCar.Pp1K = (uint)internalBs.ReadBits(14);
            garageCar.Invalid = internalBs.ReadBoolBit();
            garageCar.Tuned = internalBs.ReadBoolBit();
            garageCar.Power = (uint)internalBs.ReadBits(14);
            garageCar.DLC = internalBs.ReadBoolBit();
            garageCar.NOS = internalBs.ReadBoolBit();
            
            garageCar.Unk3 = internalBs.ReadBits(16);

            garageCar.RideOrder = internalBs.ReadUInt32();
            garageCar.GarageId = internalBs.ReadUInt32();
            garageCar.CarCode = internalBs.ReadUInt32();
            garageCar.MainColor = internalBs.ReadUInt32();
            garageCar.AccentColor = internalBs.ReadUInt32();

#if DEBUG
            // garageCar.CanHaveDirtTires = true;
            // garageCar.CanHaveSnowTires = true;
#endif

            return garageCar;
        }
        
        public void Serialize(ref BitStream bs)
        {
            bs.WriteBoolBit(CarExists);
            bs.WriteBits((ulong)Country, 7);
            bs.WriteBits((ulong)Drivetrain, 3);
            bs.WriteByte((byte)(Year - 1800));
            bs.WriteBoolBit(Favorites);
            bs.WriteBits((ulong)Aspiration, 3);
            bs.WriteBoolBit(RealSpecHidden);
            
            bs.WriteBits(Unk1, BitStream.Byte_Bits * 0x3 + 4);
            
            bs.WriteBits((ulong)FrontTire, 5);
            bs.WriteBits((ulong)RearTire, 5);
            bs.WriteBoolBit(CanHaveDirtTires);
            bs.WriteBoolBit(CanHaveSnowTires);
            bs.WriteByte((byte)Tuner);
            
            bs.WriteBits(Unk2, 10);
            
            bs.WriteBits(Weight, 14);
            bs.WriteUInt16(RideCount);
            bs.WriteBits(Pp1K, 14);
            bs.WriteBoolBit(Invalid);
            
            bs.WriteBoolBit(Tuned);
            
            bs.WriteBits(Power, 14);
            bs.WriteBoolBit(DLC);
            
            bs.WriteBoolBit(NOS);
            bs.WriteBits(Unk3, 16);
            
            bs.WriteUInt32(RideOrder);
            bs.WriteUInt32(GarageId);
            bs.WriteUInt32(CarCode);
            bs.WriteUInt32(MainColor);
            bs.WriteUInt32(AccentColor);
        }

    }
}