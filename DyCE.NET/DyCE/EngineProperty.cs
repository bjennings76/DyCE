using System.Collections.Generic;

namespace DyCE
{
    public class EngineProperty : EngineBase
    {
        public override IEnumerable<EngineBase> SubEngines { get { return new EngineBase[] {ValueEngine}; } }

        public EngineProperty(string name) : this(name, new EngineText("New Property Value")) { }
        public EngineProperty(string name, string value) : this(name, new EngineText(value)) { }

        public EngineProperty(string name, EngineBase valueEngine) : base(name) { ValueEngine = valueEngine; }

        public EngineBase ValueEngine { get; set; }

        public override ResultBase Go(int seed) { return new ResultProperty(this, seed); }

        public override string ToString()
        {
            return Name + ": " + ValueEngine;
        }
    }
}