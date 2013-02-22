using System;
using System.Collections.Generic;
using System.Linq;

namespace DyCE
{
    public class ResultObject : ResultBase
    {
        public string Name { get; set; }
        private readonly List<ResultProperty> _properties;
        public IEnumerable<ResultProperty> Properties { get { return _properties; } }

        public ResultObject(EngineObject engineObject, int seed) : base(engineObject)
        {
            var rand = new Random(seed);
            Name = engineObject.Name;
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