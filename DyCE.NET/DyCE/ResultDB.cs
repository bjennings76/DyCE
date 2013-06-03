using GalaSoft.MvvmLight;

namespace DyCE
{
    public class ResultDB : ViewModelBase
    {
        private readonly int _seed;

        public object this[string refID]
        {
            get
            {
                var subEngine =  DyCEBag.GetEngine(refID);
                if (subEngine != null)
                    return subEngine.Go(_seed);

                return DB.Instance[refID];
            }
        }

        public ResultDB(int seed) { _seed = seed; }
    }
}