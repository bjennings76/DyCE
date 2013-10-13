using System;
using System.Collections.Generic;

namespace DyCE
{
    public class ResultList : ResultBase
    {
        private readonly EngineList _engine;

        public ResultList(EngineList engine, int seed) : base(engine, seed) { _engine = engine; }

        private ResultBase _result;
        public ResultBase Result
        {
            get
            {
                if (_result != null)
                    return _result;

                if (_engine.Items.Count == 0)
                    return new ResultEmpty(_engine);

                var rand = new Random(_seed);
                int index = rand.Next(0, _engine.Items.Count);
                _result = _engine.Items[index].Go(rand.Next());
                return _result;
            }
        }

        protected override IEnumerable<ResultBase> GetSubResults() { return new List<ResultBase>{Result}; }

        public override ResultBase this[string propertyName]
        {
            get { return Result[propertyName]; }
        }
    }
}
