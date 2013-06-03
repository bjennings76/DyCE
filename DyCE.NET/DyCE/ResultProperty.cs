using System.Collections.Generic;

namespace DyCE
{
    public class ResultProperty: ResultBase
    {
        public ResultBase Result { get; set; }

        public ResultProperty(EngineProperty engineObject, int seed) : base(engineObject, seed)
        {
            Result = engineObject.SubEngineActual.Go(seed);
        }

        public override IEnumerable<ResultBase> SubResults { get { return new[] {Result}; } }
    }
}