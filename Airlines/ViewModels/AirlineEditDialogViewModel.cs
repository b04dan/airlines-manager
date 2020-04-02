using System.IO;
using System.Windows.Input;
using Airlines.Models;
using Airlines.Services;
using Airlines.Utils;
using MaterialDesignThemes.Wpf;

namespace Airlines.ViewModels
{
    class AirlineEditDialogViewModel : ViewModelBase
    {
        public AirlineEditDialogViewModel(IDialogService dialogService, IApplicationDataService dataService, Airline airline)
        {
            Airline = airline;
            DataService = dataService;
            DialogService = dialogService;
        }

        #region Properties

        private IApplicationDataService DataService { get; }
        private IDialogService DialogService { get; }

        private Airline _airplane;
        public Airline Airline
        {
            get => _airplane;
            set => SetProperty(ref _airplane, value);
        }

        #endregion

        #region Commands

        // вывод фото менеджера в новом окне
        private RelayCommand _showManagerPhotoWindowCommand;
        public ICommand ShowManagerPhotoWindowCommand
            => _showManagerPhotoWindowCommand ?? (_showManagerPhotoWindowCommand = new RelayCommand(ExecuteShowManagerPhotoWindow,
                o => Airline.Manager?.PhotoFileName != null));

        // выбор нового фото менеджера
        private RelayCommand _selectNewManagerPhotoCommand;
        public ICommand SelectNewManagerPhotoCommand
            => _selectNewManagerPhotoCommand ?? (_selectNewManagerPhotoCommand = new RelayCommand(ExecuteSelectNewManagerPhoto));

        #endregion

        #region Methods

        // вывод фото менеджера в новом окне
        private void ExecuteShowManagerPhotoWindow(object o)
        {
            // создание ViewModel'и для отображения фото
            var vm = new PhotoDialogViewModel(DataService.GetManagerPhotoUri(Airline.Manager.PhotoFileName))
            {
                Title = $"{Airline.Manager.Surname} {Airline.Manager.Name} {Airline.Manager.Patronymic}"
            };

            // вывод ViewModel'и в новом окне
            Show(vm);
        }

        // выбор нового фото менеджера
        private async void ExecuteSelectNewManagerPhoto(object o)
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
            DataService.SaveManagerPhotoFrom(DialogService.FilePath);

            // запись нового имени файла
            Airline.Manager.PhotoFileName = Path.GetFileName(DialogService.FilePath);
        }

        #endregion
    }
}
