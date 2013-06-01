using System.Collections.Generic;

namespace DyCE
{
    public class ResultProperty: ResultBase
    {
        private ResultBase _valueResult { get; set; }

        public ResultProperty(EngineProperty engineObject, int seed) : base(engineObject, seed)
        {
            _valueResult = engineObject.SubEngineActual.Go(seed);
        }

        public override IEnumerable<ResultBase> SubResults { get { return new[] {_valueResult}; } }

        public override string ToString() { return _valueResult.ToString(); }
    }
}