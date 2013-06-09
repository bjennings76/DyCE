using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DyCE
{
    [Serializable]
    public class EngineRange : EngineBase
    {
        protected override string _resultTemplateDefault { get { return "$this.Count$ $this.Name$s"; } }
        private EngineNumber _range = new EngineNumber();
        private EngineBase _subEngine;

        public EngineNumber Range
        {
            get { return _range; }
            set
            {
                _range = value;
                RaisePropertyChanged(() => Range);
                RaiseEngineChanged();
            }
        }

        public EngineBase SubEngine
        {
            get { return _subEngine; } 
            set
            {
                _subEngine = value;
                RaisePropertyChanged(() => SubEngine);
                RaisePropertyChanged(() => SubEngines);
                RaiseEngineChanged();
            }
        }

        public EngineRange() { }

        public EngineRange(string name) : base(name) { }

        public override IEnumerable<EngineBase> SubEngines { get { return new[] {SubEngine}; } }

        public override ResultBase Go(int seed) { return new ResultRange(this, seed); }
    }
}