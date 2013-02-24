using System;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;

namespace DyCE
{
    public abstract class ResultBase : ViewModelBase {
        public EngineBase Engine { get; set; }
        public string Name { get { return Engine.Name; } }

        public ResultBase(EngineBase engineObject)
        {
            Engine = engineObject;
        }

        public override string ToString() { return Engine.Name + ": " + base.ToString(); }
    }
}