using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;

namespace DyCE
{
    public abstract class ResultBase : ViewModelBase {
        public EngineBase Engine { get; set; }
        public string Name { get { return Engine.Name; } }
        public int Seed { get; set; }

        public ResultBase(EngineBase engineObject, int seed)
        {
            Engine = engineObject;
            Seed = seed;
        }

        public abstract IEnumerable<ResultBase> SubResults { get; }

        public override string ToString() { return Engine.Name + ": " + base.ToString(); }
    }
}