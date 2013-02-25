using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections.ObjectModel;
using System;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Input;
using GalaSoft.MvvmLight;

namespace DyCE
{
    public class DyCEBag : ViewModelBase
    {
        private static readonly DyCEBag _instance = new DyCEBag();
        public static DyCEBag Instance { get { return _instance; } }

        #region Bindable Properties

        private ObservableCollection<EngineBase> _dyceList = new ObservableCollection<EngineBase>();
        public ObservableCollection<EngineBase> DyCEList
        {
            get { return _dyceList; }
            set
            {
                _dyceList = value;
                foreach (var engine in _dyceList)
                    engine.Bag = this;
                RaisePropertyChanged(() => DyCEList);
            }
        }

        private EngineBase _selectedEngine;
        public EngineBase SelectedEngine
        {
            get { return _selectedEngine; } 
            set
            {
                _selectedEngine = value;
                RaisePropertyChanged(() => SelectedEngine);
            }
        }

        #endregion

        public DyCEBag()
        {
            var here = new DirectoryInfo(@".\Engines\");

            if (!here.Exists)
                here.Create();

            var engineFiles = new List<FileInfo>();
            engineFiles.AddRange(here.GetFiles("*.xml"));
            foreach (FileInfo file in engineFiles)
            {
                try
                {
                    DyCEList.Add(LoadEngine(file, this));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Can't load " + file.FullName + " (" + e.Message + ")");
                }
            }
        }

        internal string Go(string engineID) { return Go(engineID, new Random()); }
        internal string Go(string engineID, Random r) 
        { 
            return DyCEList.ToList().Find(e => e.ID == new Guid(engineID)).Go(r.Next()).ToString();
        }

        internal string GoDetails(string engineID) { return GoDetails(engineID, new Random()); }
        internal string GoDetails(string engineID, Random r) 
        { 
            return DyCEList.ToList().Find(e => e.ID == new Guid(engineID)).Go(r.Next()).ToString(); 
        }

        private static void SaveEngine(EngineBase engine)
        {
            if (engine.Name == null) return;

            try
            {
                var serializer = new XmlSerializer(typeof(EngineBase));
                //XmlSerializer serializer = new XmlSerializer(typeof(Container), new Type[] { typeof(TestInherited) });
                TextWriter textWriter = new StreamWriter(engine.File.FullName);
                serializer.Serialize(textWriter, engine);
                textWriter.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error saving engine: " + e.Message);
            }
        }

        private static EngineBase LoadEngine(FileInfo file, DyCEBag bag)
        {
            try
            {
                if (!file.Exists)
                    throw new Exception("File not found: " + file.FullName);

                var deserializer = new XmlSerializer(typeof(EngineBase));
                TextReader textReader = new StreamReader(file.FullName);
                var engine = (EngineBase)deserializer.Deserialize(textReader);
                textReader.Close();

                engine.File = file;
                engine.Bag = bag;

                return engine;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to save out " + file.Name + ". Reason: " + e.Message);
                return null;
            }
        }

        public EngineBase this[string id] { get { return this[new Guid(id)]; } }
        public EngineBase this[Guid id] { get { return GetDyCEngine(id); } }

        private EngineBase GetDyCEngine(Guid engineID)
        {
            return DyCEList.ToList().Find(e => e.ID == engineID);
        }

        #region ICommands

        private const bool CanCreateEngine = true;
        private void CreateEngine() { DyCEList.Add(new EngineList("New Engine")); }

        RelayCommand _createEngineCommand;

        public ICommand CreateEngineCommand
        {
            get {
                return _createEngineCommand ??
                       (_createEngineCommand = new RelayCommand(param => CreateEngine(), param => CanCreateEngine));
            }
        }

        #endregion
    }
}
