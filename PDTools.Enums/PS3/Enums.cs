using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDTools.Enums.PS3
{
    public enum AutomobileControllerType
    {
        UNKNOWN,
        AI,
        PAD_BUTTON,
        PAD_STICK,
        WHEEL200,
        WHEEL900S,
        WHEEL900G,
        WHEEL900GC,
        PAD_MIN,
        PAD_MAX,
        WHEEL_MIN,
        WHEEL_MAX,
    }

    public enum CarPartsType
    {
        BRAKE = 2,
        BRAKE_CONTROLLER,
        SUSPENSION,
        ASCC,
        TCSC,
        RACING_MODIFY,
        CHASSIS,
        LIGHT_WEIGHT,
        STEER,
        DRIVETRAIN,
        GEAR,
        ENGINE,
        NATUNE,
        TURBINEKIT,
        DISPLACEMENT,
        COMPUTER,
        INTERCOOLER,
        MUFFLER,
        CLUTCH,
        FLYWHEEL,
        PROPELLERSHAFT,
        LSD,
        FRONT_TIRE,
        REAR_TIRE,
        NOS,
        SUPERCHARGER,
        INTAKE_MANIFOLD,
        EXHAUST_MANIFOLD,
        CATALYST,
        AIR_CLEANER,
        BOOST_CONTROLLER,
        INDEP_THROTTLE,
        LIGHT_WEIGHT_WINDOW,
        BONNET,
        AERO,
        FLAT_FLOOR,
        FREEDOM,
        WING,
        STIFFNESS,
        SPECIAL_GAS
    }

    public enum PARTS_BRAKE
    {
        NORMAL,
        _4PISTON,
        _6PISTON,
        _8PISTON,
        CARBON,
    }

    public enum PARTS_BRAKE_CONTROLLER
    {
        NO,
        ONE,
    }

    public enum PARTS_ASCC
    {
        NO,
        ONE,
    }

    public enum PARTS_TCSC
    {
        NO,
        ONE,
    }

    public enum PARTS_LIGHT_WEIGHT
    {
        NONE = -1,
        STAGE1 = 1,
        STAGE2,
        STAGE3,
        STAGE4,
        STAGE5,
        STAGE6,
        STAGE7,
        STAGE8,
    }

    public enum PARTS_DRIVETRAIN
    {
        NORMAL,
        VARIABLE_CENTER_DIFF,
        ACTIVE_CENTER_DIFF,
    }

    public enum PARTS_DISPLACEMENT
    {
        NONE = -1,
        LEVEL1 = 1,
        LEVEL2,
        LEVEL3,
    }

    public enum PARTS_INTERCOOLER
    {
        NONE = -1,
        S,
        M,
        L,
        LL,
    }

    public enum PARTS_CLUTCH
    {
        NONE = -1,
        NORMAL,
        HIGH_CAPACITY,
        TWIN,
        TRIPLE,
    }

    public enum PARTS_FLYWHEEL
    {
        NONE = -1,
        LIGHT = 1,
        Cr_Mo,
        LIGHT_Cr_Mo,
    }

    public enum PARTS_PROPELLERSHAFT
    {
        NONE = -1,
        ONE = 1,
    }

    public enum PARTS_LSD
    {
        NORMAL,
        VARIABLE,
        AYCC,
    }

    public enum PARTS_TIRE
    {
        [Description("Comfort: Hard")]
        COMFORT_HARD,
        [Description("Comfort: Medium")]
        COMFORT_MEDIUM,
        [Description("Comfort: Soft")]
        COMFORT_SOFT,
        [Description("Sports: Hard")]
        SPORTS_HARD,
        [Description("Sports: Medium")]
        SPORTS_MEDIUM,
        [Description("Sports: Soft")]
        SPORTS_SOFT,
        [Description("Sports: Super-Soft")]
        SPORTS_SUPER_SOFT,
        [Description("Racing: Hard")]
        RACING_HARD,
        [Description("Racing: Medium")]
        RACING_MEDIUM,
        [Description("Racing: Soft")]
        RACING_SOFT,
        [Description("Racing: Super-Soft")]
        RACING_SUPER_SOFT,
        [Description("Racing: Intermediate")]
        RAIN_INTERMEDIATE,
        [Description("Racing: Rain")]
        RAIN_HEAVY_WET,
        [Description("Dirt Tyres")]
        DIRT,
        [Description("Snow Tyres")]
        SNOW,
        TIRE_DRY_MIN = 0,
        TIRE_DRY_MAX = 11,
        TIRE_TARMAC_MIN = 0,
        TIRE_TARMAC_MAX = 13,
    }

    public enum PARTS_NOS
    {
        NONE = -1,
        ONE = 1,
    }

    public enum PARTS_SUPERCHARGER
    {
        NONE = -1,
        ONE = 1,
    }

    public enum PARTS_INTAKE_MANIFOLD
    {
        NONE = -1,
        ONE = 1,
    }

    public enum PARTS_EXHAUST_MANIFOLD
    {
        NONE = -1,
        ONE = 1,
    }

    public enum PARTS_CATALYST
    {
        NONE = -1,
        SPORTS = 1,
        RACING = 2
    }

    public enum PARTS_AIR_CLEANER
    {
        NONE = -1,
        SPORTS = 1,
        RACING = 2
    }

    public enum PARTS_BOOST_CONTROLLER
    {
        ONE = 1,
    }

    public enum PARTS_INDEP_THROTTLE
    {
        ONE = 1,
    }

    public enum PARTS_LIGHT_WEIGHT_WINDOW
    {
        NONE,
        ONE,
    }

    public enum PARTS_BONNET
    {
        NONE,
        CARBON,
        PAINT_CARBON,
    }

    public enum PARTS_AERO
    {
        A = 1,
        B = 2,
        C = 3
    }

    public enum PARTS_FLAT_FLOOR
    {
        A = 1,
    }

    public enum PARTS_FREEDOM
    {
        F1 = 1,
        F2 = 2,
        F3 = 3
    }

    public enum PARTS_WING
    {
        WINGLESS = 1,
        CUSTOM,
        W1,
        W2,
        W3,
    }

    public enum PARTS_STIFFNESS
    {
        ONE = 1,
    }


    public enum PARTS_SUSPENSION 
    {
        [Description("Unspecified")]
        UNSPECIFIED = -1,

        [Description("Default")]
        NORMAL,

        [Description("Racing Suspension: Soft")]
        SPORTS1,

        [Description("Racing Suspension: Hard")]
        SPORTS2,

        [Description("Suspension: Rally")]
        SPORTS3,

        [Description("Height-Adjustable, Fully Customisable Suspension")]
        RACING,

        [Description("Full Active (?)")]
        FULL_ACTIVE,
    }

    public enum PARTS_GEAR
    {
        [Description("Unspecified")]
        UNSPECIFIED = -1,

        [Description("Default")]
        NORMAL,

        [Description("Five-Speed Transmission")]
        CLOSE,

        [Description("Six-Speed Transmission")]
        SUPER_CLOSE,

        [Description("Fully Customisable Transmission")]
        VARIABLE,
    }

    public enum PARTS_NATUNE
    {
        [Description("Default")]
        NONE = -1,

        [Description("(Placeholder Level 0)")]
        LEVEL0 = 0,

        [Description("Stage 1")]
        LEVEL1 = 1,

        [Description("Stage 2")]
        LEVEL2,

        [Description("Stage 3")]
        LEVEL3,

        [Description("Stage 4 (Normally Unavailable)")]
        LEVEL4,

        [Description("Stage 5 (Normally Unavailable)")]
        LEVEL5,
    }

    public enum PARTS_TURBINEKIT
    {
        [Description("Default")]
        NONE = -1,

        [Description("NO (?)")]
        NO,

        [Description("Low RPM Range Turbo Kit")]
        LEVEL1,

        [Description("Mid RPM Range Turbo Kit")]
        LEVEL2,

        [Description("High RPM Range Turbo Kit")]
        LEVEL3,

        [Description("Super RPM Range Turbo Kit (Normally Unavailable)")]
        LEVEL4,

        [Description("Ultra RPM Range Turbo Kit (Normally Unavailable)")]
        LEVEL5,
    }

    public enum PARTS_COMPUTER
    {
        [Description("Default")]
        NONE = -1,

        [Description("Sports Computer")]
        LEVEL1 = 1,
        LEVEL2,
    }

    public enum PARTS_MUFFLER
    {
        [Description("Unspecified")]
        UNSPECIFIED = -1,

        [Description("Default")]
        NONE,

        [Description("Sports Exhaust")]
        SPORTS,

        [Description("Semi-Racing Exhaust")]
        SEMIRACING,

        [Description("Racing Exhaust")]
        RACING,
    }


    public enum GameItemType
    {
        NONE,
        CAR,
        DRIVER,
        DRIVER_ITEM,
        MONEY,
        TUNE_PARTS,
        OTHERPARTS,
        MUSEUMCARD,
        MOVIE,
        SPECIAL,
        PARTS_TICKET,
        AVATAR,
        OTHER,
    }

    // x** = Main Category
    // *x* = Sub Category
    // **x = Item 
    public enum GameItemCategory
    {
        [Description("None")]
        NONE,

        [Description("Car (100)")]
        CAR = 100,

        [Description("Driver (200)")]
        DRIVER = 200,

        [Description("Driver Item (300)")]
        DRIVER_ITEM = 300,

        [Description("Driver Head (301)")]
        DRIVER_HEAD = 301,

        [Description("Driver Body (302)")]
        DRIVER_BODY = 302,

        [Description("Driver Set (303)")]
        DRIVER_SET = 303,

        [Description("Money (400)")]
        MONEY = 400,

        [Description("Tuning Parts (500)")]
        TUNERPARTS = 500,

        [Description("Body/Chassis (511)")]
        BODY_CHASSIS = 511,

        [Description("Engine (521)")]
        ENGINE = 521,

        [Description("Admission (531)")]
        ADMISSION = 531,

        [Description("Emission (532)")]
        EMISSION = 532,

        [Description("Booster (541)")]
        BOOSTER = 541,

        [Description("Transmission (551)")]
        TRANSMISSION = 551,

        [Description("Drivetrain (556)")]
        DRIVETRAIN = 556,

        [Description("Suspension (561)")]
        SUSPENSION = 561,

        [Description("Brake (571)")]
        BRAKE = 571,

        [Description("B Tire (581)")]
        BTIRE = 581,

        [Description("C Tire (582)")]
        CTIRE = 582,

        [Description("V TIre (583)")]
        VTIRE = 583,

        [Description("S Tire (586)")]
        STIRE = 586,

        [Description("Others (591)")]
        OTHERS = 591,

        [Description("Horn (596)")]
        HORN = 596,

        [Description("Other Parts (600)")]
        OTHER_PARTS = 600,

        [Description("Paint Item (601)")]
        PAINT_ITEM = 601,

        [Description("Special Paint Item (602)")]
        SPECIAL_PAINT_ITEM = 602,

        [Description("Museum Card (700)")]
        MUSEUMCARD = 700,

        [Description("Movie (800)")]
        MOVIE = 800,

        [Description("Special (900)")]
        SPECIAL = 900,

        [Description("Car Ticket (901)")]
        PRESENTCAR_TICKET = 901,

        [Description("Item Ticket (902)")]
        PRESENTITEM_TICKET = 902,

        [Description("Special Ticket (903)")]
        SPECIAL_TICKET = 903,
    }
    
    public enum Drivetrain
    {
        [Description("FR")]
        FR = 0,
        
        [Description("FF")]
        FF = 1,
        
        [Description("4WD")]
        AWD = 2,
        
        [Description("MR")]
        MR = 3,
        
        [Description("RR")]
        RR = 4,
    }
    
    public enum Aspiration
    {
        [Description("Naturally Aspirated")]
        NA = 0,

        [Description("Turbocharged")]
        TURBO = 1,

        [Description("Supercharged")]
        SUPERCHARGED = 2,
        
        [Description("Hybrid")]
        EV = 4,
    }

    public enum GarageSortOrder
    {
        Normal = 0,
        Reverse = 1,
    }

    public enum GarageSortType
    {
        Obtain = 0,
        CarName = 1,
        Tuner = 2,
        Nationality = 3,
        Power = 4,
        Weight = 5,
        Year = 6,
        Distance = 7,
        RideCount = 8,
        Pp  = 9,
        Ride = 10,
    }

    [Flags]
    public enum GenericFlag
    {
        UsePresetEnemy = 0x1,
        UseProBehavior = 0x2,
        UsePremiumModel = 0x4,
        DontSell = 0x8,
        CanChangeDecken = 0x10,
        CanAllPaint = 0x20,
        IsF1Car = 0x40,
        CanPresent = 0x80,
        HaveDirtTire = 0x100,
        CanChangeWing = 0x200,
        HaveCarbonBonnet = 0x400,
        HaveLightWeightWindot = 0x800,
        IsVoucherCar = 0x1000,
        IsRacingKart = 0x2000,
        IsStrangeCar = 0x4000,
        OneMakeOnly = 0x8000,
        CanCarWash = 0x10000,
        NoUse17 = 0x20000,
        IsDlcCar = 0x40000,
        IsDctrlImpreza = 0x80000,
    }

    public enum RacingGearType
    {
        [Description("AVATAR_MET")]
        AvatarMet = 0,
        [Description("AVATAR_SUIT")]
        AvatarSuit = 1,
        [Description("AVATAR_SET")]
        AvatarSet = 2,
    }
}
