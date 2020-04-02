using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airlines.Models;
using Newtonsoft.Json;

namespace Airlines.Services
{
    // сервис для работы с данными приложения
    class DefaultApplicationDataService : IApplicationDataService
    {
        public DefaultApplicationDataService(string airplanesPhotoFolderPath, string videoFolderPath,
            string managersPhotoFolderPath, string airlinesDataFolderPath)
        {
            AirplanesPhotoFolderPath = airplanesPhotoFolderPath;
            AirplanesVideoFolderPath = videoFolderPath;
            ManagersPhotoFolderPath = managersPhotoFolderPath;
            AirlinesDataFolderPath = airlinesDataFolderPath;
        }

        public string AirplanesPhotoFolderPath { get; protected set; }
        public string ManagersPhotoFolderPath { get; protected set; }
        public string AirplanesVideoFolderPath { get; protected set; }
        public string AirlinesDataFolderPath { get; protected set; }

        // сохранение фото самолета в папке приложения
        public bool SaveAirplanePhotoFrom(string pathToPhoto) 
            => CopyFileToFolder(pathToPhoto, AirplanesPhotoFolderPath);

        // сохранение фото менеджера в папке приложения
        public bool SaveManagerPhotoFrom(string pathToPhoto) 
            => CopyFileToFolder(pathToPhoto, ManagersPhotoFolderPath);

        // сохранение видео в папке приложения
        public bool SaveAirplaneVideoFrom(string pathToVideo) 
            => CopyFileToFolder(pathToVideo, AirplanesVideoFolderPath);
        
        // загрузка коллекции авиалиний из папки приложения. Передается только название файла
        public ObservableCollection<Airline> UploadAirlinesFromDataFolder(string fileName)
            => UploadAirlines(Path.Combine(AirlinesDataFolderPath, fileName));

        // сохранение коллекции авиакомпаний в файл. Передается только название файла
        public void SaveAirlinesToDataFolder(string fileName, ObservableCollection<Airline> airlines)
            => SaveAirlines(Path.Combine(AirlinesDataFolderPath, fileName), airlines);

        // загрузка коллекции авиакомпаний из файла
        public ObservableCollection<Airline> UploadAirlines(string pathToFile)
        => !File.Exists(pathToFile) 
                ? null 
                : JsonConvert.DeserializeObject<ObservableCollection<Airline>>(File.ReadAllText(pathToFile));

        // сохранение коллекции авиакомпаний в файл
        public void SaveAirlines(string pathToFile, ObservableCollection<Airline> airlines)
            => File.WriteAllText(pathToFile,
                JsonConvert.SerializeObject(airlines, Formatting.Indented));

        // копирование файла в заданную папке
        private bool CopyFileToFolder(string pathToFile, string pathToFolder)
        {
            // если файла не существует - возвращает false
            if (pathToFile == null || !File.Exists(pathToFile)) return false;

            // получение имени файла
            var fileName = Path.GetFileName(pathToFile);

            // если заданной папки нет - создает
            Directory.CreateDirectory(pathToFolder);

            // копирование в папку
            File.Copy(pathToFile, Path.Combine(pathToFolder, fileName), true);

            return true;
        }

        // получение пути к заданному видео в папке приложения
        public Uri GetAirplaneVideoUri(string videoFileName)
        {
            var path = Path.Combine(AirplanesVideoFolderPath, videoFileName);
            return !File.Exists(path) ? null : new Uri(path, UriKind.Absolute);
        }

        // получение пути к заданному фото самолета в папке приложения
        public Uri GetAirplanePhotoUri(string photoFileName)
        {
            var path = Path.Combine(AirplanesPhotoFolderPath, photoFileName);
            return !File.Exists(path) ? null : new Uri(path, UriKind.Absolute);
        }

        // получение пути к заданному фото менеджера в папке приложения
        public Uri GetManagerPhotoUri(string photoFileName)
        {
            var path = Path.Combine(ManagersPhotoFolderPath, photoFileName);
            return !File.Exists(path) ? null : new Uri(path, UriKind.Absolute);
        }
    }
}
