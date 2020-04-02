using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Airlines.Utils;

namespace Airlines.ViewModels
{
    public class VideoDialogViewModel : ViewModelBase
    {
        public VideoDialogViewModel(Uri videoSource)
        {
            IsLoaderVisible = true;

            MediaElementObject = new MediaElement
            {
                Source = videoSource,
                LoadedBehavior = MediaState.Manual,
                Stretch = Stretch.UniformToFill,
                ScrubbingEnabled = true
            };


            DispatcherTimer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(50)};
            DispatcherTimer.Tick += (sender, args) =>
            {
                if (MediaElementObject != null && !_userIsDraggingSlider)
                    PositionSliderValue = MediaElementObject.Position.TotalSeconds;
            };

            MediaElementObject.MediaOpened +=
                (sender, args) =>
                {
                    IsDashboardVisible = true;
                    IsLoaderVisible = false;
                    VideoDuration = MediaElementObject.NaturalDuration.TimeSpan.TotalSeconds;
                    DispatcherTimer.Start();
                };

            MediaElementObject.MediaEnded += (sender, args) =>
            {
                MediaElementObject.Position = TimeSpan.Zero;
                MediaElementObject.Play();
            };
            
            // запуск воспроизведения
            IsPlaying = true;
        }

        #region Properties

        // MediaElement создается в коде, для доступа к нему
        // Не совсем правильно с точки зрения MVVM. Нужен сервис
        // TODO: переделать
        public MediaElement MediaElementObject { get; private set; } 
        public DispatcherTimer DispatcherTimer { get; private set; }

        // перетаскивает ли пользователь слайдер в данный момент
        private bool _userIsDraggingSlider;

        // позиция слайдера 
        private double _positionSliderValue;
        public double PositionSliderValue
        {
            get => _positionSliderValue;
            set => SetProperty(ref _positionSliderValue, value);
        }

        // продолжительность видео
        private double _videoDuration;
        public double VideoDuration
        {
            get => _videoDuration;
            set => SetProperty(ref _videoDuration, value);
        }

        // идет ли воспроизведение видео
        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                if (value) MediaElementObject.Play();
                else MediaElementObject.Pause();
                SetProperty(ref _isPlaying, value);
            }
        }

        // открыта ли панель управления
        private bool _isDashboardVisible;
        public bool IsDashboardVisible
        {
            get => _isDashboardVisible;
            set => SetProperty(ref _isDashboardVisible, value);
        }

        // отображается ли анимация загрузки
        private bool _isLoaderVisible;
        public bool IsLoaderVisible
        {
            get => _isLoaderVisible;
            set => SetProperty(ref _isLoaderVisible, value);
        }

        // открыто ли видео на весь экран
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

        // прокрутить видео на заданное кол-во секунд
        // отрицательное число - назад, положительное - вперед
        private RelayCommand _scrollPlaybackCommand;
        public ICommand ScrollPlaybackCommand =>
            _scrollPlaybackCommand ?? (_scrollPlaybackCommand =
                new RelayCommand(o => ScrollPlayback(int.Parse(o.ToString())), CanExecute));

        // пользователь начал перетаскивать слайдер
        private RelayCommand _positionDragStartedCommand;
        public ICommand PositionDragStartedCommand =>
            _positionDragStartedCommand ?? (_positionDragStartedCommand =
                new RelayCommand(o => _userIsDraggingSlider = true, CanExecute));

        // пользователь закончил перетаскивать слайдер
        private RelayCommand _positionDragCompletedCommand;
        public ICommand PositionDragCompletedCommand =>
            _positionDragCompletedCommand ?? (_positionDragCompletedCommand =
                new RelayCommand(o =>
                {
                    _userIsDraggingSlider = false;
                    MediaElementObject.Position = TimeSpan.FromSeconds(PositionSliderValue);
                }, CanExecute));

        // команда разворачивания и сворачивания окна
        private RelayCommand _maximizeMinimizeWindowCommand;
        public ICommand MaximizeMinimizeWindowCommand =>
            _maximizeMinimizeWindowCommand ?? (_maximizeMinimizeWindowCommand =
                new RelayCommand(o => { IsMaximized = !IsMaximized; }));

        // команда запуска и остановки видео
        private RelayCommand _playPauseCommand;
        public ICommand PlayPauseCommand =>
            _playPauseCommand ?? (_playPauseCommand =
                new RelayCommand(o => { IsPlaying = !IsPlaying; }, CanExecute));

        // команда отображения и скрытия панели инструментов
        private RelayCommand _showHideDashboardCommand;
        public ICommand ShowHideDashboardCommand =>
            _showHideDashboardCommand ?? (_showHideDashboardCommand =
                new RelayCommand(o => { IsDashboardVisible = !IsDashboardVisible; }, CanExecute));

        #endregion

        #region Methods

        // возможно ли выполнение команд
        private bool CanExecute(object obj)
            => MediaElementObject != null && MediaElementObject.IsLoaded;
        
        // прокрутить вперед/назад
        private void ScrollPlayback(int seconds)
            => MediaElementObject.Position = TimeSpan.FromSeconds(MediaElementObject.Position.Seconds + seconds);
        
        // пререопределение метода, который вызовется при закрытии окна 
        protected override void Closed()
        {
            if (MediaElementObject != null)
            {
                DispatcherTimer.Stop();
                MediaElementObject.Stop();
                MediaElementObject.Close();
                MediaElementObject = null;
            }
            base.Closed();
        }

        #endregion
    }
}
