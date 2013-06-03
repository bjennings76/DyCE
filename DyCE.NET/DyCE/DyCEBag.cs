using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Collections.ObjectModel;
using System;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DyCE
{
    /// <summary>
    /// A collection of dynamic content engines.
    /// </summary>
    public class DyCEBag : ViewModelBase
    {
        /// <summary>
        /// Internal variable that holds the DyCEBag's name.
        /// </summary>
        private string _name = "General";

        //TODO: Separate the concept of ID from Name like it works in a DyCEBag object.

        /// <summary>
        /// The name of the DyCEBag.
        /// </summary>
        [XmlAttribute]
        public string Name
        {
            get { return _name; } 
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        /// <summary>
        /// Name of the DyCEBag's creator/owner which will eventually be used as a 'namespace' to handle DyCEBags created by different people but hold the same name/ID.
        /// </summary>
        [XmlAttribute]
        public string Creator { get; set; }

        /// <summary>
        /// The internal list of dynamic content engines.
        /// </summary>
        private ObservableCollection<EngineBase> _dyceList = new ObservableCollection<EngineBase>();

        /// <summary>
        /// The list of dynamic content engines.
        /// </summary>
        [XmlElement("Engine")]
        public ObservableCollection<EngineBase> DyCEList
        {
            get { return _dyceList; }
            set
            {
                _dyceList = value;
                RaisePropertyChanged(() => DyCEList);
            }
        }

        /// <summary>
        /// The internal file info of the DyCEBag's .xml file.
        /// </summary>
        private FileInfo _file;

        /// <summary>
        /// The file info of the DyCEBag's .xml file.
        /// </summary>
        [XmlIgnore]
        public FileInfo File { get { return _file ?? (_file = new FileInfo(Path.Combine("Engines", Name + ".xml"))); } set { _file = value; } }

        /// <summary>
        /// This indexer returns the dynamic content engine matching the supplied ID.
        /// </summary>
        /// <param name="id">The ID of the requested dynamic content engine.</param>
        /// <returns>The dynamic content engine that matches the supplied ID.</returns>
        public EngineBase this[string id] { get { return DyCEList.FirstOrDefault(e => e.ID == id); } }

        /// <summary>
        /// Creates a new DyCEBag object, adding the Creator's name. (Currently the computer's user name and domain name.)
        /// </summary>
        public DyCEBag() { Creator = Environment.UserName + "@" + Environment.UserDomainName; }

        /// <summary>
        /// Adds the supplied engine to the DyCEBag's DyCEList.
        /// </summary>
        /// <param name="newEngine">The engine to add to the DyCEBag.</param>
        public void Add(EngineBase newEngine) { DyCEList.Add(newEngine); }

        /// <summary>
        /// Adds a new Object Engine to the DyCEBag's DyCEList.
        /// </summary>
        public RelayCommand AddEngineObjectCommand { get { return new RelayCommand(() => DyCEList.Add(new EngineObject("New Object Engine"))); } }

        /// <summary>
        /// Adds a new List Engine to the DyCEBag's DyCEList.
        /// </summary>
        public RelayCommand AddEngineListCommand { get { return new RelayCommand(() => DyCEList.Add(new EngineList("New List Engine"))); } }

        /// <summary>
        /// Adds a new Text Engine to the DyCEBag's DyCEList.
        /// </summary>
        public RelayCommand AddEngineTextCommand { get { return new RelayCommand(() => DyCEList.Add(new EngineText("New Text Value", "New Text Engine"))); } }

        /// <summary>
        /// Adds a new Number Engine to the DyCEBag's DyCEList.
        /// </summary>
        public RelayCommand AddEngineNumberCommand { get { return new RelayCommand(() => DyCEList.Add(new EngineNumber("New Number Engine"))); } }

        /// <summary>
        /// Saves the DyCEBag to it's .xml file.
        /// </summary>
        public RelayCommand SaveCommand { get { return new RelayCommand(Save); } }

        /// <summary>
        /// Used by the 'Save' command to save the DyCEBag to it's .xml file.
        /// </summary>
        private void Save()
        {
            Utilities.SaveToXML(File, this);
            Process.Start(File.FullName);
        }

        /// <summary>
        /// Loads a DyCEBag from the given .xml file.
        /// </summary>
        /// <param name="file">The file containing the serialized DyCEBag object.</param>
        /// <returns>Returns the loaded DyCEBag object.</returns>
        public static DyCEBag Load(FileInfo file)
        {
            var dyceBag = Utilities.LoadFromXML<DyCEBag>(file);
            dyceBag.File = file;
            return dyceBag;
        }

        /// <summary>
        /// Gets a new sub-engine reference object using the supplied sub-engine.
        /// </summary>
        /// <param name="subEngine">The sub-engine to create the reference from.</param>
        /// <returns>The referenced sub-engine or, if it's an anonymous reference, the original engine.</returns>
        public static EngineBase GetSubEngineRef(EngineBase subEngine)
        {
            if ((subEngine is EngineRef) || string.IsNullOrWhiteSpace(subEngine.ID))
                return subEngine;

            return new EngineRef(subEngine.ID);
        }

        /// <summary>
        /// Gets a new sub-engine reference object using the supplied sub-engine ID.
        /// </summary>
        /// <param name="engineID">ID of the sub-engine to reference.</param>
        /// <returns>The referenced sub-engine.</returns>
        public static EngineRef GetSubEngineRef(string engineID) { return new EngineRef(engineID); }

        /// <summary>
        /// Gets the engine from the supplied engine ID.
        /// </summary>
        /// <param name="engineID">ID of the engine to get.</param>
        /// <returns>The engine that matches the supplied engine ID.</returns>
        public static EngineBase GetEngine(string engineID)
        {
            //TODO: Don't just use the 'General' DyCEBag. This should support references to other DyCEBags as well.
            var bag = DB.Instance["General"];

            if (bag == null)
                return null;

            var engine = bag[engineID];

            if (engine == null)
                throw new Exception("Could not find engine " + engineID);

            return engine;
        }
    }
}
