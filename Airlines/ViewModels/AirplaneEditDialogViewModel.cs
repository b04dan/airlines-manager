using System.IO;
using System.Windows.Input;
using Airlines.Models;
using Airlines.Services;
using Airlines.Utils;
using MaterialDesignThemes.Wpf;

namespace Airlines.ViewModels
{
    class AirplaneEditDialogViewModel : ViewModelBase
    {
        public AirplaneEditDialogViewModel(IDialogService dialogService, IApplicationDataService dataService, Airplane airplane)
        {
            Airplane = airplane;
            DialogService = dialogService;
            DataService = dataService;
        }

        #region Properties

        private IDialogService DialogService { get; }
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

        // выбор нового фото самолета
        private RelayCommand _selectNewAirplanePhotoCommand;
        public ICommand SelectNewAirplanePhotoCommand
            => _selectNewAirplanePhotoCommand ?? (_selectNewAirplanePhotoCommand = new RelayCommand(ExecuteSelectNewAirplanePhoto));

        // выбор нового видео самолета
        private RelayCommand _selectNewAirplaneVideoCommand;
        public ICommand SelectNewAirplaneVideoCommand
            => _selectNewAirplaneVideoCommand ?? (_selectNewAirplaneVideoCommand = new RelayCommand(ExecuteSelectNewAirplaneVideo));

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

            // отображение ViewModel'и
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

        // выбор нового фото самолета
        private async void ExecuteSelectNewAirplanePhoto(object o)
        {
            // запуск диалога для выбора файла
            if (!DialogService.OpenFileDialog("Изображения (*.png, *.jpg)|*.png;*.jpg")) return;

            // запрос на подтверждение
            var result = await DialogHost.Show(
                $"Файл \"{Path.GetFileName(DialogService.FilePath)}\" будет скопирован в папку приложения.\nВы уверены?",
                "ConfirmationDialog");

            // выход, если подтверждения не было
            if (result == null || result is bool resBool && !resBool) return;

            // сохранение заданного файла
            DataService.SaveAirplanePhotoFrom(DialogService.FilePath);

            // запись нового имени файла
            Airplane.PhotoFileName = Path.GetFileName(DialogService.FilePath);
        }

        // выбор нового видео самолета
        private async void ExecuteSelectNewAirplaneVideo(object o)
        {
            // запуск диалога для выбора файла
            if (!DialogService.OpenFileDialog("Видеоролик  (*.mpg, *.mp4)|*.mpg;*.mp4")) return;

            // запрос на подтверждение
            var result = await DialogHost.Show(
                $"Файл \"{Path.GetFileName(DialogService.FilePath)}\" будет скопирован в папку приложения.\nВы уверены?",
                "ConfirmationDialog");

            // выход, если подтверждения не было
            if (result == null || result is bool resBool && !resBool) return;

            // сохранение заданного файла
            DataService.SaveAirplaneVideoFrom(DialogService.FilePath);

            // запись нового имени файла
            Airplane.VideoFileName = Path.GetFileName(DialogService.FilePath);
        }

        #endregion
    }
}
