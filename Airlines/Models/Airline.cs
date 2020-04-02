using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using static Airlines.Utils.Generators;

namespace Airlines.Models
{
    public class Airline : ValidatableModelBase
    {

        #region Поля
        private string _title; // название
        private string _address; // адрес
        private Manager _manager; // генеральный директор
        private string _description; // описание компании
        private ObservableCollection<Airplane> _airplanes; // список самолетов компании
        #endregion

        #region Свойства
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value, v => string.IsNullOrWhiteSpace(v) || v.Length < 3,
                "Название компании должно состоять минимум из 3 символов");
        }

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value, v => string.IsNullOrWhiteSpace(v) || v.Length < 10,
                "Адрес компании должен состоять минимум из 10 символов");
        }

        public Manager Manager
        {
            get => _manager;
            set => SetProperty(ref _manager, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public ObservableCollection<Airplane> Airplanes
        {
            get => _airplanes;
            set => SetProperty(ref _airplanes, value);
        }

        // кол-во самолетов
        [JsonIgnore]
        public int AirplanesCount => Airplanes?.Count ?? 0;

        // общая пассажировместимость
        [JsonIgnore]
        public int TotalPassengersCount => Airplanes?.Sum(a => a.MaxPassengersCount) ?? 0;

        // общая грузоподъемность
        [JsonIgnore]
        public int TotalTakeoffWeight => Airplanes?.Sum(a => a.MaxTakeoffWeight) ?? 0;
        #endregion

        #region Генерация компаний
        public static Airline Generate(int airplanesCount)
        {
            // генерация данных
            var title = AirlineTitles[GetRand(AirlineTitles.Length - 1)];
            var address = $"{GetRand(1, 1000)} {Streets[GetRand(Streets.Length - 1)]}";
            var manager = Manager.Generate();


            // генерация самолетов
            var airplanes = new ObservableCollection<Airplane>();
            for (int i = 0; i < airplanesCount; i++)
                airplanes.Add(Airplane.Generate());

            // создание авиакомпании
            return new Airline()
            {
                Title = title,
                Address = address,
                Manager = manager,
                Description = $"Авиакомпания \"{title}\" имеет офис по адресу {address}, генеральный директор - {manager.Surname} {manager.Name} {manager.Patronymic}({manager.Email}).",
                Airplanes = airplanes
            };
        }

        // названия компаний
        // https://www.airlines-inform.ru/rankings/airline_ranking_2014.html
        private static readonly string[] AirlineTitles =
        {
            "American Airlines", "Delta Air Lines", "United Airlines", "Emirates", "Southwest Airlines",
            "Lufthansa", "China Southern Airlines", "China Eastern Airlines", "British Airways ", "Air France",
            "Ryanair", "Air China", "Turkish Airlines", "Qatar Airways", "Cathay Pacific", "Air Canada", "Singapore Airlines",
            "KLM", "Etihad Airways", "All Nippon Airways", "EasyJet", "Qantas", "Аэрофлот", "Korean Air", "JetBlue Airways",
            "Hainan Airlines", "TAM Linhas Aereas", "Thai Airways", "Japan Airlines", "Saudia", "Alaska Airlines", "Iberia",
            "Air Berlin", "Norwegian ", "Malaysia Airlines", "Asiana Airlines", "Jet Airways", "Shenzhen Airlines", "GOL Transportes Aereas",
            "Air India", "Virgin Atlantic Airways", "China Airlines", "Swiss ", "Avianca", "Sichuan Airlines",
            "EVA Air", "West Jet", "Alitalia", "IndiGo", "SAS"
        };

        // самые известные улицы мира
        // https://hiconsumption.com/29-of-the-worlds-most-famous-streets/
        private static readonly string[] Streets =
        {
            "Via Dolorosa, Jerusalem", "The Bowery, New York City", "La Pigalle, Paris",
            "Harley Street, London", "Chandni Chowk, Delhi", "Grafton Street, Dublin",
            "Orchard Road, Singapore", "Carnaby Street, London", "The Shankill Road, Belfast",
            "The Falls Road, Belfast", "Bourbon Street, New Orleans", "Savile Row, London",
            "La Rambla, Barcelona", "Camden High Street, London", "Highway 61, USA",
            "Kings Road, London", "Broadway, New York City", "Champs-Elysees, Paris",
            "The Royal Mile, Edinburgh", "Lombard Street, San Francisco", "Hollywood Boulevard, Los Angeles",
            "Downing Street, London", "Fifth Avenue, New York City", "Abbey Road, London"
        };

        #endregion

        // клон информации о компании без самолетов
        public Airline CloneWithoutAirplanes()
            => new Airline()
            {
                Title = _title,
                Address = _address,
                Manager = (Manager)_manager?.Clone(),
                Description = _description
            };
    }
}
