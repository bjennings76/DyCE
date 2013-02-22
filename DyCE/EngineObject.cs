using System.Collections.ObjectModel;
using System.Linq;

namespace DyCE
{
    public class EngineObject : EngineBase
    {
        private readonly ObservableCollection<EngineProperty> _properties = new ObservableCollection<EngineProperty>();
        public ObservableCollection<EngineProperty> Properties { get { return _properties; } }

        public EngineObject(string name, params EngineProperty[] properties)
        {
            Name = name;
            _properties = new ObservableCollection<EngineProperty>(properties);
        }

        public override ResultBase Go(int seed) { return new ResultObject(this, seed); }
        public override string ToString() { return Name + " Engine: " + Properties.Select(p => p.ToString()).Aggregate((s1, s2) => s1 + ", " + s2); }
    }
}