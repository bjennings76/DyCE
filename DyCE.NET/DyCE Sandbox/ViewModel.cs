using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using DyCE;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

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
        public string WindowName { get { return SelectedEngine == null ? "DyCE Editor" : "DyCE Editor: " + SelectedEngine.Name; } }

        public static DyCEBag Bag { get { return DyCEBag.Instance; } }

        private static readonly DispatcherTimer _timer = new DispatcherTimer();

        public double TimerDuration
        {
            get { return _timer.Interval.TotalSeconds; } 
            set
            {
                _timer.Interval = TimeSpan.FromSeconds(value);
                RaisePropertyChanged(() => TimerDuration);
            }
        }

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

        private bool _paused;
        public bool Paused
        {
            get { return _paused; } 
            set
            {
                _paused = value;
                RaisePropertyChanged(() => Paused);
            }
        }

        /// <summary>
        /// Initializes a new instance of the ViewModel class.
        /// </summary>
        public ViewModel()
        {
            _timer.Interval = TimeSpan.FromSeconds(0.5);
            _timer.Tick += _timer_Tick;
            _timer.Start();

            PausePreviewCommand = new RelayCommand(PausePreview, CanPausePreview);
        }

        public RelayCommand PausePreviewCommand { get; private set; }

        public bool CanPausePreview() { return SelectedEngine != null; }

        public void PausePreview() { Paused = !Paused; }

        void _timer_Tick(object sender, EventArgs e)
        {
            if (SelectedEngine != null && SelectedEngine.CanRun && !Paused)
            {
                Results.Add(SelectedEngine.Go(new Random().Next()));

                if (Results.Count > 100)
                    Results.RemoveAt(0);
            }
        }

    }
}