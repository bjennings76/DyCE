using System.Collections.Generic;

namespace DyCE
{
    public class ResultProperty: ResultBase
    {
        public ResultBase ValueResult { get; set; }

        public ResultProperty(EngineProperty engineObject, int seed) : base(engineObject, seed)
        {
            ValueResult = engineObject.SubEngineActual.Go(seed);
        }

        public override IEnumerable<ResultBase> SubResults { get { return new[] {ValueResult}; } }

        public override string ToString() { return ValueResult.ToString(); }
    }
}