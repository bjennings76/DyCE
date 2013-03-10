using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using GalaSoft.MvvmLight.Command;

namespace DyCE
{
    public class EngineObject : EngineBase
    {
        private readonly ObservableCollection<EngineProperty> _properties = new ObservableCollection<EngineProperty>();
        [XmlElement("Property")]
        public ObservableCollection<EngineProperty> Properties { get { return _properties; } }

        public override System.Collections.Generic.IEnumerable<EngineBase> SubEngines { get { return Properties; } }

        public EngineObject(string name, params EngineProperty[] properties)
            : base(name)
        {
            _properties = new ObservableCollection<EngineProperty>(properties);
        }

        public EngineObject() { }

        public RelayCommand CreatePropertyCommand { get { return new RelayCommand(CreateProperty); } }
        public void CreateProperty()
        {
            var newProperty = new EngineProperty("New Property");
            Properties.Add(newProperty);
            SelectedSubEngine = newProperty;
        }

        public RelayCommand<EngineProperty> DeleteCommand { get { return new RelayCommand<EngineProperty>(DeleteProperty); } }
        public void DeleteProperty(EngineProperty engine) { Properties.Remove(engine); }

        public RelayCommand AddEngineObjectCommand { get { return new RelayCommand(AddEngineObject); } }
        public void AddEngineObject() {
            var newEngine = new EngineObject("New Object Engine");
            DB.Instance["General"].Add(newEngine);
            Add(newEngine); 
        }

        public RelayCommand AddEngineListCommand { get { return new RelayCommand(AddEngineList); } }
        public void AddEngineList() { Add(new EngineList()); }

        public RelayCommand AddEngineTextCommand { get { return new RelayCommand(AddEngineText); } }
        public void AddEngineText() { Add(new EngineText("New Text Value")); }

        public override void Add(object item)
        {
            if (item is EngineBase)
                Properties.Add(new EngineProperty("New Property", item as EngineBase));
            else if (item is string)
                Properties.Add(new EngineProperty("New Property", item as string));
            else if (item is IEnumerable<object>)
                Properties.Add(new EngineProperty("New Property", new EngineList(item as IEnumerable<object>)));
            else
                throw new Exception("Unknown item type: " + item);
        }

        public EngineBase this[string propertyName] { get { return Properties.FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase)); } }

        public override ResultBase Go(int seed) { return new ResultObject(this, seed); }
        public override string ToString() { return Name + " Object"; }
    }
}