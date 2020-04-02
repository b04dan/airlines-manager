using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Airlines.Models
{
    public class Airplane : ValidatableModelBase, ICloneable
    {

        #region Поля
        private string _name; // модель самолета
        private string _photoFileName; // название файла с фото
        private string _videoFileName; // название файла с видео
        private string _regNumber; // регистрационный номер
        private int _maxPassengersCount; // максимальное число пассажиров
        private int _maxTakeoffWeight; // максимальный взлетный вес(кг)
        private int _maxFlightHeight; // максимальная высота полета(м)
        private int _cruisingSpeed; // крейсерская скорость(км/ч)
        private int _fuelConsumption; // часовой расход топлива(кг)
        private int _flightRange; // дальность полета(км)
        private double _length; // длина самолета(м)
        private double _height; // высота самолета(м)
        private double _wingspan; // размах крыльев(м)
        private long _price; // стоимость
        #endregion

        #region Свойства
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value, string.IsNullOrWhiteSpace,
                "Модель самолета не может быть пустой строкой");
        }

        public string PhotoFileName
        {
            get => _photoFileName;
            set => SetProperty(ref _photoFileName, value);
        }

        public string VideoFileName
        {
            get => _videoFileName;
            set => SetProperty(ref _videoFileName, value);
        }

        public string RegNumber
        {
            get => _regNumber;
            set => SetProperty(ref _regNumber, value, v => string.IsNullOrWhiteSpace(v) || v.Length < 6,
                "Регистрационный номер самолета должен состоять минимум из 6 символов");
        }

        public int MaxPassengersCount
        {
            get => _maxPassengersCount;
            set => SetProperty(ref _maxPassengersCount, value, v => v < 1,
                "Максимальное кол-во пассажиров не может быть меньше 1чел.");
        }

        public int MaxTakeoffWeight
        {
            get => _maxTakeoffWeight;
            set => SetProperty(ref _maxTakeoffWeight, value, v => v < 1000,
                "Максимальный взлетный вес самолета не может быть меньше 1000кг.");
        }

        public int MaxFlightHeight
        {
            get => _maxFlightHeight;
            set => SetProperty(ref _maxFlightHeight, value, v => v < 20 || v > 20000,
                "Максимальная высота полета самолета не может быть меньше 20м или больше 20000м.");
        }

        public int CruisingSpeed
        {
            get => _cruisingSpeed;
            set => SetProperty(ref _cruisingSpeed, value, v => v < 20,
                "Крейсерская скорость самолета не может быть меньше 20 км/час.");
        }

        public int FuelConsumption
        {
            get => _fuelConsumption;
            set => SetProperty(ref _fuelConsumption, value, v => v <= 0,
                "Часовой расход топлива самолета должен быть больше 1 кг.");
        }

        public int FlightRange
        {
            get => _flightRange;
            set => SetProperty(ref _flightRange, value, v => v <= 0,
                "Дальность полета самолета должна быть больше 1 км.");
        }

        public double Length
        {
            get => _length;
            set => SetProperty(ref _length, value, v => v <= 0,
                "Длина самолета должна быть больше 1м.");
        }

        public double Height
        {
            get => _height;
            set => SetProperty(ref _height, value, v => v <= 0,
                "Высота самолета должна быть больше 1м.");
        }

        public double Wingspan
        {
            get => _wingspan;
            set => SetProperty(ref _wingspan, value, v => v <= 0,
                "Размах крыльев самолета должен быть больше 1м.");
        }

        public long Price
        {
            get => _price;
            set => SetProperty(ref _price, value, v => v < 0,
                "Цена должна быть целым положительным числом.");
        }
        #endregion

        #region Генерация самолетов
        public static Airplane Generate()
        {
            var airplane = (Airplane)Airplanes[Utils.Generators.GetRand(Airplanes.Count - 1)].Clone();
            airplane.RegNumber = Utils.Generators.GenerateNumber();
            return airplane;
        }

        // список самолетов, используемый для генерации 
        private static readonly List<Airplane> Airplanes = new List<Airplane>
        {
            // Данные взяты отсюда: https://jets.ru/enc/
            // Параметры самолетов заданы сразу, а не генерируются, чтобы были реальные значения
            
            new Airplane
            {
                Name = "Boeing BBJ 787", 
                // самолет без видео и фото. Для демонстрации
                /*VideoFileName = "default.mpg", PhotoFileName = @"boeing_bbj787.jpg",*/
                MaxPassengersCount = 100,
                MaxTakeoffWeight = 227930,
                MaxFlightHeight = 13000,
                CruisingSpeed = 917,
                FuelConsumption = 2500,
                FlightRange = 18418,
                Length = 56.7,
                Height = 16.3,
                Wingspan = 60.1,
                Price = 224000000
            },
            new Airplane
            {
                Name = "Airbus ACJ318",
                PhotoFileName = @"airbus_acj318.jpg",
                MaxPassengersCount = 19,
                MaxTakeoffWeight = 67993,
                VideoFileName = "default.mpg",
                MaxFlightHeight = 12500,
                CruisingSpeed = 898,
                FuelConsumption = 2000,
                FlightRange = 7800,
                Length = 31.45,
                Height = 12.51,
                Wingspan = 35.8,
                Price = 72000000
            },
            new Airplane
            {
                Name = "Airbus ACJ380", 
                // самолет без видео. Для демонстрации
                /*VideoFileName = "default.mpg",*/
                PhotoFileName = @"airbus_acj380.jpg",
                MaxPassengersCount = 100,
                MaxTakeoffWeight = 560000,
                MaxFlightHeight = 13100,
                CruisingSpeed = 900,
                FuelConsumption = 3200,
                FlightRange = 17250,
                Length = 72.72,
                Height = 24.09,
                Wingspan = 79.75,
                Price = 445000000
            },
            new Airplane
            {
                Name = "Avro Business Jet",
                PhotoFileName = @"avro_business_jet.jpg",
                MaxPassengersCount = 46,
                MaxTakeoffWeight = 43998,
                VideoFileName = "default.mpg",
                MaxFlightHeight = 9400,
                CruisingSpeed = 760,
                FuelConsumption = 2300,
                FlightRange = 2130,
                Length = 28.55,
                Height = 8.61,
                Wingspan = 26.34,
                Price = 145000000
            },
            new Airplane
            {
                Name = "Boeing BBJ MAX 8",
                PhotoFileName = @"boeing_bbj.jpg",
                MaxPassengersCount = 50,
                MaxTakeoffWeight = 77564,
                VideoFileName = "default.mpg",
                MaxFlightHeight = 10371,
                CruisingSpeed = 842,
                FuelConsumption = 2600,
                FlightRange = 11519,
                Length = 35.60,
                Height = 12.30,
                Wingspan = 35.90,
                Price = 71000000
            },
            new Airplane
            {
                Name = "Gulfstream G550",
                CruisingSpeed = 941,
                MaxFlightHeight = 15545,
                MaxTakeoffWeight = 41277,
                FlightRange = 12501,
                MaxPassengersCount = 18,
                Length = 29.39,
                Height = 7.87,
                Wingspan = 28.5,
                FuelConsumption = 2000,
                PhotoFileName = "gulfstream_g550.jpg",
                // самолет без видео. Для демонстрации
                /*VideoFileName = "default.mpg",*/
                Price = 61500000
            },
            new Airplane
            {
                Name = "Dassault Falcon 8X",
                CruisingSpeed = 1100,
                MaxFlightHeight = 1545,
                MaxTakeoffWeight = 33113,
                FlightRange = 11945,
                MaxPassengersCount = 19,
                Length = 24.46,
                Height = 7.94,
                Wingspan = 26.29,
                FuelConsumption = 1900,
                PhotoFileName = "dassault_falcon_8x.jpg",
                VideoFileName = "default.mpg",
                Price = 59000000
            },
            new Airplane
            {
                Name = "Sukhoi Business Jet",
                CruisingSpeed = 860,
                MaxFlightHeight = 12192,
                MaxTakeoffWeight = 49450,
                FlightRange = 7880,
                MaxPassengersCount = 19,
                Length = 29.94,
                Height = 10.28,
                Wingspan = 27.80,
                FuelConsumption = 2600,
                PhotoFileName = "sukhoi_business_jet.jpg",
                VideoFileName = "default.mpg",
                Price = 35000000
            },
            new Airplane
            {
                Name = "Gulfstream V",
                CruisingSpeed = 851,
                MaxFlightHeight = 15545,
                MaxTakeoffWeight = 41051,
                FlightRange = 11903,
                MaxPassengersCount = 19,
                Length = 29.39,
                Height = 7.87,
                Wingspan = 28.50,
                FuelConsumption = 3200,
                PhotoFileName = "gulfstream_v.jpg",
                VideoFileName = "default.mpg",
                Price = 29500000
            },
            new Airplane
            {
                Name = "Gulfstream G650",
                CruisingSpeed = 982,
                MaxFlightHeight = 15545,
                MaxTakeoffWeight = 45178,
                FlightRange = 12964,
                MaxPassengersCount = 18,
                Length = 30.40,
                Height = 7.82,
                Wingspan = 30.35,
                FuelConsumption = 1700,
                PhotoFileName = "gulfstream_g650.jpg",
                VideoFileName = "default.mpg",
                Price = 64500000
            }
        };
        #endregion

        public object Clone()
        {
            // в данном случае можно использовать метод поверхностного копирования,
            // т.к. используются только примитивные типы
            return MemberwiseClone();
        }

        // словарь названий и значений полей, по которым достпуна фильтрация
        // используется для вывода в ComboBox
        public static readonly Dictionary<string, string> FilteredFields = new Dictionary<string, string>()
        {
            ["Name"] = "Модель",
            ["RegNumber"] = "Регистрационный номер",
            ["MaxPassengersCount"] = "Максимальное число пассажиров",
            ["MaxTakeoffWeight"] = "Максимальный взлетный вес",
            ["MaxFlightHeight"] = "Максимальная высота полета",
            ["CruisingSpeed"] = "Крейсерская скорость",
            ["FuelConsumption"] = "Часовой расход топлива",
            ["FlightRange"] = "Дальность полета",
            ["Length"] = "Длина самолета",
            ["Height"] = "Высота самолета",
            ["Wingspan"] = "Размах крыльев",
            ["Price"] = "Стоимость"
        };

    }
}
