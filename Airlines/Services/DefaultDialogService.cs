using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace Airlines.Services
{
    // сервис для работы с диалогами
    public class DefaultDialogService : IDialogService
    {
        public string FilePath { get; protected set; }

        public bool OpenFileDialog(string filter = null)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (!string.IsNullOrWhiteSpace(filter)) openFileDialog.Filter = filter;

            if (openFileDialog.ShowDialog() != true) return false;

            FilePath = openFileDialog.FileName;
            return true;
        }

        public bool SaveFileDialog(string filter = null)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (!string.IsNullOrWhiteSpace(filter)) saveFileDialog.Filter = filter;

            if (saveFileDialog.ShowDialog() != true) return false;

            FilePath = saveFileDialog.FileName;
            return true;
        }
    }
}
