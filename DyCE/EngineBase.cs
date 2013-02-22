using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;

namespace DyCE
{
    public abstract class EngineBase : ViewModelBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }

            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private Guid _id = Guid.NewGuid();
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

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public FileInfo File { get; set; }

        public DyCEBag Bag { internal get; set; }

        public virtual bool CanRun { get { return true; } }

        public bool IsSelected { get; set; }

        public abstract ResultBase Go(int seed);

        public override string ToString() { return "EngingBase: " + Name; }
    }
}