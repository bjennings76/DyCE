using System.Collections.Generic;
using System.Xml.Serialization;

namespace DyCE
{
    public class EngineRef : EngineBase
    {
        [XmlAttribute]
        public string RefID { get; set; }

        public EngineBase SubEngine { get { return DyCEBag.Instance[RefID]; } }

        public override IEnumerable<EngineBase> SubEngines { get { return new[] {SubEngine}; } }

        public override ResultBase Go(int seed) { return SubEngine.Go(seed); }

        public EngineRef() { }

        public EngineRef(string engineID) { RefID = engineID; }

        public override string ToString()
        {
            return SubEngine + " Ref";
        }
    }
}