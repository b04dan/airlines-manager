using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airlines.Models;

namespace Airlines.Services
{
    // TODO: переделать
    // очень топорное решение, думаю, можно намного упростить

    // сервис для работы с данными приложения
    public interface IApplicationDataService
    {
        string AirplanesPhotoFolderPath { get; }        // путь к папке с фото самолетов
        string ManagersPhotoFolderPath { get; }         // путь к папке с фото менеджеров
        string AirplanesVideoFolderPath { get; }        // путь к папке с видео
        string AirlinesDataFolderPath { get; }          // путь к папке с данными о авиакомпаниях

        bool SaveAirplanePhotoFrom(string pathToPhoto); // сохранение фото самолета в папке приложения
        bool SaveManagerPhotoFrom(string pathToPhoto);  // сохранение фото директора в папке приложения
        bool SaveAirplaneVideoFrom(string pathToVideo); // сохранение видео в папке приложения
        void SaveAirlines(string filename, ObservableCollection<Airline> airlines); // сохранение данных в папке приложения
        ObservableCollection<Airline> UploadAirlines(string filename); // загрузка данных из папки приложения

        void SaveAirlinesToDataFolder(string fileName, ObservableCollection<Airline> airlines);
        ObservableCollection<Airline> UploadAirlinesFromDataFolder(string fileName);

        Uri GetAirplaneVideoUri(string videoFileName); // получение пути к заданному видео в папке приложения
        Uri GetAirplanePhotoUri(string photoFileName); // получение пути к фото самолета в папке приложения
        Uri GetManagerPhotoUri(string photoFileName);  // получение пути к фото директора в папке приложения
    }
}
