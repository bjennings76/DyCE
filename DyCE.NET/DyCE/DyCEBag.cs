﻿using System.Collections.Generic;
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
    public class DB
    {
        private static readonly DB _instance = new DB();
        public static DB Instance { get { return _instance; } }

        private ObservableCollection<DyCEBag> _dyCEBags;
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

        public void Add(DyCEBag bag) { DyCEBags.Add(bag); }


        private bool _loading;

        #region Loaded [event]

        public event EventHandler Loaded;

        public void RaiseLoaded()
        {
            EventHandler handler = Loaded;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion

        private ObservableCollection<DyCEBag> LoadDyCEBags()
        {
            _loading = true;
            var engineDirectory = new DirectoryInfo("Engines");

            if (!engineDirectory.Exists) 
                return null;

            var files = engineDirectory.GetFiles("*.xml");
            var db = new ObservableCollection<DyCEBag>();

            foreach (var file in files)
            {
                var engine = DyCEBag.Load(file);

                if (engine == null) 
                    continue;

                db.Add(engine);
            }
            _loading = false;
            return db;
        }

        public DyCEBag this[string id] { get { return _loading ? null : DyCEBags.FirstOrDefault(b => b.Name == id); } }
    }

    public class DyCEBag : ViewModelBase
    {
        private string _name = "General";
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

        [XmlAttribute]
        public string Creator { get; set; }

        private ObservableCollection<EngineBase> _dyceList = new ObservableCollection<EngineBase>();

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

        private FileInfo _file;
        [XmlIgnore]
        public FileInfo File { get { return _file ?? (_file = new FileInfo(Path.Combine("Engines", Name + ".xml"))); } set { _file = value; } }

        public EngineBase this[string id] { get { return DyCEList.FirstOrDefault(e => e.ID == id); } }
		
		public DyCEBag()
		{
		    Creator = Environment.UserName + "@" + Environment.UserDomainName;
        }

        public RelayCommand AddEngineObjectCommand { get { return new RelayCommand(AddEngineObject);} }

        public void AddEngineObject()
        {
            Add(new EngineObject("New Object Engine"));
        }

        public EngineBase Add(EngineBase newEngine)
        {
            DyCEList.Add(newEngine);
            return newEngine;
        }

        public RelayCommand AddEngineListCommand { get{ return new RelayCommand(AddEngineList);} }
        public void AddEngineList() { DyCEList.Add(new EngineList("New List Engine")); }

        public RelayCommand AddEngineTextCommand { get { return new RelayCommand(AddEngineText); } }
        public void AddEngineText() { DyCEList.Add(new EngineText("New Text Value", "New Text Engine")); }

        public RelayCommand SaveCommand { get { return new RelayCommand(Save); } }
        public void Save()
        {
            Utilities.SaveToXML(File, this);
            Process.Start(File.FullName);
        }

        public static DyCEBag Load(FileInfo file)
        {
            var dyceBag = Utilities.LoadFromXML<DyCEBag>(file);
            dyceBag.File = file;
            return dyceBag;
        }

        public static EngineBase GetSubEngineRef(EngineBase subEngine)
        {
            if ((subEngine is EngineRef) || string.IsNullOrWhiteSpace(subEngine.ID))
                return subEngine;

            return new EngineRef(subEngine.ID);
        }

        public static EngineRef GetSubEngineRef(string engineID) { return new EngineRef(engineID); }

        public static EngineBase GetEngine(string engineID)
        {
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
