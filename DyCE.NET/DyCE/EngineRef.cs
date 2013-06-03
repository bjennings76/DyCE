using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace DyCE
{
    public class EngineRef : EngineBase
    {
        /// <summary>
        /// Internal engine reference ID.
        /// </summary>
        private string _refID;

        /// <summary>
        /// The referenced engine's ID.
        /// </summary>
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

        /// <summary>
        /// The referenced sub-engine.
        /// </summary>
        public EngineBase SubEngine { get { return DyCEBag.GetEngine(RefID); } }

        protected override string _resultTemplateDefault { get { return "$this.Name$ Ref"; } }

        /// <summary>
        /// List of referenced sub-engines. In this case, the one referenced engine.
        /// </summary>
        public override IEnumerable<EngineBase> SubEngines { get { return new[] {SubEngine}; } }

        /// <summary>
        /// Creates a new empty Engine Reference object.
        /// </summary>
        public EngineRef() { }

        /// <summary>
        /// Creates a new Engine Reference object using the supplied engine ID.
        /// </summary>
        /// <param name="engineID">The ID of the referenced engine.</param>
        public EngineRef(string engineID) : this()
        {
            RefID = engineID;
            TrackEngineChanges();
        }

        /// <summary>
        /// Indicates whether or not the referenced engine's changes are being tracked.
        /// </summary>
        private bool _trackingChanges;

        /// <summary>
        /// Triggers after the DyCEBag has loaded, then tracks the sub-engine's changes.
        /// </summary>
        /// <param name="sender">DyCE database that has finished loading.</param>
        /// <param name="eventArgs">Event args.</param>
        private void DBLoaded(object sender, EventArgs eventArgs) { TrackEngineChanges(); }

        /// <summary>
        /// Triggers after the default DyCEBag list changes.
        /// </summary>
        /// <param name="sender">The DyCEBag that has updated it's list.</param>
        /// <param name="notifyCollectionChangedEventArgs">Changed collection event args.</param>
        private void DyCEListOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (SubEngine == null) return;
            TrackEngineChanges();

            //TODO: Don't just use the 'General' DyCEBag. This should support references to other DyCEBags as well.
            DB.Instance["General"].DyCEList.CollectionChanged -= DyCEListOnCollectionChanged;
        }

        /// <summary>
        /// Function that starts tracking the referenced engine's changes if they aren't already tracked.
        /// </summary>
        private void TrackEngineChanges()
        {
            // If the referenced engine isn't loaded already, then wait for the DyCEBag to load first.
            if (SubEngine == null)
            {
                //TODO: Don't just use the 'General' DyCEBag. This should support references to other DyCEBags as well.
                var bag = DB.Instance["General"];

                if (bag == null)
                {
                    DB.Instance.Loaded += DBLoaded;
                    return;
                }

                //TODO: Don't just use the 'General' DyCEBag. This should support references to other DyCEBags as well.
                DB.Instance["General"].DyCEList.CollectionChanged += DyCEListOnCollectionChanged;
                return;
            }

            // If changes are not being tracked and we have a SubEngine, then start tracking subengine changes and stop listening for the load.
            if (_trackingChanges) return;

            SubEngine.SubscribeToChange(() => SubEngine.ID, sender => RefID = sender.ID);
            _trackingChanges = true;

            //TODO: Don't just use the 'General' DyCEBag. This should support references to other DyCEBags as well.
            DB.Instance["General"].DyCEList.CollectionChanged -= DyCEListOnCollectionChanged;
            DB.Instance.Loaded -= DBLoaded;
        }

        /// <summary>
        /// Returns the referenced engine's Engine Result based on the supplied seed number.
        /// </summary>
        /// <param name="seed">The seed number which will allow the engine to repeatedly return the same 'random' result.</param>
        /// <returns>The referenced engine's result based on the seed number supplied.</returns>
        public override ResultBase Go(int seed) { return SubEngine.Go(seed); }

        /// <summary>
        /// Gets the display name of the Engine Reference.
        /// </summary>
        /// <returns>The display name of the Engine Reference.</returns>
        public override string ToString() { return SubEngine + " Ref"; }
    }
}