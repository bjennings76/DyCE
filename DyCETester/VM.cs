using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Threading;
using DyCE;

namespace DyCETester
{
    public class VM : INotifyPropertyChanged 
    {
        private static VM _instance;
        public static VM Instance { get { return _instance ?? (_instance = new VM()); } }

        private DyCEBag _bag;
        public DyCEBag Bag { get { return _bag ?? (_bag = new DyCEBag()); } }

        private static readonly DispatcherTimer _timer = new DispatcherTimer();

        private readonly ObservableCollection<ResultBase> _results = new ObservableCollection<ResultBase>();
        public ObservableCollection<ResultBase> Results { get { return _results; } }

        public EngineBase SelectedEngine { get; set; }

        public bool Paused { get; set; }

        public VM()
        {
            _timer.Interval = TimeSpan.FromSeconds(0.5);
            _timer.Tick += _timer_Tick;
            _timer.Start();
            Bag.DyCEList.CollectionChanged += DyCEList_CollectionChanged;
        }

        void DyCEList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems.Count > 0 && e.NewItems[0] is EngineBase)
                SelectedEngine = (EngineBase) e.NewItems[0];
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            if (SelectedEngine != null && SelectedEngine.CanRun && !Paused)
                Results.Add(SelectedEngine.Go(new Random().Next()));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
