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
                TrackEngineChanges();
                RaisePropertyChanged(() => SubEngine);
                RaisePropertyChanged(() => SubEngines);
            }
        }

        public EngineBase SubEngine { get { return DyCEBag.Instance[RefID]; } }

        public override IEnumerable<EngineBase> SubEngines { get { return new[] {SubEngine}; } }

        public override ResultBase Go(int seed) { return SubEngine.Go(seed); }

        public EngineRef()
        {
            TrackEngineChanges();
        }

        private bool _trackingChanges;
        private void TrackEngineChanges()
        {
            if (SubEngine != null)
            {
                if (!_trackingChanges)
                {
                    SubEngine.SubscribeToChange(() => SubEngine.ID, IDUpdated);
                    _trackingChanges = true;
                    DyCEBag.Instance.DyCEList.CollectionChanged -= DyCEListOnCollectionChanged;
                }
            }
            else
            {
                DyCEBag.Instance.DyCEList.CollectionChanged += DyCEListOnCollectionChanged;
            }
        }

        private void DyCEListOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (SubEngine != null)
            {
                TrackEngineChanges();
                DyCEBag.Instance.DyCEList.CollectionChanged -= DyCEListOnCollectionChanged;
            }
        }

        private void IDUpdated(EngineBase sender) { RefID = sender.ID; }

        public EngineRef(string engineID) { RefID = engineID; }

        public override string ToString()
        {
            return SubEngine + " Ref";
        }
    }
}