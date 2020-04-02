using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Airlines.Models;
using Airlines.Services;
using Airlines.Utils;
using MaterialDesignThemes.Wpf;

namespace Airlines.ViewModels
{
    class ApplicationViewModel : ViewModelBase
    {
        // название файла, в который сохраняются данные
        private const string AirlinesFileName = "airlines.json";

        public ApplicationViewModel(IDialogService dialogService, IApplicationDataService applicationDataService)
        {
            // сервисы
            DialogService = dialogService;
            DataService = applicationDataService;


            // загрузка данных
            Airlines = DataService.UploadAirlinesFromDataFolder(AirlinesFileName);

            // если данных нет - они генерируются
            if (Airlines == null || Airlines.Count < 1)
            {
                Airlines = new ObservableCollection<Airline>();
                for (int i = 0; i < 5; i++)
                    Airlines.Add(Airline.Generate(Utils.Generators.GetRand(5, 20)));

                DataService.SaveAirlinesToDataFolder(AirlinesFileName, Airlines);
            }
            
            // выбранная авиакомпания
            SelectedAirline = Airlines[0];

            // загрузка данных в словари с названиями полей
            SortedFields = new Dictionary<string, string>(Airplane.FilteredFields);
            FilteredFields = new Dictionary<string, string>(Airplane.FilteredFields);

            // удаление полей, по которым не может идти фильтрация
            FilteredFields.Remove("Name");
            FilteredFields.Remove("RegNumber");

            // добавление варианта "Не сортировать"
            SortedFields.Add("NoSorting", "Не сортировать");
            SelectedSortField = "NoSorting";
            SelectedFilterField = FilteredFields.Keys.FirstOrDefault();

            // заполнение списка представлений коллекций самолетов
            FillAirplanesViewsCollection();

            // светлая тема по умолчанию
            IsLightThemeEnabled = true;
        }

        // заполнение списка представлений коллекций самолетов
        private void FillAirplanesViewsCollection()
        {
            AirplanesViewsCollection = new List<ICollectionView>();
            foreach (var airline in Airlines)
            {
                var view = CollectionViewSource.GetDefaultView(airline.Airplanes);
                view.Filter = AirplanesFilter;
                AirplanesViewsCollection.Add(view);
            }
        }

        #region Properties

        // была ли коллекция обновлена во время работы приложения
        private bool _airlinesCollectionUpdated;

        // сервисы
        public IDialogService DialogService { get; }
        public IApplicationDataService DataService { get; }

        // коллекция авиакомпаний
        private ObservableCollection<Airline> _airlines;
        public ObservableCollection<Airline> Airlines
        {
            get => _airlines;
            set => SetProperty(ref _airlines, value);
        }

        // представления коллекций самолетов
        // используются для фильтрации, сортировки и группировки выводимых данных
        public List<ICollectionView> AirplanesViewsCollection { get; set; }

        // выбранный самолет
        private Airplane _selectedAirplane;
        public Airplane SelectedAirplane
        {
            get => _selectedAirplane;
            set => SetProperty(ref _selectedAirplane, value);
        }

        // выбранная компания
        private Airline _selectedAirline;
        public Airline SelectedAirline
        {
            get => _selectedAirline;
            set => SetProperty(ref _selectedAirline, value);
        }

        // открыт ли PopupBox с настройками фильтра
        private bool _isFilterPopupBoxOpen;
        public bool IsFilterPopupBoxOpen
        {
            get => _isFilterPopupBoxOpen;
            set => SetProperty(ref _isFilterPopupBoxOpen, value);
        }

        // открыт ли PopupBox с настройками сортировки
        private bool _isSortPopupBoxOpen;
        public bool IsSortPopupBoxOpen
        {
            get => _isSortPopupBoxOpen;
            set => SetProperty(ref _isSortPopupBoxOpen, value);
        }

        // открыт ли PopupBox с настройками сортировки
        private bool _isResultPopupBoxOpen;
        public bool IsResultPopupBoxOpen
        {
            get => _isResultPopupBoxOpen;
            set => SetProperty(ref _isResultPopupBoxOpen, value);
        }

        // открыт ли основной диалог
        private bool _isRootDialogOpened;
        public bool IsRootDialogOpened
        {
            get => _isRootDialogOpened;
            set => SetProperty(ref _isRootDialogOpened, value);
        }

        // начальное значение фильтрации
        private double? _filterFromValue;
        public double? FilterFromValue
        {
            get => _filterFromValue;
            set => SetProperty(ref _filterFromValue, value);
        }

        // конечное значение фильтрации
        private double? _filterToValue;
        public double? FilterToValue
        {
            get => _filterToValue;
            set => SetProperty(ref _filterToValue, value);
        }

        // параметр фильтрации, определяющий, исключать ли самолеты без видео
        private bool _filterIsExcludeWithoutVideo;
        public bool FilterIsExcludeWithoutVideo
        {
            get => _filterIsExcludeWithoutVideo;
            set => SetProperty(ref _filterIsExcludeWithoutVideo, value);
        }

        // параметр фильтрации, определяющий, исключать ли самолеты без фото
        private bool _filterIsExcludeWithoutPhoto;
        public bool FilterIsExcludeWithoutPhoto
        {
            get => _filterIsExcludeWithoutPhoto;
            set => SetProperty(ref _filterIsExcludeWithoutPhoto, value);
        }

        // словарь полей, по которым может идти фильтрация
        private Dictionary<string, string> _filteredFields;
        public Dictionary<string, string> FilteredFields
        {
            get => _filteredFields;
            set => SetProperty(ref _filteredFields, value);
        }

        // словарь полей, по которым может идти сортировка
        private Dictionary<string, string> _sortedFields;
        public Dictionary<string, string> SortedFields
        {
            get => _sortedFields;
            set => SetProperty(ref _sortedFields, value);
        }

        // название выбранного для фильтрации поля
        private string _selectedFilterField;
        public string SelectedFilterField
        {
            get => _selectedFilterField;
            set => SetProperty(ref _selectedFilterField, value);
        }

        // название выбранного для сортировки поля
        private string _selectedSortField;
        public string SelectedSortField
        {
            get => _selectedSortField;
            set => SetProperty(ref _selectedSortField, value);
        }

        // сортировка по убыванию или нет
        private bool _isDescendingSort;
        public bool IsDescendingSort
        {
            get => _isDescendingSort;
            set => SetProperty(ref _isDescendingSort, value);
        }

        // состояние окна
        private WindowState _windowState;
        public WindowState WindowState
        {
            get => _windowState;
            set => SetProperty(ref _windowState, value);
        }

        // светлая тема включена
        private bool _isLightThemeEnabled;
        public bool IsLightThemeEnabled
        {
            get => _isLightThemeEnabled;
            set => SetProperty(ref _isLightThemeEnabled, value);

        }

        // темная тема включена
        private bool _isDarkThemeEnabled;
        public bool IsDarkThemeEnabled
        {
            get => _isDarkThemeEnabled;
            set => SetProperty(ref _isDarkThemeEnabled, value);
        }

        #endregion

        #region Commands

        // вывод видео в новом окне
        private RelayCommand _showAirplaneVideoWindowCommand;
        public ICommand ShowAirplaneVideoWindowCommand
            => _showAirplaneVideoWindowCommand ?? (_showAirplaneVideoWindowCommand = new RelayCommand(ExecuteShowAirplaneVideoWindow));

        // изменение темы приложения
        private RelayCommand _changeThemeCommand;
        public ICommand ChangeThemeCommand 
            => _changeThemeCommand ?? (_changeThemeCommand = new RelayCommand(ExecuteChangeThemeCommand));

        // вывод диалога с подробной информацией о самолете
        private RelayCommand _showAirplaneDetailsDialogCommand;
        public ICommand ShowAirplaneDetailsDialogCommand 
            => _showAirplaneDetailsDialogCommand ?? (_showAirplaneDetailsDialogCommand = new RelayCommand(ExecuteAirplaneDetailsDialog));

        // вывод диалога с подробной информацией о авиакомпании
        private RelayCommand _showAirlineDetailsDialogCommand;
        public ICommand ShowAirlineDetailsDialogCommand 
            => _showAirlineDetailsDialogCommand ?? (_showAirlineDetailsDialogCommand = new RelayCommand(ExecuteAirlineDetailsDialog,
                   o => SelectedAirline != null));

        // вывод диалога с редактором самолета
        private RelayCommand _showAirplaneEditDialogCommand;
        public ICommand ShowAirplaneEditDialogCommand 
            => _showAirplaneEditDialogCommand ?? (_showAirplaneEditDialogCommand = new RelayCommand(ExecuteAirplaneEditDialog));

        // вывод диалога с редактором авиакомпании
        private RelayCommand _showAirlineEditDialogCommand;
        public ICommand ShowAirlineEditDialogCommand
            => _showAirlineEditDialogCommand ?? (_showAirlineEditDialogCommand = new RelayCommand(ExecuteAirlineEditDialog,
                   o => SelectedAirline != null));

        // вывод диалога с добавлением авиакомпании
        private RelayCommand _showAirlineAddDialogCommand;
        public ICommand ShowAirlineAddDialogCommand
            => _showAirlineAddDialogCommand ?? (_showAirlineAddDialogCommand = new RelayCommand(ExecuteAirlineAddDialog));

        // вывод диалога с добавлением самолета
        private RelayCommand _showAirplaneAddDialogCommand;
        public ICommand ShowAirplaneAddDialogCommand 
            => _showAirplaneAddDialogCommand ?? (_showAirplaneAddDialogCommand = new RelayCommand(ExecuteAirplaneAddDialog));

        // применение заданных настроек фильтрации
        private RelayCommand _filterCommand;
        public ICommand FilterCommand 
            => _filterCommand ?? (_filterCommand = new RelayCommand(ExecuteFilter));

        // сброс фильтра
        private RelayCommand _resetFilterCommand;
        public ICommand ResetFilterCommand 
            => _resetFilterCommand ?? (_resetFilterCommand = new RelayCommand(ExecuteResetFilter));

        // показать PopupBox с настройками фильтрации
        private RelayCommand _showFilterPopupBoxCommand;
        public ICommand ShowFilterPopupBoxCommand 
            => _showFilterPopupBoxCommand ?? (_showFilterPopupBoxCommand = new RelayCommand(o => IsFilterPopupBoxOpen = true));

        // показать PopupBox с настройками сортировки
        private RelayCommand _showSortPopupBoxCommand;
        public ICommand ShowSortPopupBoxCommand
            => _showSortPopupBoxCommand ?? (_showSortPopupBoxCommand = new RelayCommand(o => IsSortPopupBoxOpen = true));

        // показать PopupBox с настройками сортировки
        private RelayCommand _showResultPopupBoxCommand;
        public ICommand ShowResultPopupBoxCommand
            => _showResultPopupBoxCommand ?? (_showResultPopupBoxCommand = new RelayCommand(o => IsResultPopupBoxOpen = true));

        // применение настроек сортировки
        private RelayCommand _sortCommand;
        public ICommand SortCommand => _sortCommand ?? (_sortCommand = new RelayCommand(ExecuteSort));

        // сброс настроек сортировки
        private RelayCommand _resetSortCommand;
        public ICommand ResetSortCommand 
            => _resetSortCommand ?? (_resetSortCommand = new RelayCommand(ExecuteResetSort));

        // загрузить данные из файла
        private RelayCommand _uploadDataFromFileCommand;
        public ICommand UploadDataFromFileCommand 
            => _uploadDataFromFileCommand ?? (_uploadDataFromFileCommand = new RelayCommand(ExecuteUploadDataFromFile));

        // сохранить данные в файл
        private RelayCommand _saveDataToFileCommand;
        public ICommand SaveDataToFileCommand 
            => _saveDataToFileCommand ?? (_saveDataToFileCommand = new RelayCommand(ExecuteSaveDataToFile));

        // сохранить данные в выбранный файл
        private RelayCommand _saveDataToFileAsCommand;
        public ICommand SaveDataToFileAsCommand 
            => _saveDataToFileAsCommand ?? (_saveDataToFileAsCommand = new RelayCommand(ExecuteSaveDataToFileAs));

        // развертывание окна
        private RelayCommand _maximizeWindowCommand;
        public ICommand MaximizeWindowCommand 
            => _maximizeWindowCommand ?? (_maximizeWindowCommand = new RelayCommand(o => WindowState = WindowState.Maximized, 
                o => WindowState != WindowState.Maximized));

        // вывод диалога с информацией о программе
        private RelayCommand _showAboutProgramDialogCommand;
        public ICommand ShowAboutProgramDialogCommand 
            => _showAboutProgramDialogCommand ?? (_showAboutProgramDialogCommand = new RelayCommand(
                async o => await DialogHost.Show(new AboutProgramDialogViewModel(), "RootDialog")
        ));

        // удалить самолет
        private RelayCommand _removeAirplaneCommand;
        public ICommand RemoveAirplaneCommand 
            => _removeAirplaneCommand ?? (_removeAirplaneCommand = new RelayCommand(ExecuteRemoveAirplane));

        // удалить авиакомпанию
        private RelayCommand _removeAirlineCommand;
        public ICommand RemoveAirlineCommand => _removeAirlineCommand ?? (_removeAirlineCommand = new RelayCommand(ExecuteRemoveAirline, 
            o => SelectedAirline != null));

        #endregion

        #region Methods

        // вывод видео в новом окне
        private void ExecuteShowAirplaneVideoWindow(object o)
        {
            // путь к видео
            var uri = new Uri(Path.Combine(DataService.AirplanesVideoFolderPath, SelectedAirplane.VideoFileName), UriKind.Absolute);

            // создание ViewModel'и для отображения видео
            var vm = new VideoDialogViewModel(uri) { Title = $"{SelectedAirplane.Name} - {SelectedAirplane.RegNumber} - Видеоролик" };

            // вывод ViewModel'и в новом окне
            Show(vm);
        }

        // изменение темы приложения
        private void ExecuteChangeThemeCommand(object o)
        {
            if (!(o is bool isLight)) return;

            // модификация темы
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(isLight ? Theme.Light : Theme.Dark);
            paletteHelper.SetTheme(theme);

            IsLightThemeEnabled = isLight;
            IsDarkThemeEnabled = !isLight;
        }

        // вывод диалога с подробной информацией о самолете
        private async void ExecuteAirplaneDetailsDialog(object o)
        {
            if (!(o is Airplane airplane) || IsRootDialogOpened) return;

            // создание ViewModel'и для отображения подробной информации о самолете
            var vm = new AirplaneDetailsDialogViewModel(DataService, airplane);

            // вывод ViewModel'и в DialogHost
            var result = await DialogHost.Show(vm, "RootDialog");

            // получение результата и выполнение требуемой операции
            if (!(result is DetailsDialogResult)) return;
            switch ((DetailsDialogResult)result)
            {
                case DetailsDialogResult.Remove:
                    RemoveAirplaneCommand.Execute(airplane);
                    break;

                case DetailsDialogResult.Edit:
                    ShowAirplaneEditDialogCommand.Execute(airplane);
                    break;
            }
        }

        // вывод диалога с подробной информацией о авиакомпании
        private async void ExecuteAirlineDetailsDialog(object o)
        {
            if (!(o is Airline airline) || IsRootDialogOpened) return;

            // создание ViewModel'и для отображения подробной информации о авиакомпании
            var vm = new AirlineDetailsDialogViewModel(DataService, airline);

            var result = await DialogHost.Show(vm, "RootDialog");

            // получение результата и выполнение требуемой операции
            if (!(result is DetailsDialogResult)) return;
            switch ((DetailsDialogResult)result)
            {
                case DetailsDialogResult.Remove:
                    RemoveAirlineCommand.Execute(airline);
                    break;

                case DetailsDialogResult.Edit:
                    ShowAirlineEditDialogCommand.Execute(airline);
                    break;
            }
        }

        // вывод диалога с редактором самолета
        private async void ExecuteAirplaneEditDialog(object o)
        {
            if (!(o is Airplane airplane) || IsRootDialogOpened) return;

            // создание копии объекта для его изменения
            var airplaneClone = (Airplane)airplane.Clone();

            // создание ViewModel'и для редактирования информации о самолете
            var vm = new AirplaneEditDialogViewModel(DialogService, DataService, airplaneClone);

            // вывод ViewModel'и в DialogHost
            var result = await DialogHost.Show(vm, "RootDialog");

            // получение результата и выполнение требуемой операции
            if (!(result is EditDialogResult)) return;
            switch ((EditDialogResult)result)
            {
                case EditDialogResult.Save:
                    SelectedAirline.Airplanes[SelectedAirline.Airplanes.IndexOf(airplane)] = airplaneClone;
                    _airlinesCollectionUpdated = true;
                    break;
                case EditDialogResult.Remove:
                    RemoveAirplaneCommand.Execute(airplane);
                    _airlinesCollectionUpdated = true;
                    break;
            }
        }

        // вывод диалога с редактором авиакомпании
        private async void ExecuteAirlineEditDialog(object o)
        {
            // ссылка на редактируемую компанию
            if (!(o is Airline airline) || IsRootDialogOpened) return;

            // создание копии объекта для его изменения
            var airlineClone = airline.CloneWithoutAirplanes();

            // создание ViewModel'и для редактирования информации о авиакомпании
            var vm = new AirlineEditDialogViewModel(DialogService, DataService, airlineClone);

            // вывод ViewModel'и в DialogHost
            var result = await DialogHost.Show(vm, "RootDialog");

            // получение результата и выполнение требуемой операции
            if (!(result is EditDialogResult)) return;
            switch ((EditDialogResult)result)
            {
                case EditDialogResult.Save:
                    airlineClone.Airplanes = SelectedAirline.Airplanes;
                    Airlines[Airlines.IndexOf(SelectedAirline)] = airlineClone;
                    SelectedAirline = airlineClone;
                    _airlinesCollectionUpdated = true;
                    break;
                case EditDialogResult.Remove:
                    RemoveAirlineCommand.Execute(airline);
                    _airlinesCollectionUpdated = true;
                    break;
            }
        }

        // вывод диалога с добавлением авиакомпании
        private async void ExecuteAirlineAddDialog(object o)
        {
            if (IsRootDialogOpened) return;

            // создание новой авиакомпании
            // используется "костыль", чтобы сработала валидация
            // подробнее в ValidatableModelBase.cs
            var newAirline = new Airline()
            {
                Title = "",
                Address = "",
                Airplanes = new ObservableCollection<Airplane>(),
                Manager = new Manager()
                {
                    Surname = "",
                    Name = "",
                    Patronymic = "",
                    Email = "",
                    PhotoFileName = null,
                    PhoneNumber = ""
                }
            };

            // создание ViewModel'и для добавления авиакомпании
            var vm = new AirlineEditDialogViewModel(DialogService, DataService, newAirline);

            // вывод ViewModel'и в DialogHost
            var result = await DialogHost.Show(vm, "RootDialog");

            // получение результата и выполнение требуемой операции
            if (!(result is EditDialogResult)) return;
            switch ((EditDialogResult)result)
            {
                case EditDialogResult.Save:
                    Airlines.Add(newAirline);
                    SelectedAirline = newAirline;
                    var airplanesView = CollectionViewSource.GetDefaultView(newAirline.Airplanes);
                    airplanesView.Filter = AirplanesFilter;
                    AirplanesViewsCollection.Add(airplanesView);
                    _airlinesCollectionUpdated = true;
                    break;
                case EditDialogResult.Remove:
                    break;
                case EditDialogResult.Cancel:
                    break;
            }
        }

        // вывод диалога с добавлением самолета
        private async void ExecuteAirplaneAddDialog(object o)
        {
            if (IsRootDialogOpened) return;

            // создание нового самолета
            // используется "костыль", чтобы сработала валидация
            // подробнее в ValidatableModelBase.cs
            var newAirplane = new Airplane()
            {
                Name = "",
                PhotoFileName = null,
                VideoFileName = null,
                RegNumber = "",
                CruisingSpeed = 0,
                FlightRange = 0,
                FuelConsumption = 0,
                Height = 0,
                Length = 0,
                MaxFlightHeight = 0,
                MaxPassengersCount = 0,
                MaxTakeoffWeight = 0,
                Price = 0,
                Wingspan = 0
            };

            // создание ViewModel'и для добавления самолета
            var vm = new AirplaneEditDialogViewModel(DialogService, DataService, newAirplane);

            var result = await DialogHost.Show(vm, "RootDialog");

            // получение результата и выполнение требуемой операции
            if (!(result is EditDialogResult)) return;
            switch ((EditDialogResult)result)
            {
                case EditDialogResult.Save:
                    SelectedAirline.Airplanes.Add(newAirplane);
                    _airlinesCollectionUpdated = true;
                    break;
                case EditDialogResult.Remove:
                    break;
                case EditDialogResult.Cancel:
                    break;
            }
        }

        // применение заданных настроек фильтрации 
        private void ExecuteFilter(object o)
        {
            IsFilterPopupBoxOpen = false;

            // для каждого представления коллекции задается фильтр
            AirplanesViewsCollection.ForEach(view => view.Filter = AirplanesFilter);
        }

        // предикат-фильтр
        private bool AirplanesFilter(object obj)
        {
            var airplane = (Airplane)obj;

            // получение значения выбранного свойства с помощью рефлексии
            double value = Convert.ToDouble((typeof(Airplane).GetProperty(SelectedFilterField)?.GetValue(airplane)));

            // фильтрация
            // TODO: упростить
            return (FilterFromValue ?? double.MinValue) <= value &&
                   value <= (FilterToValue ?? double.MaxValue) &&
                   (!FilterIsExcludeWithoutVideo || airplane.VideoFileName != null) &&
                   (!FilterIsExcludeWithoutPhoto || airplane.PhotoFileName != null);
        }

        // сброс фильтра
        private void ExecuteResetFilter(object o)
        {
            IsFilterPopupBoxOpen = false;

            // сброс введенных значений
            FilterFromValue = FilterToValue = null;
            FilterIsExcludeWithoutPhoto = FilterIsExcludeWithoutVideo = false;
            SelectedFilterField = Airplane.FilteredFields.Keys.FirstOrDefault();

            // сброс фильтра для всех представлений
            AirplanesViewsCollection.ForEach(view => view.Filter = null);
        }

        // применение заданных настроек сортировки 
        private void ExecuteSort(object o)
        {
            IsSortPopupBoxOpen = false;

            // при выборе варианта не сортировать - настройки сортировки сбрасываются
            if (SelectedSortField == "NoSorting")
            {
                ResetSortCommand.Execute(null);
                return;
            }

            // добавление всем представлениям коллекций заданной сортировки
            AirplanesViewsCollection.ForEach(view =>
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription(SelectedSortField,
                    IsDescendingSort ? ListSortDirection.Descending : ListSortDirection.Ascending));
            });
        }

        // сброс сортировки
        private void ExecuteResetSort(object o)
        {
            IsSortPopupBoxOpen = false;
            SelectedSortField = "NoSorting";
            AirplanesViewsCollection.ForEach(view => view.SortDescriptions.Clear());
        }

        // загрузить данные из файла
        private async void ExecuteUploadDataFromFile(object o)
        {
            // запуск диалога для выбора файла
            if (!DialogService.OpenFileDialog("JSON-файл (*.json)|*.json")) return;

            // запрос подтверждения
            var confirm = await DialogHost.Show($"Вы уверены, что хотите загрузить данные из файла \"{Path.GetFileName(DialogService.FilePath)}\"?\nВнимание! Текущие данные будут утеряны!",
                "ConfirmationDialog");

            if (!(confirm is bool boolConfirm && boolConfirm)) return;

            // зашрузка данных из файла файла
            Airlines = DataService.UploadAirlines(DialogService.FilePath);
            if(Airlines == null || Airlines.Count < 1) return;

            // выбранная авиакомпания
            SelectedAirline = Airlines[0];

            // заполнение списка представлений коллекций самолетов
            FillAirplanesViewsCollection();
        }

        // сохранить данные в файл
        private void ExecuteSaveDataToFile(object o)
        {
            // сохранение данных в файл в папке приложения
            DataService.SaveAirlinesToDataFolder(AirlinesFileName, Airlines);
            _airlinesCollectionUpdated = false;
        }

        // сохранить данные в выбранный файл
        private void ExecuteSaveDataToFileAs(object o)
        {
            // запуск диалога для выбора файла
            if (!DialogService.SaveFileDialog("JSON-файл (*.json)|*.json")) return;

            // сохранение файла
            DataService.SaveAirlines(DialogService.FilePath, Airlines);
        }

        // удалить самолет
        private async void ExecuteRemoveAirplane(object o)
        {
            if (!(o is Airplane airplane)) return;

            // запрос подтверждения
            var confirm = await DialogHost.Show($"Вы уверены, что хотите удалить самолет с номером \"{airplane.RegNumber}\"?",
                "ConfirmationDialog");

            // если диалог вернул true - самолет удаляется
            if (confirm is bool boolConfirm && boolConfirm)
            {
                SelectedAirline.Airplanes.Remove(airplane);
                _airlinesCollectionUpdated = true;
            }
        }

        // удалить авиакомпанию
        private async void ExecuteRemoveAirline(object o)
        {
            if (!(o is Airline airline)) return;

            // запрос подтверждения
            var confirm = await DialogHost.Show($"Вы уверены, что хотите удалить авиакомпанию \"{airline.Title}\" со всеми ее самолетами?",
                "ConfirmationDialog");

            // если диалог вернул true - авиакомпания удаляется
            if (confirm is bool boolConfirm && boolConfirm)
            {
                Airlines.Remove(airline);
                SelectedAirline = Airlines.FirstOrDefault();
                _airlinesCollectionUpdated = true;
            }
        }

        // обработчик закрытия окна
        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if(!_airlinesCollectionUpdated) return;
            
            e.Cancel = true;
            // диалог с подтверждением удаления
            DialogHost.Show("Сохранить внесенные изменения в файл?",
                "ConfirmationDialog", delegate(object o, DialogClosingEventArgs args)
                {
                    if (!(args.Parameter is bool parameter)) return;

                    if(parameter)
                        DataService.SaveAirlinesToDataFolder(AirlinesFileName, Airlines);
                    Application.Current.Shutdown();
                });
        }

        #endregion
    }
}
