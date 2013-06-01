using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace DyCE
{
    public class EngineRef : EngineBase
    {
        private string _refID;
        [XmlAttribute]
        public string RefID
        {
            get { return _refID; } 
            set
            {
                _refID = value;
                RaisePropertyChanged(() => RefID);
                //TrackEngineChanges();
                RaisePropertyChanged(() => SubEngine);
                RaisePropertyChanged(() => SubEngines);
                RaiseEngineChanged();
            }
        }

        public EngineBase SubEngine { get { return DyCEBag.GetEngine(RefID); } }

        public override IEnumerable<EngineBase> SubEngines { get { return new[] {SubEngine}; } }

        public override ResultBase Go(int seed) { return SubEngine.Go(seed); }

        private bool _trackingChanges;
        private void TrackEngineChanges()
        {
            if (SubEngine != null)
            {
                if (!_trackingChanges)
                {
                    SubEngine.SubscribeToChange(() => SubEngine.ID, IDUpdated);
                    _trackingChanges = true;
                    DB.Instance["General"].DyCEList.CollectionChanged -= DyCEListOnCollectionChanged;
                    DB.Instance.Loaded -= DBLoaded;
                }
            }
            else
            {
                var bag = DB.Instance["General"];

                if (bag == null)
                {
                    DB.Instance.Loaded += DBLoaded;
                    return;
                }

                DB.Instance["General"].DyCEList.CollectionChanged += DyCEListOnCollectionChanged;
            }
        }

        private void DBLoaded(object sender, EventArgs eventArgs) { TrackEngineChanges(); }

        private void DyCEListOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (SubEngine != null)
            {
                TrackEngineChanges();
                DB.Instance["General"].DyCEList.CollectionChanged -= DyCEListOnCollectionChanged;
            }
        }

        private void IDUpdated(EngineBase sender) { RefID = sender.ID; }

        public EngineRef()
        {
        }

        public EngineRef(string engineID) : this()
        {
            RefID = engineID;
            TrackEngineChanges();
        }

        public override string ToString()
        {
            return SubEngine + " Ref";
        }

        public override void Add(object item) { throw new NotImplementedException(); }
    }
}