using System;
using System.Collections.Generic;

namespace DyCE
{
    public class ResultRange : ResultBase
    {
        private readonly EngineRange _engine;
        public ResultRange(EngineRange engine, int seed) : base(engine, seed) { _engine = engine; }

        private ResultNumber _rangeCountResult;

        private List<ResultBase> _results;
        public IEnumerable<ResultBase> Results
        {
            get
            {
                if (_results == null)
                {
                    _results = new List<ResultBase>();
                    var rand = new Random(Seed);
                    _rangeCountResult = _engine.Range.Go(new Random(Seed).Next()) as ResultNumber;

                    if (_rangeCountResult == null)
                        throw new Exception("Range results is not a 'ResultNumber' result somehow.");

                    _subResults.Add(_rangeCountResult);

                    for (int i = 0; i < _rangeCountResult.Result; i++)
                        _results.Add(_engine.SubEngine.Go(rand.Next()));

                    _subResults.AddRange(_results);
                }

                return _results;
            }
        }

        public int Count { get { return _results.Count; } }

        private List<ResultBase> _subResults = new List<ResultBase>();
        public override IEnumerable<ResultBase> SubResults { get { return _subResults ?? (_subResults = GetSubResults()); } }

        private List<ResultBase> GetSubResults()
        {
            var subResults = new List<ResultBase>(Results);
            subResults.Insert(0, _rangeCountResult);
            return subResults;
        }
    }
}