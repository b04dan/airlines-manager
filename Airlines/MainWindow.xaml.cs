using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Airlines.Services;
using Airlines.Styles;
using Airlines.ViewModels;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;


namespace Airlines
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            MaterialDesignWindow.RegisterCommands(this);
            InitializeComponent();

            
            // рабочая папка
            var baseDirectory = Environment.CurrentDirectory; ;

            // сервис для работы с данными приложения
            var dataService = new DefaultApplicationDataService(
                Path.Combine(baseDirectory, "Data", "Airplanes"),
                Path.Combine(baseDirectory, "Data", "Videos"),
                Path.Combine(baseDirectory, "Data", "Managers"),
                Path.Combine(baseDirectory, "Data"));

            // основная модель представления
            var mainViewModel = new ApplicationViewModel(new DefaultDialogService(), dataService);

            // обработчик события закрытия окна
            Closing += mainViewModel.OnWindowClosing;

            // запись модели представления в контекст окна
            DataContext = mainViewModel;
        }
    }
}
