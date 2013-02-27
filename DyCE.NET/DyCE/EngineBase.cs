using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;

namespace DyCE
{
    public abstract class EngineBase : ViewModelBase
    {
        private string _name;
        public virtual string Name
        {
            get { return _name; }

            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private Guid _id = Guid.NewGuid();
        protected EngineBase(string name) { _name = name; }

        public Guid ID
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                RaisePropertyChanged(() => ID);
            }
        }

        public abstract IEnumerable<EngineBase> SubEngines { get; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public FileInfo File { get; set; }

        public virtual bool CanRun { get { return true; } }

        public bool IsSelected { get; set; }

        public abstract ResultBase Go(int seed);

        public override string ToString() { return "EngingBase: " + Name; }
    }
}