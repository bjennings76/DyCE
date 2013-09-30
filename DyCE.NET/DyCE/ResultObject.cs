using System;
using System.Collections.Generic;
using System.Linq;

namespace DyCE
{
    public class ResultObject : ResultBase
    {
        private List<ResultProperty> _properties;
        public IEnumerable<ResultProperty> Properties { get { return _properties ?? (_properties = GetUpdatedProperties()); } }

        public override IEnumerable<ResultBase> SubResults { get { return Properties; } }

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

        void engineObject_Changed(object sender, EventArgs e)
        {
            _properties = GetUpdatedProperties();
            RaisePropertyChanged(() => Properties);
            RaisePropertyChanged(() => SubResults);
        }

        private List<ResultProperty> GetUpdatedProperties()
        {
            var rand = new Random(_seed);
            var engine = Engine as EngineObject;
            return engine == null ? null : engine.Properties.Select(p => p.Go(rand.Next()) as ResultProperty).ToList();
        }

        //public override string ToString()
        //{
        //    if (!Properties.Any())
        //        return Name;

        //    if (!string.IsNullOrWhiteSpace(Name))
        //        return Name + " Result: " + Properties.Select(p => p.ToString()).Aggregate((s1, s2) => s1 + ", " + s2);

        //    return Properties.Select(p => p.ToString()).Aggregate((s1, s2) => s1 + ", " + s2) + ")";
        //}
    }
}
