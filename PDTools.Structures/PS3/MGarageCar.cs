using System;
using PDTools.Enums;
using PDTools.Enums.PS3;
using PDTools.Utils;

namespace PDTools.Structures.PS3
{
    public class MGarageCar
    {

        private byte[]? _rawData;

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
        
        // ReSharper disable once InconsistentNaming
        public bool DLC { get; set; }
        
        // ReSharper disable once InconsistentNaming
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
            
            garageCar.Deserialize(ref bs);

            return garageCar;
        }

        public void Deserialize(ref BitStream bs)
        {
            _rawData = new byte[0x28];
            bs.ReadIntoByteArray(_rawData.Length, _rawData, BitStream.Byte_Bits);

            var internalBs = new BitStream(BitStreamMode.Read, _rawData);

            CarExists = internalBs.ReadBoolBit();
            // if (!garageCar.CarExists) // 1 bit
            //     return garageCar;

            Country = (Country)internalBs.ReadBits(7); // 8 bits
            Drivetrain = (Drivetrain)internalBs.ReadBits(3); // 11 bits
            Year = 1800 + internalBs.ReadByte(); // 19 bits - 2010 - 1800 = 210 or 1987 - 1800 = 87
            Favorites = internalBs.ReadBoolBit(); // 20 bits
            Aspiration = (Aspiration)internalBs.ReadBits(3); // 23 bits
            RealSpecHidden = internalBs.ReadBoolBit(); // 24 bits
            
            Unk1 = internalBs.ReadBits(BitStream.Byte_Bits * 0x3 + 4); // Unknown 52 bits
            
            FrontTire = (PARTS_TIRE)internalBs.ReadBits(5);
            RearTire = (PARTS_TIRE)internalBs.ReadBits(5);
            CanHaveDirtTires = internalBs.ReadBoolBit();
            CanHaveSnowTires = internalBs.ReadBoolBit(); // First 8 bytes fully read
            Tuner = (Tuner)internalBs.ReadByte(); // 72 bits
            
            Unk2 = internalBs.ReadBits(10); // 94 bits
            
            Weight = (uint)internalBs.ReadBits(14);
            RideCount = internalBs.ReadUInt16();
            Pp1K = (uint)internalBs.ReadBits(14);
            Invalid = internalBs.ReadBoolBit();
            Tuned = internalBs.ReadBoolBit();
            Power = (uint)internalBs.ReadBits(14);
            DLC = internalBs.ReadBoolBit();
            NOS = internalBs.ReadBoolBit();
            
            Unk3 = internalBs.ReadBits(16);

            RideOrder = internalBs.ReadUInt32();
            GarageId = internalBs.ReadUInt32();
            CarCode = internalBs.ReadUInt32();
            MainColor = internalBs.ReadUInt32();
            AccentColor = internalBs.ReadUInt32();
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
        
        internal byte[]? GetRawData()
        {
            return _rawData;
        }

        internal void UpdateRawDataAndReload(byte[] data)
        {
            var bs = new BitStream(BitStreamMode.Read, data);
            Deserialize(ref bs);
        }

    }
}