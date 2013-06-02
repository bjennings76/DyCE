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
        private EngineBase _subEngineActual;

        /// <summary>
        /// The actual sub-engine regardless of reference or anonymous state. Generally by the Property Result object to get this property's result.
        /// </summary>
        [XmlIgnore]
        public EngineBase SubEngineActual
        {
            get { return _subEngineActual; } 
            set
            {
                _subEngineActual = value;
                RaiseEngineChanged();
                RaisePropertyChanged(() => SubEngine);
                RaisePropertyChanged(() => SubEngines);
                RaisePropertyChanged(() => SubEngineActual);
                RaisePropertyChanged(() => SubEngineSaved);
                RaisePropertyChanged(() => RefID);
            }
        }

        /// <summary>
        /// Derived from SubEngine. This is only serialized if the engine is not an engine reference. (anonymous)
        /// </summary>
        [XmlElement("Engine")]
        public EngineBase SubEngineSaved { get { return SubEngineActual is EngineRef ? null : SubEngineActual; } set { SubEngineActual = value; } }

        /// <summary>
        /// The pass-through engine for the Engine Editor UI to use.
        /// </summary>
        [XmlIgnore]
        public EngineBase SubEngine
        {
            get
            {
                var refEngine = SubEngineActual as EngineRef;
                return refEngine != null ? refEngine.SubEngine : SubEngineActual;
            } 
            set { SubEngineActual = DyCEBag.GetSubEngineRef(value); }
        }

        /// <summary>
        /// The reference ID as an attribute if this Object Engien property uses a referenced engine.
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

        /// <summary>
        /// Object Engine properties do not use this ID, but it is included due to inheritance form EngineBase.
        /// </summary>
        [XmlAttribute]
        public override string ID { get { return null; } set { base.ID = value; } }

        /// <summary>
        /// Serializable name of this Object Engine property.
        /// </summary>
        [XmlAttribute("Name")]
        public override string NameSaved { get { return Name; } set { Name = value; } }

        /// <summary>
        /// List of one: the sub-engine this property references. Used by the Engine Editor's tree control.
        /// </summary>
        public override IEnumerable<EngineBase> SubEngines { get { return new[] { SubEngine }; } }

        /// <summary>
        /// Creates a new Object Engine property using the string supplied as the property name.
        /// </summary>
        /// <param name="name">The name of the Object Engine's property.</param>
        public EngineProperty(string name) 
            : this(name, new EngineText("New Property Value")) { }

        /// <summary>
        /// Creates a new text Object Engine property using the name and text value supplied.
        /// </summary>
        /// <param name="name">The name of the Object Engine's property.</param>
        /// <param name="value">Text to use as the new Text Sub-Engine's starting value.</param>
        public EngineProperty(string name, string value) 
            : this(name, new EngineText(value)) { }

        /// <summary>
        /// Creates a new Object Engine property using the name and sub-engine.
        /// </summary>
        /// <param name="name">The name of the Object Engine's property.</param>
        /// <param name="subEngine">The sub-engine to reference from the Object Engine property.</param>
        public EngineProperty(string name, EngineBase subEngine)
            : base(name) { SubEngine = DyCEBag.GetSubEngineRef(subEngine); }

        /// <summary>
        /// Creates a new emtpy Object Engine property.
        /// </summary>
        public EngineProperty() { }

        /// <summary>
        /// Returns an Engine Result based on the supplied seed number.
        /// </summary>
        /// <param name="seed">The seed number which will allow the engine to repeatedly return the same 'random' result.</param>
        /// <returns>The property's Property Result based on the seed number supplied. The seed is passed along into the sub-engine.</returns>
        public override ResultBase Go(int seed) { return new ResultProperty(this, seed); }

        /// <summary>
        /// Gets the display name of this Object Engine Property.
        /// </summary>
        /// <returns>The display name of the Object Engine Property.</returns>
        public override string ToString() { return Name + " Property"; }
    }
}