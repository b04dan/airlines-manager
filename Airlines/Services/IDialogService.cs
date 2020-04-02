using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airlines.Services {

    // сервис для работы с диалогами
    public interface IDialogService 
    {
        string FilePath { get; }       // путь к выбранному файлу
        bool OpenFileDialog(string filter);  // открытие файла
        bool SaveFileDialog(string filter);  // сохранение файла
    }
}
