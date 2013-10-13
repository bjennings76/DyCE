using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace DyCE
{
    /// <summary>
    /// The DyCEBag Database.
    /// </summary>
    public class DB
    {
        /// <summary>
        /// Internal variable that holds the single static instance of the DyCEBag Database.
        /// </summary>
        private static readonly DB _instance = new DB();

        /// <summary>
        /// The single static instance of the DyCEBag Database.
        /// </summary>
        public static DB Instance { get { return _instance; } }

        public bool IsLoaded { get { return _dyCEBags != null; } }

        /// <summary>
        /// The internal list of DyCEBags.
        /// </summary>
        private ObservableCollection<DyCEBag> _dyCEBags;

        /// <summary>
        /// The list of DyCEBags.
        /// </summary>
        public ObservableCollection<DyCEBag> DyCEBags
        {
            get
            {
                if (_dyCEBags == null)
                {
                    _dyCEBags = LoadDyCEBags();
                    RaiseLoaded();
                }

                return _dyCEBags;
            }
        }

        /// <summary>
        /// Function that adds a DyCEBag to the DyCEBag list.
        /// </summary>
        /// <param name="bag">The DyCEBag object to be added.</param>
        public void Add(DyCEBag bag) { DyCEBags.Add(bag); }

        #region Loaded [event]

        /// <summary>
        /// Returns true when the DyCEBag list is currently being loaded. Used for concurrance conflicts.
        /// </summary>
        private bool _loading;

        /// <summary>
        /// Event that fires when the DyCEBag list is done loading.
        /// </summary>
        public event EventHandler Loaded;

        /// <summary>
        /// Raises the Loaded event after the DyCEBag list is done loading.
        /// </summary>
        private void RaiseLoaded()
        {
            EventHandler handler = Loaded;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// Loads the DyCEBag list from the Engines folder.
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<DyCEBag> LoadDyCEBags()
        {
            _loading = true;
            var engineDirectory = new DirectoryInfo("Engines");

            if (!engineDirectory.Exists) 
                return null;

            var files = engineDirectory.GetFiles("*.xml");
            var db = new ObservableCollection<DyCEBag>();

            foreach (var engine in files.Select(DyCEBag.Load).Where(engine => engine != null))
                db.Add(engine);

            _loading = false;
            return db;
        }

        /// <summary>
        /// Indexer that supplies the DyCEBag by ID.
        /// </summary>
        /// <param name="id">The ID of the DyCEBag requested.</param>
        /// <returns>Returns the DyCEBag object matching the supplied ID or null if it's not found or the list is still loading.</returns>
        public DyCEBag this[string id] { get { return _loading ? null : DyCEBags.FirstOrDefault(b => b.Name == id); } }
    }
}
