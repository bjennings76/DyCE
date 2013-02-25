using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
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
                return SelectedEngine == null ? "DyCE Editor" : "DyCE Editor: " + SelectedEngine.Name;
            }
        }


        private DyCEBag _bag;
        public DyCEBag Bag { get { return _bag ?? (_bag = new DyCEBag()); } }

        private static readonly DispatcherTimer _timer = new DispatcherTimer();

        private readonly ObservableCollection<ResultBase> _results = new ObservableCollection<ResultBase>();
        private EngineBase _selectedEngine;
        public ObservableCollection<ResultBase> Results { get { return _results; } }

        public EngineBase SelectedEngine
        {
            get { return _selectedEngine; } 
            set
            {
                _selectedEngine = value;
                RaisePropertyChanged(() => SelectedEngine);
                RaisePropertyChanged(() => WindowName);
                Results.Clear();
            }
        }

        public bool Paused { get; set; }

        /// <summary>
        /// Initializes a new instance of the ViewModel class.
        /// </summary>
        public ViewModel()
        {
            _timer.Interval = TimeSpan.FromSeconds(0.5);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            if (SelectedEngine != null && SelectedEngine.CanRun && !Paused)
                Results.Add(SelectedEngine.Go(new Random().Next()));
        }

    }
}