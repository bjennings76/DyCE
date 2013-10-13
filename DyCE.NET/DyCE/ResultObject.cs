using System;
using System.Collections.Generic;
using System.Linq;

namespace DyCE
{
    public class ResultObject : ResultBase
    {
        private List<ResultProperty> _properties;
        public IEnumerable<ResultProperty> Properties { get { return _properties ?? (_properties = GetUpdatedProperties()); } }

        public override ResultBase this[string propertyName]
        {
            get
            {
                return Properties.First(p => p.ID.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        public ResultObject(EngineObject engine, int seed) : base(engine, seed)
        {
            engine.Changed += engineObject_Changed;
        }

        private void engineObject_Changed(object sender, EventArgs e)
        {
            _properties = GetUpdatedProperties();
            RaisePropertyChanged(() => Properties);
            RaisePropertyChanged(() => SubResults);
        }

        private List<ResultProperty> GetUpdatedProperties()
        {
            var rand = new Random(_seed);
            var engine = Engine as EngineObject;

            if (engine == null)
                throw new Exception("Whoah. Engine is somehow not of type EngineObject.");

            // Get all the results at once so the order they are accessed in does not matter.
            return engine.Properties.Select(p => p.Go(rand.Next()) as ResultProperty).ToList();
        }

        protected override IEnumerable<ResultBase> GetSubResults() { return _properties != null ? _properties.Cast<ResultBase>() : new List<ResultBase>(); }

    }
}
