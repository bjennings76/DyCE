using System.Collections.Generic;
using System.Xml.Serialization;

namespace DyCE
{
    public class EngineProperty : EngineBase
    {
        private string _engineRef;
        [XmlAttribute]
        public string EngineRef
        {
            get { return ValueEngine.ID; }
            set
            {
                _engineRef = value;
                RaisePropertyChanged(() => EngineRef);
                RaisePropertyChanged(() => ValueEngine);
            }
        }

        [XmlIgnore]
        public EngineBase ValueEngine { get { return DyCEBag.GetEngine(_engineRef); } set { EngineRef = value.ID; } }

        public override IEnumerable<EngineBase> SubEngines { get { return new[] {ValueEngine}; } }

        public EngineProperty(string name) : this(name, new EngineText("New Property Value")) { }
        public EngineProperty(string name, string value) : this(name, new EngineText(value)) { }
        public EngineProperty(string name, EngineBase valueEngine) : base(name) { ValueEngine = valueEngine; }
        public EngineProperty() { }

        public override ResultBase Go(int seed) { return new ResultProperty(this, seed); }

        public override string ToString()
        {
            return Name + " Property";
        }
    }
}