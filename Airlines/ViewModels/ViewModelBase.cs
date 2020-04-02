using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Airlines.Views;

namespace Airlines.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> 
        /// Устанавливает новое значение заданному полю и зажигает событие PropertyChanged
        /// </summary>
        /// <typeparam name="T">Тип поля</typeparam>
        /// <param name="field">Ссылка на поле</param>
        /// <param name="newValue">Новое значение</param>
        /// <param name="propertyName">Название свойства</param>
        /// <returns>false - если переданное значение равно текущему значению поля, иначе - true</returns>
        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName]string propertyName = null)
        {
            // если переданное значение равно текущему - возвращает false
            if (EqualityComparer<T>.Default.Equals(field, newValue)) return false;

            // присваивание ссылки на новое значение
            field = newValue;

            // зажигание события и возврат true
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary> 
        /// Зажигает событие PropertyChanged
        /// </summary>
        /// <param name="propertyName">Название поля</param>
        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        // Окно в котором показывается текущий ViewModel
        private ChildWindow _wnd;

        // Метод показа ViewModel в окне
        // Переданная ViewModel выводится в новый ChildWindow.
        // Связь между нужной ViewModel и View задана в App.xaml
        // Способ взят отсюда: http://losev-al.blogspot.com/2015/12/view-mvvm-2.html
        protected void Show(ViewModelBase viewModel)
        {
            viewModel._wnd = new ChildWindow { DataContext = viewModel };
            viewModel._wnd.Closed += (sender, e) => viewModel.Closed();
            viewModel._wnd.Show();
        }

        // Заголовок окна
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        // Метод, вызываемый окном при закрытии
        protected virtual void Closed() { }

        // Метод, вызываемый для закрытия окна связанного с ViewModel
        public bool Close()
        {
            var result = false;
            if (_wnd != null)
            {
                _wnd.Close();
                _wnd = null;
                result = true;
            }
            return result;
        }

        // Метод для разворачивания окна
        protected void MaximizeWindow()
        {
            if (_wnd == null) return;
            _wnd.WindowStyle = WindowStyle.None;
            _wnd.WindowState = WindowState.Maximized;
        }

        // Метод для сворачивания окна
        protected void NormalizeWindow()
        {
            if (_wnd == null) return;
            _wnd.WindowStyle = WindowStyle.SingleBorderWindow;
            _wnd.WindowState = WindowState.Normal;
        }
    }
}
