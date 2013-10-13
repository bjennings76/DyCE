using System;
using System.Collections.Generic;

namespace DyCE
{
    public class ResultRange : ResultBase
    {
        private readonly EngineRange _engine;
        public ResultRange(EngineRange engine, int seed) : base(engine, seed) { _engine = engine; }

        private ResultNumber _rangeCountResultRoot;
        private ResultNumber _rangeCountResult { get { return _rangeCountResultRoot ?? (_rangeCountResultRoot = _engine.Range.Go(new Random(_seed).Next()) as ResultNumber); } }

        public int Count { get { return _rangeCountResult.Result; } }

        private List<ResultBase> _results;
        public IEnumerable<ResultBase> Results
        {
            get
            {
                if (_results == null)
                {
                    _results = new List<ResultBase>();
                    var rand = new Random(_seed);

                    for (int i = 0; i < _rangeCountResult.Result; i++)
                        _results.Add(_engine.SubEngine.Go(rand.Next()));
                }

                return _results;
            }
        }

        private List<ResultBase> _subResults = new List<ResultBase>();

        public override ResultBase this[string propertyName]
        {
            get { return null; }
        }

        protected override IEnumerable<ResultBase> GetSubResults()
        {
            var subResults = new List<ResultBase>(Results);
            subResults.Insert(0, _rangeCountResult);
            return subResults;
        }
    }
}
