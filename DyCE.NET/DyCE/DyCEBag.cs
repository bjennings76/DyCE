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
    public class DyCEBag : ViewModelBase
    {
        private static readonly DyCEBag _instance = new DyCEBag();
        public static DyCEBag Instance { get { return _instance; } }

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

        public static DyCEBag Load(FileInfo file)
        {
            var dyceBag = Utilities.LoadFromXML<DyCEBag>(file);
            dyceBag.File = file;
            return dyceBag;
        }

        public EngineBase this[string id] { get { return DyCEList.FirstOrDefault(e => e.ID == id); } }
		
		public DyCEBag()
		{
		    Creator = Environment.UserName + "@" + Environment.UserDomainName;
        }

        public RelayCommand AddEngineObjectCommand { get { return new RelayCommand(AddEngineObject);} }
        public void AddEngineObject() { DyCEList.Add(new EngineObject("New Object Engine")); }

        public RelayCommand AddEngineListCommand { get{ return new RelayCommand(AddEngineList);} }
        public void AddEngineList() { DyCEList.Add(new EngineList("New List Engine")); }

        public RelayCommand AddEngineTextCommand { get { return new RelayCommand(AddEngineText); } }
        public void AddEngineText() { DyCEList.Add(new EngineText("New Text Engine")); }

        [XmlIgnore]
        public RelayCommand SaveCommand { get { return new RelayCommand(Save); } }
        public void Save()
        {
            Utilities.SaveToXML(File, this);
            Process.Start(File.FullName);
        }

        public static EngineBase GetEngine(string id) { return Instance.DyCEList.FirstOrDefault(e => e.ID.Equals(id, StringComparison.OrdinalIgnoreCase)); }
        public static EngineBase GetSubEngineRef(EngineBase subEngine)
        {
            if ((subEngine is EngineRef) || string.IsNullOrWhiteSpace(subEngine.ID))
                return subEngine;

            return new EngineRef(subEngine.ID);
        }

        public static EngineRef GetSubEngineRef(string engineID) { return new EngineRef(engineID); }
    }
}
