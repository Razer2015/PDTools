using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PDTools.Enums;
using PDTools.Enums.PS3;
using PDTools.Utils;
using Syroot.BinaryData.Memory;

namespace PDTools.Structures.PS3
{
    public class MGarage
    {
        private int VersionMajor { get; set; }

        private int VersionMinor { get; set; }
        
        private int RidingCarVersion { get; set; }

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

        private List<(uint CarId, bool DlcExpended, bool DlcInvalid)> DlcTable { get; set; } = [];

        private MGarageCar[] Cars { get; set; } = [];
        
        // GARG - Car parameters
        public class MGarg
        {
            private string? GaragePath { get; set; }
            
            public bool GarageLoaded { get; set; }

            public uint Magic { get; set; }

            public uint Version { get; set; }

            public uint EntrySize { get; set; }

            public uint SheetCount { get; set; }

            public uint MaxCars { get; set; }

            public uint MCarParameterSizeAligned { get; set; }

            public uint MCarParameterSettingsVersion { get; set; }

            public uint MCarParameterVersion { get; set; }

            public static MGarg Read(string garagePath)
            {
                var garg = new MGarg
                {
                    GaragePath = garagePath,
                };
                
                using var fileStream = new FileStream(garagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                
                // Allocate a buffer for the header
                var header = new byte[0x20];

                // Read the first 0x20 bytes
                fileStream.Seek(0, SeekOrigin.Begin);
                var read = fileStream.Read(header, 0, header.Length);
                
                if (read != header.Length)
                    throw new InvalidDataException("Failed to read GARG header");
                
                var sr = new SpanReader(header, Syroot.BinaryData.Core.Endian.Big);
                
                // Ensure Magic is GARG
                garg.Magic = sr.ReadUInt32();
                if (garg.Magic != 0x47415247)
                    throw new InvalidDataException("Invalid GARG magic");
                
                garg.Version = sr.ReadUInt32();
                garg.EntrySize = sr.ReadUInt32();
                garg.SheetCount = sr.ReadUInt32();
                garg.MaxCars = sr.ReadUInt32();
                garg.MCarParameterSizeAligned = sr.ReadUInt32();
                garg.MCarParameterSettingsVersion = sr.ReadUInt32();
                garg.MCarParameterVersion = sr.ReadUInt32();
                garg.GarageLoaded = true;

                return garg;
            }
            
            public byte[] GetMCarParameterSheet(int index, int slotId)
            {
                if (!GarageLoaded)
                    throw new InvalidOperationException("Garage not loaded");
                
                if (GaragePath == null)
                    throw new InvalidOperationException("Garage path is null");
                
                if (slotId is < 0 or > 2)
                    throw new ArgumentException("Slot ID must be between 0 and 2 (sheet A, B or C)");
                
                using var fileStream = new FileStream(GaragePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                
                // Allocate a buffer for the sheet
                var buffer = new byte[EntrySize];

                var offset = (0x20 + index * EntrySize * SheetCount) + (slotId * EntrySize);

#if DEBUG
                Console.WriteLine($"Reading MCarParameter sheet at index {index} and slot ID {slotId} at offset {offset}");
#endif
                
                // Move to the specific offset
                fileStream.Seek(offset, SeekOrigin.Begin);

                // Read the sheet
                var read = fileStream.Read(buffer, 0, buffer.Length);
                
                if (read != buffer.Length)
                    throw new InvalidDataException("Failed to read MCarParameter sheet");
                
                return buffer;
            }
            
            public void SetMCarParameterSheet(int index, int slotId, byte[] data)
            {
                if (!GarageLoaded)
                    throw new InvalidOperationException("Garage not loaded");
                
                if (GaragePath == null)
                    throw new InvalidOperationException("Garage path is null");
                
                if (slotId is < 0 or > 2)
                    throw new ArgumentException("Slot ID must be between 0 and 2 (sheet A, B or C)");
                
                using var fileStream = new FileStream(GaragePath, FileMode.Open, FileAccess.Write);
                
                var offset = (0x20 + index * EntrySize * SheetCount) + (slotId * EntrySize);
                
                // Move to the specific offset
                fileStream.Seek(offset, SeekOrigin.Begin);
                
                // Write the sheet
                fileStream.Write(data, 0, data.Length);
            }

            public void RemoveCar(int index)
            {
                SetMCarParameterSheet(index, 0, new byte[EntrySize]);
                SetMCarParameterSheet(index, 1, new byte[EntrySize]);
                SetMCarParameterSheet(index, 2, new byte[EntrySize]);
            }
            
            public void Write()
            {
                if (GaragePath == null)
                    throw new InvalidOperationException("Garage path is null");
                
                using var fs = new FileStream(GaragePath, FileMode.OpenOrCreate, FileAccess.Write);
                var bw = new SpanWriter(new byte[0x20], Syroot.BinaryData.Core.Endian.Big);
                
                bw.WriteUInt32(Magic);
                bw.WriteUInt32(Version);
                bw.WriteUInt32(EntrySize);
                bw.WriteUInt32(SheetCount);
                bw.WriteUInt32(MaxCars);
                bw.WriteUInt32(MCarParameterSizeAligned);
                bw.WriteUInt32(MCarParameterSettingsVersion);
                bw.WriteUInt32(MCarParameterVersion);
                    
                fs.Write(bw.Span);
            }
        }
        
        private MGarg? Garg { get; set; }

        public bool Load(string path)
        {
            if (!File.Exists(path))
                return false;
            
            Garg = MGarg.Read(path);

            return true;
        }
        
        public void Save()
        {
            Garg?.Write();
        }

        public static MGarage Deserialize(Span<byte> data, string? savePath = null)
        {
            var garage = new MGarage();

            var bs = new BitStream(BitStreamMode.Read, data);

            garage.VersionMajor = bs.ReadInt32();
            garage.VersionMinor = bs.ReadInt32();

            // Read riding car
            // garage.RidingCar = MCarParameter.ImportFromBlob(ref bs);
            garage.RidingCarVersion = bs.ReadInt32();
            switch (garage.RidingCarVersion)
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

            garage.Cars = new MGarageCar[garage.MaxGarageCount];
            for (var i = 0; i < garage.MaxGarageCount; i++)
            {
                if (bs.Position + 0x28 > bs.Length)
                {
                    garage.Cars[i] = new MGarageCar();
                    continue;
                }

                var garageCar = MGarageCar.Load(ref bs);
                garage.Cars[i] = garageCar;
// #if DEBUG
//                 if (garageCar.CarExists)
//                 {
//                     File.WriteAllBytes($"garage_entry_{i}.bin", garageCar.RawData);
//                 }
// #endif
            }

#if DEBUG
            // var unk1Bools = garage.Cars.Where(x => x.Tuned).ToArray();
            // var unk2Bools = garage.Cars.Where(x => x.DLC).ToArray();
            // var unk3Bools = garage.Cars.Where(x => x.NOS).ToArray();
            //
            // var carModels = string.Join(", ", unk2Bools.Select(x => x.CarCode.ToString()));
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
                bs.WriteBoolBit(dlcEntry.DlcExpended);
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
            
            bs.Align(0x20C /* Headers, RidingCar, etc. */ 
                     + Cars.Length * 0x28 /* GarageCars */ 
                     + 0xF4 /* Idk but maybe padding in preparation for DlcTable later on since they push the data forwards without making the size bigger? */
            );
            // bs.Align(0x5120);

            return bs.GetBuffer();
        }

        public int GetRidingCarId()
        {
            return CurrentGarageId;
        }
        
        // ReSharper disable once InconsistentNaming
        public bool HasDLCExpended(int carCode)
        {
            return DlcTable.Any(x => x.CarId == carCode && x.DlcExpended);
        }
        
        public void SetInvalid(int carCode, bool invalid)
        {
            // Set invalid flag to the DlcTable
            for (var i = 0; i < DlcTable.Count; i++)
            {
                if (DlcTable[i].CarId == carCode && DlcTable[i].DlcExpended)
                {
                    DlcTable[i] = (DlcTable[i].CarId, DlcTable[i].DlcExpended, invalid);
                }
            }
            
            // Set invalid flag to the garage car
            for (var i = 0; i < Cars.Length; i++)
            {
                if (Cars[i].CarCode == carCode)
                {
                    Cars[i].Invalid = invalid;
                }
            }
        }
        
        public MCarParameter GetCar(uint garageId)
        {
            var garageCar = ReferGarageCar(garageId);
            
            return GetCar(garageId, garageCar.SlotId);
        }
        
        private MCarParameter GetCar(uint garageId, int slotId)
        {
            // Get the car data
            var carData = GetCarRaw(garageId, slotId);
            
            return MCarParameter.ImportFromBlob(carData);
        }
        
        public byte[] GetCarRaw(uint garageId, int slotId)
        {
            if (Garg is not { GarageLoaded: true })
                throw new InvalidOperationException("Garage not loaded");
            
            if (slotId is < 0 or > 2)
                throw new ArgumentException("Slot ID must be between 0 and 2 (sheet A, B or C)");
            
            // Get index of the car in the garage
            var index = Array.FindIndex(Cars, x => x.GarageId == garageId);
            if (index == -1)
                throw new InvalidOperationException("Car not found in garage");
            
            // Get the car data
            var carData = Garg!.GetMCarParameterSheet(index, slotId);
            
            return carData;
        }

        public MCarParameter GetRidingCar()
        {
            return MCarParameter.ImportFromBlob(RidingCarBlob);
        }

        public void AddCar(MCarParameter car, MGarageCar garageCar)
        {
            AddCar(car, false, garageCar);
        }
        
        public void AddCar(MCarParameter car, bool voucherCar, MGarageCar garageCar)
        {
            if (Cars.Count(x => x.CarExists) >= MaxGarageCount)
                throw new InvalidOperationException("Garage is full");
            
            // Prepare the garage car
            garageCar.CarExists = true;
            garageCar.GarageId = (uint)NextGarageId;
            garageCar.Invalid = false;
            
            // Prepare the car
            car.Settings.GarageID = NextGarageId;
            car.ObtainDate = new PDIDATETIME32(DateTime.Now);
            
            // Find the first empty slot
            var index = Array.FindIndex(Cars, x => !x.CarExists);
            if (index == -1)
                throw new InvalidOperationException("No empty slots in garage");
            
            // Add the garageCar
            Cars[index] = garageCar;
            
            // Add the car parameters
            var carData = car.Serialize().ToArray();
            Garg!.SetMCarParameterSheet(index, 0, carData);
            Garg!.SetMCarParameterSheet(index, 1, carData);
            Garg!.SetMCarParameterSheet(index, 2, carData);
            
            // Add the DLC entry
            if (voucherCar)
            {
                DlcTable.Add((garageCar.CarCode, true, false));
            }
            
            // Increment the garage ID
            NextGarageId++;
        }

        public void RemoveCar(MGarageCar car)
        {
            var index = Array.FindIndex(Cars, x => x.GarageId == car.GarageId);
            if (index == -1)
                throw new InvalidOperationException("Car not found in garage");
            
            RemoveCar(index);
        }

        public void RemoveCar(int index)
        {
            if (index < 0 || index >= Cars.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            
            // Is currently riding car
            if (CurrentGarageId == Cars[index].GarageId)
            {
                // throw new InvalidOperationException("Cannot remove currently riding car");
                CurrentGarageId = -1;
            }
            
            // Remove the car from the garage car
            Cars[index].CarExists = false;
            
            // Remove the car from the garage
            Garg!.RemoveCar(index);
        }

        public void ClearCars()
        {
            var existingCarIndexes = Cars
                .Select((car, index) => new { car, index })
                .Where(x => x.car.CarExists)
                .Select(x => x.index)
                .ToArray();

            foreach (var index in existingCarIndexes)
            {
                RemoveCar(index);
            }
        }

        public MGarageCar ReferGarageCar(uint garageId)
        {
            var garageCar = Cars.FirstOrDefault(x => x.GarageId == garageId);
            if (garageCar == null)
                throw new InvalidOperationException("Car not found in garage");
            
            return garageCar;
        }

        public byte[]? GetMGarageRawData(uint garageId)
        {
            return Cars.FirstOrDefault(x => x.GarageId == garageId)?.GetRawData();
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
                ? query.OrderByDescending(sortFunc)
                : query.OrderBy(sortFunc);

            return query.ToArray();
        }

        #region DLC/Voucher methods

        public void ClearDlcEntries()
        {
            DlcTable = [];
        }

        public void AddDlcEntry(uint carId, bool dlcEnabled, bool dlcInvalid)
        {
            DlcTable.Add((carId, dlcEnabled, dlcInvalid));
        }

        public void RemoveDlcEntry(uint carId)
        {
            DlcTable.RemoveAll(x => x.CarId == carId);
        }

        public bool IsDlcCar(uint carId)
        {
            return DlcTable.Any(x => x.CarId == carId);
        }

        public bool IsDlcCarEnabled(uint carId)
        {
            return DlcTable.FirstOrDefault(x => x.CarId == carId).DlcExpended;
        }

        public bool IsDlcCarInvalid(uint carId)
        {
            return DlcTable.FirstOrDefault(x => x.CarId == carId).DlcInvalid;
        }

        public void RevalidateDlcCars()
        {
            Cars.Where(x => x.Invalid).ToList().ForEach(x => x.Invalid = false);
        }

        #endregion

        #region Update MGarageCar methods

        public void UpdateMGarageRawData(uint garageId, byte[] rawData)
        {
            var index = Array.FindIndex(Cars, x => x.GarageId == garageId);
            if (index == -1)
                return;

            Cars[index]?.UpdateRawDataAndReload(rawData);
        }

        public void UpdateMGarageCar(uint garageId, MGarageCar car)
        {
            var index = Array.FindIndex(Cars, x => x.GarageId == garageId);
            if (index == -1)
                return;

            Cars[index] = car;
        }

        #endregion

        private static Func<MGarageCar, object> GetSortFunc(GarageSortType sortType)
        {
            return sortType switch
            {
                GarageSortType.Obtain => car => car.GarageId,
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