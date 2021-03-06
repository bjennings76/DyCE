using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DyCE;
using DyCE.Editor.Properties;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DyCE.Editor
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModel : ViewModelBase
    {
        public string WindowName { get { return SelectedEngine == null ? "DyCE Editor" : "DyCE Editor: " + SelectedEngine.DisplayName; } }

        private static readonly DispatcherTimer _timer = new DispatcherTimer();
        private static readonly Random _random = new Random();

        public double TimerDuration
        {
            get { return _timer.Interval.TotalSeconds; } 
            set
            {
                _timer.Interval = TimeSpan.FromSeconds(value);
                RaisePropertyChanged(() => TimerDuration);
            }
        }

        private int _maxResults = 10;
        public int MaxResults
        {
            get { return _maxResults; }
            set
            {
                _maxResults = value;
                RaisePropertyChanged(() => MaxResults);

                lock (this)
                {
                    while (Results.Count > MaxResults)
                        Results.RemoveAt(0);

                    while (Results.Count < MaxResults)
                        Results.Add(SelectedEngine.Go(_random.Next()));

                    UpdateResults();
                }
            }
        }

        public IEnumerable<DyCEBag> DyCEBags { get { return DB.Instance.DyCEBags; } }

        private DyCEBag _bag;
        public DyCEBag Bag
        {
            get { return _bag ?? (_bag = GetDefaultDyCEBag()); }
            set
            {
                _bag = value;
                RaisePropertyChanged(() => Bag);

                if (_bag == null)
                    return;

                SelectedEngine = _bag.DyCEList.FirstOrDefault();
                Settings.Default.CurrentDyCEBag = _bag.ID;
            }
        }

        private DyCEBag GetDefaultDyCEBag()
        {
            DyCEBag bag;

            if (Settings.Default.CurrentDyCEBag.IsNullOrEmpty())
                bag = DyCEBags.FirstOrDefault();
            else
                bag = DyCEBags.FirstOrDefault(b => b.ID == Settings.Default.CurrentDyCEBag) ?? DyCEBags.FirstOrDefault();

            if (bag != null)
                SelectedEngine = bag.DyCEList.FirstOrDefault();

            return bag;
        }

        private readonly ObservableCollection<ResultBase> _results = new ObservableCollection<ResultBase>();
        public ObservableCollection<ResultBase> Results { get { return _results; } }

        private string _resultsHTML;
        public string ResultsHTML
        {
            get
            {
                if (_resultsHTML == null)
                    UpdateResults();

                return _resultsHTML;
            }
        }

        private EngineBase _selectedEngine;
        public EngineBase SelectedEngine
        {
            get { return _selectedEngine; } 
            set
            {
                if (_selectedEngine == value)
                    return;

                if (_selectedEngine != null)
                {
                    _selectedEngine.Changed -= SelectedEngineOnChanged;
                    _selectedEngine.IsSelected = false;
                }
                _selectedEngine = value;
                RaisePropertyChanged(() => SelectedEngine);
                RaisePropertyChanged(() => WindowName);
                Results.Clear();

                if (_selectedEngine == null) return;

                Results.Add(SelectedEngine.Go(new Random().Next()));
                _selectedEngine.Changed += SelectedEngineOnChanged;
                _selectedEngine.IsSelected = true;
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
            _saveSettingsTimer.Interval = TimeSpan.FromSeconds(2);
            _saveSettingsTimer.Tick += SaveSettingsTimerOnTick;
            Results.CollectionChanged += Results_CollectionChanged;
            Settings.Default.PropertyChanged += DefaultOnPropertyChanged;
        }

        private static readonly DispatcherTimer _saveSettingsTimer = new DispatcherTimer();
        private static void DefaultOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs) { _saveSettingsTimer.Start(); }

        private static void SaveSettingsTimerOnTick(object sender, EventArgs eventArgs)
        {
            Settings.Default.Save();
            Console.WriteLine("Settings saved.");
            _saveSettingsTimer.Stop();
        }

        void Results_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) { UpdateResults(); }

        private CancellationTokenSource _cancellationTokenSource;
        private void UpdateResults()
        {
            if (_cancellationTokenSource != null) 
                _cancellationTokenSource.Cancel();

            _cancellationTokenSource = new CancellationTokenSource();

            var token = _cancellationTokenSource.Token;

            Task.Factory.StartNew<string>(GetHTMLResult, token).ContinueWith(task =>
            {
                if (token.IsCancellationRequested)
                    return;

                _resultsHTML = task.Result;
                RaisePropertyChanged(() => ResultsHTML);
            }, token);
        }

        private string GetHTMLResult()
        {
            var results = new List<ResultBase>();
            lock (this)
            {
                results.AddRange(Results);
            }
            return results.Count == 0 ? "" : results.Select(r => string.Concat("<p>", r.ToString(), "</p>")).JoinToString();
        }

        private void SelectedEngineOnChanged(object sender, EventArgs eventArgs) { UpdateResults(); }


        public RelayCommand PausePreviewCommand { get { return new RelayCommand(() => Paused = !Paused, () => SelectedEngine != null); } }

        public RelayCommand<EngineBase> SelectEngineCommand { get { return new RelayCommand<EngineBase>(e => SelectedEngine = e); } } 

        void _timer_Tick(object sender, EventArgs e)
        {
            if (SelectedEngine == null || Paused) 
                return;

            lock (this)
            {
                if (Results.Count >= MaxResults)
                    Results.RemoveAt(0);

                Results.Add(SelectedEngine.Go(new Random().Next()));
            }

            UpdateResults();
        }

    }
}
