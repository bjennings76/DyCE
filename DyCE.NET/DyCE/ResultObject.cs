using System;
using System.Collections.Generic;
using System.Linq;

namespace DyCE
{
    public class ResultObject : ResultBase
    {
        private readonly List<ResultProperty> _properties;
        public IEnumerable<ResultProperty> Properties { get { return _properties; } }

        public ResultBase this[string propertyName] { get { return Properties.FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase)); } }

        public ResultObject(EngineObject engineObject, int seed) : base(engineObject)
        {
            var rand = new Random(seed);
            _properties = engineObject.Properties.Select(p => p.Go(rand.Next()) as ResultProperty).ToList();
        }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Name))
                return Name + " Result: " + Properties.Select(p => p.ToString()).Aggregate((s1, s2) => s1 + ", " + s2);

            return Properties.Select(p => p.ToString()).Aggregate((s1, s2) => s1 + ", " + s2) + ")";
        }
    }
}