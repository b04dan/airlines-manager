using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Airlines.Models;
using Airlines.Services;
using Airlines.Utils;
using MaterialDesignThemes.Wpf;

namespace Airlines.ViewModels
{
    class AirplaneDetailsDialogViewModel : ViewModelBase
    {
        public AirplaneDetailsDialogViewModel(IApplicationDataService dataService, Airplane airplane)
        {
            Airplane = airplane;
            DataService = dataService;
        }

        #region Properties

        private IApplicationDataService DataService { get; }

        private Airplane _airplane;
        public Airplane Airplane
        {
            get => _airplane;
            set => SetProperty(ref _airplane, value);
        }

        #endregion

        #region Commands

        // вывод видео самолета в новом окне
        private RelayCommand _showAirplaneVideoWindowCommand;
        public ICommand ShowAirplaneVideoWindowCommand
            => _showAirplaneVideoWindowCommand ?? (_showAirplaneVideoWindowCommand = new RelayCommand(ExecuteShowAirplaneVideoWindow, 
                o => Airplane.VideoFileName != null));

        // вывод фото самолета в новом окне
        private RelayCommand _showAirplanePhotoWindowCommand;
        public ICommand ShowAirplanePhotoWindowCommand
            => _showAirplanePhotoWindowCommand ?? (_showAirplanePhotoWindowCommand = new RelayCommand(ExecuteShowAirplanePhotoWindow, 
                o => Airplane.PhotoFileName != null));

        #endregion

        #region Methods

        // вывод видео самолета в новом окне
        private void ExecuteShowAirplaneVideoWindow(object o)
        {
            // создание ViewModel'и для отображения видео
            var vm = new VideoDialogViewModel(DataService.GetAirplaneVideoUri(Airplane.VideoFileName))
            {
                Title = $"{Airplane.Name} - {Airplane.RegNumber} - Видеоролик"
            };

            // вывод ViewModel'и в новом окне
            Show(vm);
        }

        // вывод фото самолета в новом окне
        private void ExecuteShowAirplanePhotoWindow(object o)
        {
            // создание ViewModel'и для отображения фото
            var vm = new PhotoDialogViewModel(DataService.GetAirplanePhotoUri(Airplane.PhotoFileName))
            {
                Title = $"{Airplane.Name} - {Airplane.RegNumber} - Фото"
            };

            // вывод ViewModel'и в новом окне
            Show(vm);
        }

        #endregion
    }
}
