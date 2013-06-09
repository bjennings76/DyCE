using System.Collections.Generic;

namespace DyCE
{
    public class ResultProperty: ResultBase
    {
        public ResultBase Result { get; set; }

        public ResultProperty(EngineProperty engine, int seed) : base(engine, seed)
        {
            Result = engine.SubEngineActual.Go(seed);
        }

        public override IEnumerable<ResultBase> SubResults { get { return new[] {Result}; } }
    }
}