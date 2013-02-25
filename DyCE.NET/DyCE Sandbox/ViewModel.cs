using DyCE;
using GalaSoft.MvvmLight;

namespace DyCE_Sandbox
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModel : ViewModelBase
    {
        public string WindowName
        {
            get
            {
                return DyCEBag.Instance.SelectedEngine == null ? "DyCE Editor" : "DyCE Editor: " + DyCEBag.Instance.SelectedEngine.Name;
            }
        }


        /// <summary>
        /// Initializes a new instance of the ViewModel class.
        /// </summary>
        public ViewModel()
        {
            DyCEBag.Instance.SubscribeToChange(() => DyCEBag.Instance.SelectedEngine, SelectedEngineChanged);
        }
        
        private void SelectedEngineChanged(DyCEBag sender) { RaisePropertyChanged(() => WindowName); }

    }
}