using System;
using System.Collections.Generic;
using DyCE.Annotations;
using GalaSoft.MvvmLight;

namespace DyCE
{
    public class ResultDB : ViewModelBase
    {
        private readonly Random _rand;
        public readonly List<ResultBase> Results = new List<ResultBase>();

        [UsedImplicitly]
        public object this[string refID]
        {
            get
            {
                var subEngine =  DyCEBag.GetEngine(refID);

                if (subEngine == null)
                    return DB.Instance[refID];

                var result = subEngine.Go(_rand.Next());
                Results.Add(result);
                return result;
            }
        }

        public ResultDB(int seed)
        {
            _rand = new Random(seed);
        }
    }
}
