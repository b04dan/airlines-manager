using System.Windows.Input;
using Airlines.Utils;

namespace Airlines.ViewModels
{
    class AboutProgramDialogViewModel : ViewModelBase
    {

        #region Commands

        private RelayCommand _openLinkInBrowserCommand;
        public ICommand OpenLinkInBrowserCommand =>
            _openLinkInBrowserCommand ?? (_openLinkInBrowserCommand =
                new RelayCommand(o =>
                {
                    if (o is string link) Link.OpenInBrowser(link);
                }));

        #endregion
    }
}
