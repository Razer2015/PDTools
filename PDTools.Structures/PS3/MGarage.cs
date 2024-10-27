using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PDTools.Enums;
using PDTools.Enums.PS3;
using PDTools.Utils;

namespace PDTools.Structures.PS3
{
    public class MGarage
    {

        private int VersionMajor { get; set; }

        private int VersionMinor { get; set; }

        private byte[] RidingCarBlob { get; set; }

        private int GarageCarVersion { get; set; }

        private int NextGarageId { get; set; }

        private int TotalChanges { get; set; }

        private int CurrentGarageId { get; set; }

        private int MaxGarageCount { get; set; }

        private uint Unk1 { get; set; } // Hash of some sort? Has something to do with stockyard

        private int Unk2 { get; set; }

        private int Unk3 { get; set; }

        // public MCarParameter? RidingCar { get; private set; }

        private List<(uint CarId, bool DlcEnabled, bool DlcInvalid)> DlcTable { get; set; } = [];

        private List<MGarageCar> Cars { get; set; } = [];

        public static MGarage Load(Span<byte> data, string? savePath = null)
        {
            var garage = new MGarage();

            var bs = new BitStream(BitStreamMode.Read, data);

            garage.VersionMajor = bs.ReadInt32();
            garage.VersionMinor = bs.ReadInt32();

            // Read riding car
            // garage.RidingCar = MCarParameter.ImportFromBlob(ref bs);
            switch (bs.ReadInt32())
            {
                case 0x6B:
                    bs.Position -= 0x04;
                    garage.RidingCarBlob = new byte[0x0200];
                    bs.ReadIntoByteArray(garage.RidingCarBlob.Length, garage.RidingCarBlob, BitStream.Byte_Bits);
                    break;
                case 0x6D:
                    bs.Position -= 0x04;
                    garage.RidingCarBlob = new byte[0x01E0];
                    bs.ReadIntoByteArray(garage.RidingCarBlob.Length, garage.RidingCarBlob, BitStream.Byte_Bits);
                    break;
            }

            // DLC car amount
            var dlcEntryCount = bs.ReadInt32();
            if (dlcEntryCount > 0)
            {
                for (var i = 0; i < dlcEntryCount; i++)
                {
                    var carId = bs.ReadUInt32();
                    var dlcEnabled = bs.ReadBoolBit();
                    var dlcInvalid = bs.ReadBoolBit();
                    garage.DlcTable.Add((carId, dlcEnabled, dlcInvalid));
                }
            }

            bs.Align(0x04);
            bs.ReadBits(dlcEntryCount);

#if DEBUG
            // byte[] buffer = new byte[bs.Length - bs.Position - 1];
            // bs.ReadIntoByteArray(buffer.Length, buffer, BitStream.Byte_Bits);
            // File.WriteAllBytes(Path.Combine(savePath, $"garage_entries.bin"), buffer);
            // return garage;
#endif

            garage.GarageCarVersion = bs.ReadInt32();
            garage.NextGarageId = bs.ReadInt32();
            garage.TotalChanges = bs.ReadInt32();
            garage.CurrentGarageId = bs.ReadInt32();
            garage.MaxGarageCount = bs.ReadInt32();
            garage.Unk1 = bs.ReadUInt32(); // This has something to do with stockyard
            garage.Unk2 = bs.ReadInt32(); // Just padding?
            garage.Unk3 = bs.ReadInt32(); // Just padding?

            for (var i = 0; i < garage.MaxGarageCount; i++)
            {
                if (bs.Position + 0x28 > bs.Length)
                    break;

                var garageCar = MGarageCar.Load(ref bs);
                garage.Cars.Add(garageCar);
// #if DEBUG
//                 if (garageCar.CarExists)
//                 {
//                     File.WriteAllBytes($"garage_entry_{i}.bin", garageCar.RawData);
//                 }
// #endif
            }

#if DEBUG
            var unk1Bools = garage.Cars.Where(x => x.Tuned).ToArray();
            var unk2Bools = garage.Cars.Where(x => x.DLC).ToArray();
            var unk3Bools = garage.Cars.Where(x => x.NOS).ToArray();

            var carModels = string.Join(", ", unk2Bools.Select(x => x.CarCode.ToString()));
#endif

            return garage;
        }

        public Span<byte> Serialize()
        {
            var bs = new BitStream(BitStreamMode.Write);

            bs.WriteInt32(VersionMajor);
            bs.WriteInt32(VersionMinor);

            // Riding car
            bs.WriteByteData(RidingCarBlob);

            // DLC car amount
            bs.WriteInt32(DlcTable.Count);
            foreach (var dlcEntry in DlcTable)
            {
                bs.WriteUInt32(dlcEntry.CarId);
                bs.WriteBoolBit(dlcEntry.DlcEnabled);
                bs.WriteBoolBit(dlcEntry.DlcInvalid);
            }

            bs.Align(0x04);
            bs.WriteBits(0, (ulong)DlcTable.Count);

            bs.WriteInt32(GarageCarVersion);
            bs.WriteInt32(NextGarageId);
            bs.WriteInt32(TotalChanges);
            bs.WriteInt32(CurrentGarageId);
            bs.WriteInt32(MaxGarageCount);
            bs.WriteUInt32(Unk1);
            bs.WriteInt32(Unk2);
            bs.WriteInt32(Unk3);

            foreach (var garageCar in Cars)
            {
                garageCar.Serialize(ref bs);
            }

            bs.Align(0x5120);

            return bs.GetBuffer();
        }

        public void AddCar(MGarageCar car)
        {
            throw new NotImplementedException();
            // Cars.Add(car);
        }

        public void RemoveCar(MGarageCar car)
        {
            throw new NotImplementedException();
            // Cars.Remove(car);
        }

        public void RemoveCar(int index)
        {
            throw new NotImplementedException();
            // Cars.RemoveAt(index);
        }

        public void ClearCars()
        {
            throw new NotImplementedException();
            // Cars.Clear();
        }

        public MGarageCar GetCar(int index)
        {
            return Cars[index];
        }

        public int GetCarCount()
        {
            return Cars.Count(x => x.CarExists);
        }

        public MGarageCar[] GetCars(MGarageFilters filters)
        {
            var sortFunc = GetSortFunc(filters.SortType);
            
            var query = Cars.Where(x => x.CarExists);
            query = filters.SortOrder == GarageSortOrder.Normal
                ? query.OrderBy(sortFunc)
                : query.OrderByDescending(sortFunc);
            
            return query.ToArray();
        }
        
        private static Func<MGarageCar, object> GetSortFunc(GarageSortType sortType)
        {
            return sortType switch
            {
                GarageSortType.Obtain => car => car.RideOrder,
                // GarageSortType.CarName => car => car.CarName,
                GarageSortType.Tuner => car => car.Tuner,
                GarageSortType.Nationality => car => car.Country,
                GarageSortType.Power => car => car.Power,
                GarageSortType.Weight => car => car.Weight,
                GarageSortType.Year => car => car.Year,
                // GarageSortType.Distance => car => car.Distance,
                GarageSortType.RideCount => car => car.RideCount,
                GarageSortType.Pp => car => car.Pp1K,
                GarageSortType.Ride => car => car.RideOrder,
                _ => throw new ArgumentOutOfRangeException(nameof(sortType), sortType, null)
            };
        }

    }

    public class MGarageFilters
    {

        public GarageSortType SortType { get; set; } = GarageSortType.Ride;

        public GarageSortOrder SortOrder { get; set; } = GarageSortOrder.Normal;

        public List<Country>? FilterNationality { get; set; }
        
        public List<Tuner>? FilterTuner { get; set; }
        
        public List<Drivetrain>? FilterDrivetrain { get; set; }

        public bool? Favorite { get; set; }

        public bool? Invalid { get; set; }
        
    }
}