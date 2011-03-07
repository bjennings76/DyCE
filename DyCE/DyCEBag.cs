using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections.ObjectModel;
using System;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Input;

namespace DynamicContent
{
    public class DyCEBag : INotifyPropertyChanged
    {
        #region Bindable Properties

        private ObservableCollection<DyCE> _dyceList = new ObservableCollection<DyCE>();
        public ObservableCollection<DyCE> DyCEList
        {
            get { return _dyceList; }
            set
            {
                _dyceList = value;
                foreach (DyCE engine in _dyceList)
                    engine.Bag = this;
                PropertyChanged.Notify(() => DyCEList);
            }
        }

        #endregion

        public DyCEBag()
        {
            DirectoryInfo here = new DirectoryInfo(@".\Engines\");

            if (!here.Exists)
                here.Create();

            List<FileInfo> engineFiles = new List<FileInfo>();
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
            return DyCEList.ToList().Find(e => e.ID == new Guid(engineID)).Go(r); 
        }

        internal string GoDetails(string engineID) { return GoDetails(engineID, new Random()); }
        internal string GoDetails(string engineID, Random r) 
        { 
            return DyCEList.ToList().Find(e => e.ID == new Guid(engineID)).GoDetails(r); 
        }

        internal void RenameEngine(DyCE dyce, string newName)
        {
            FileInfo newFile = new FileInfo(@".\Engines\" + newName + ".xml");

            if (dyce.File == null)
                dyce.File = newFile;
            else
            {
                try
                {
                    dyce.File.MoveTo(newFile.FullName);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error saving file : " + e.Message);
                }
            }
        }

        private static void SaveEngine(DyCE engine)
        {
            if (engine.Name != null)
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(DyCE));
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
        }

        private static DyCE LoadEngine(FileInfo file, DyCEBag bag)
        {
            try
            {
                if (!file.Exists)
                    throw new Exception("File not found: " + file.FullName);

                XmlSerializer deserializer = new XmlSerializer(typeof(DyCE));
                TextReader textReader = new StreamReader(file.FullName);
                DyCE engine = (DyCE)deserializer.Deserialize(textReader);
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

        public DyCE this[string ID] { get { return this[new Guid(ID)]; } }
        public DyCE this[Guid ID] { get { return GetDyCEngine(ID); } }

        private DyCE GetDyCEngine(Guid engineID)
        {
            return DyCEList.ToList().Find(e => e.ID == engineID);
        }

        #region ICommands


        private bool CanCreateEngine = true;
        private void CreateEngine() { DyCEList.Add(new DyCE("New Engine", this)); }

        RelayCommand _createEngineCommand;
        public ICommand CreateEngineCommand
        {
            get
            {
                if (_createEngineCommand == null)
                    _createEngineCommand = new RelayCommand(param => this.CreateEngine(), param => this.CanCreateEngine);

                return _createEngineCommand;
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
}
