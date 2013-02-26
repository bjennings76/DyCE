using System.Collections.Generic;

namespace DyCE
{
    public class ResultEmpty : ResultBase
    {
        public ResultEmpty(EngineBase engine, int seed)
            : base(engine, seed) { }

        public override IEnumerable<ResultBase> SubResults { get { return null; } }
    }
}