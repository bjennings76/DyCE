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
        public ObservableCollection<EngineProperty> Properties { get { return _properties; } }

        public override System.Collections.Generic.IEnumerable<EngineBase> SubEngines { get { return Properties; } }

        public EngineObject(string name, params EngineProperty[] properties)
            : base(name)
        {
            if (properties.Length == 0)
                properties = new[] {new EngineProperty("New Property")};

            _properties = new ObservableCollection<EngineProperty>(properties);

            CreatePropertyCommand = new RelayCommand(CreateProperty);
            DeletePropertyCommand = new RelayCommand(DeleteProperty, CanDeleteProperty);
        }

        public EngineObject() { }

        [XmlIgnore]
        public RelayCommand CreatePropertyCommand { get; private set; }
        public void CreateProperty() { Properties.Add(new EngineProperty("New Property")); }

        [XmlIgnore]
        public RelayCommand DeletePropertyCommand { get; private set; }
        public bool CanDeleteProperty() { return Properties.Any(p => p.IsSelected); }
        public void DeleteProperty()
        {
            var selectedProperties = Properties.Where(p => p.IsSelected).ToList();
            if (selectedProperties.Count > 0)
                selectedProperties.ForEach(p => Properties.Remove(p));
        }

        public EngineBase this[string propertyName] { get { return Properties.FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase)); } }

        public override ResultBase Go(int seed) { return new ResultObject(this, seed); }
        public override string ToString() { return Name + " Object"; }
    }
}