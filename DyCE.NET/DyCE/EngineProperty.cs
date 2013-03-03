using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace DyCE
{
    public class EngineProperty : EngineBase
    {
        /// <summary>
        /// The actual sub-engine regardless of reference or anonymous state.
        /// </summary>
        [XmlIgnore]
        public EngineBase SubEngineActual { get; set; }

        /// <summary>
        /// Derived from SubEngine. This is only saved if the engine is non-ref. (anonymous)
        /// </summary>
        [XmlElement("Engine")]
        public EngineBase SubEngineSaved { get { return SubEngineActual is EngineRef ? null : SubEngineActual; } set { SubEngineActual = value; } }

        /// <summary>
        /// The pass-through engine for the UI to use so that the
        /// </summary>
        [XmlIgnore]
        public EngineBase SubEngine { get { return SubEngineActual is EngineRef ? SubEngineActual.SubEngines.FirstOrDefault() : SubEngineActual; } set { SubEngineActual = DyCEBag.GetSubEngineRef(value); } }

        /// <summary>
        /// The reference ID as an attribute if the sub-engine is a referenced engine.
        /// </summary>
        [XmlAttribute]
        public string RefID
        {
            get
            {
                var refEngine = SubEngineActual as EngineRef;
                return refEngine == null ? null : refEngine.RefID;
            }
            set { SubEngineActual = DyCEBag.GetSubEngineRef(value); }
        }

        [XmlAttribute]
        public override string ID { get { return null; } set { base.ID = value; } }

        [XmlAttribute("Name")]
        public override string NameSaved { get { return Name; } set { Name = value; } }

        public override IEnumerable<EngineBase> SubEngines { get { return new[] { SubEngine }; } }

        public EngineProperty(string name) 
            : this(name, new EngineText("New Property Value")) { }

        public EngineProperty(string name, string value) 
            : this(name, new EngineText(value)) { }

        public EngineProperty(string name, EngineBase subEngine)
            : base(name)
        {
            SubEngine = DyCEBag.GetSubEngineRef(subEngine);
        }

        public EngineProperty() { }

        public override ResultBase Go(int seed) { return new ResultProperty(this, seed); }

        public override string ToString()
        {
            return Name + " Property";
        }
    }
}