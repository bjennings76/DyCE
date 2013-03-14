using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;

namespace DyCE
{
    public abstract class ResultBase : ViewModelBase {
        public EngineBase Engine { get; set; }
        public string Name { get { return Engine.Name; } }
        public string DisplayName { get { return ToString(); } }
        public int Seed { get; set; }


        public string this[string url]
        {
            get
            {
                return "Accessing: " + url;
            }
        }


        public ResultBase(EngineBase engineObject, int seed)
        {
            Engine = engineObject;
            Seed = seed;
            Engine.Changed += EngineChanged;
            Engine.SubscribeToChange(() => Engine.Name, NameChanged);
        }

        private void EngineChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(() => Name);
            RaisePropertyChanged(() => DisplayName);
            RaisePropertyChanged(() => SubResults);
        }

        private void NameChanged(EngineBase sender)
        {
            RaisePropertyChanged(() => Name);
            RaisePropertyChanged(() => DisplayName);
        }

        public abstract IEnumerable<ResultBase> SubResults { get; }

        public override string ToString() { return Engine.Name + ": " + base.ToString(); }
    }
}