using System.Collections.Generic;

namespace DyCE
{
    public class ResultNumber : ResultBase {
        public ResultNumber(EngineBase engineObject, int seed) : base(engineObject, seed) {}
        public override IEnumerable<ResultBase> SubResults { get { return null; } }

        public override string ToString() { throw new System.NotImplementedException(); }
    }
}