using System.Windows.Input;
using Airlines.Models;
using Airlines.Services;
using Airlines.Utils;

namespace Airlines.ViewModels
{
    class AirlineDetailsDialogViewModel : ViewModelBase
    {
        public AirlineDetailsDialogViewModel(IApplicationDataService dataService, Airline airline)
        {
            Airline = airline;
            DataService = dataService;
        }

        #region Properties

        private IApplicationDataService DataService { get; }

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

            // вывод
            Show(vm);
        }

        #endregion
    }
}
