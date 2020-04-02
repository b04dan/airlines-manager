using System;
using System.Windows.Input;
using Airlines.Utils;

namespace Airlines.ViewModels
{
    class PhotoDialogViewModel : ViewModelBase
    {
        public PhotoDialogViewModel(Uri photoSource)
        {
            PhotoSource = photoSource;
        }

        #region Properties

        // путь к файлу
        private Uri _photoSource;
        public Uri PhotoSource
        {
            get => _photoSource;
            set => SetProperty(ref _photoSource, value);
        }

        // открыта ли панель инструментов
        private bool _isDashboardVisible;
        public bool IsDashboardVisible
        {
            get => _isDashboardVisible;
            set => SetProperty(ref _isDashboardVisible, value);
        }

        // открыто ли окно на весь экран
        private bool _isMaximized;
        public bool IsMaximized
        {
            get => _isMaximized;
            set
            {
                if (value) MaximizeWindow();
                else NormalizeWindow();
                SetProperty(ref _isMaximized, value);
            }
        }

        #endregion

        #region Commands

        // команда разворачивания и сворачивания окна
        private RelayCommand _maximizeMinimizeWindowCommand;
        public ICommand MaximizeMinimizeWindowCommand =>
            _maximizeMinimizeWindowCommand ?? (_maximizeMinimizeWindowCommand =
                new RelayCommand(o => { IsMaximized = !IsMaximized; }));

        // команда отображения и скрытия панели инструментов
        private RelayCommand _showHideDashboardCommand;
        public ICommand ShowHideDashboardCommand =>
            _showHideDashboardCommand ?? (_showHideDashboardCommand =
                new RelayCommand(o => { IsDashboardVisible = !IsDashboardVisible; }));

        #endregion
    }
}
