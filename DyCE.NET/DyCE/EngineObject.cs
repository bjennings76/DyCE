using System;
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
            newProperty.IsSelected = true;
            SelectedSubEngine = newProperty;
        }

        public RelayCommand<EngineProperty> DeleteCommand { get { return new RelayCommand<EngineProperty>(DeleteProperty); } }
        public void DeleteProperty(EngineProperty engine) { Properties.Remove(engine); }

        public EngineBase this[string propertyName] { get { return Properties.FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase)); } }

        public override ResultBase Go(int seed) { return new ResultObject(this, seed); }
        public override string ToString() { return Name + " Object"; }
    }
}