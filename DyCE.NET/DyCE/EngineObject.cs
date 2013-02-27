using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Command;

namespace DyCE
{
    public class EngineObject : EngineBase
    {
        public override System.Collections.Generic.IEnumerable<EngineBase> SubEngines { get { return Properties.Select(p => p.ValueEngine); } }
        private readonly ObservableCollection<EngineProperty> _properties = new ObservableCollection<EngineProperty>();
        public ObservableCollection<EngineProperty> Properties { get { return _properties; } }

        public EngineObject(string name, params EngineProperty[] properties)
        {
            Name = name;

            if (properties.Length == 0)
                properties = new[] {new EngineProperty("New Property")};

            _properties = new ObservableCollection<EngineProperty>(properties);
            CreatePropertyCommand = new RelayCommand(CreateEngineProperty);
        }

        public RelayCommand CreatePropertyCommand { get; private set; }

        public void CreateEngineProperty()
        {
            Properties.Add(new EngineProperty("New Property"));
        }

        public EngineBase this[string propertyName] { get { return Properties.FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase)); } }

        public override ResultBase Go(int seed) { return new ResultObject(this, seed); }
        public override string ToString() { return Name + " Engine: " + Properties.Select(p => p.ToString()).Aggregate((s1, s2) => s1 + ", " + s2); }
    }
}