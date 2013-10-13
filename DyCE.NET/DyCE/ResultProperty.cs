using System.Collections.Generic;

namespace DyCE
{
    public class ResultProperty: ResultBase
    {
        public ResultBase Result { get; set; }

        public override ResultBase this[string propertyName] { get { return Result[propertyName]; } }
        protected override IEnumerable<ResultBase> GetSubResults() { return new List<ResultBase>{Result}; }

        public ResultProperty(EngineProperty engine, int seed) : base(engine, seed)
        {
            Result = engine.SubEngineActual.Go(seed);
        }

        public string ID { get { return Engine.ID; } }
    }
}
