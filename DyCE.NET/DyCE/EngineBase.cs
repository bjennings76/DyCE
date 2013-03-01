using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;

namespace DyCE
{
    [XmlInclude(typeof(EngineObject)), XmlInclude(typeof(EngineList)), XmlInclude(typeof(EngineText))]
    public abstract class EngineBase : ViewModelBase
    {
        private string _name;
        [XmlAttribute]
        public virtual string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
                RaisePropertyChanged(() => ID);
            }
        }

        // dyce:Bear.Weapon
        private string _id;
        [XmlAttribute]
        public string ID
        {
            get
            {
                if (_id != null)
                    return _id;

                if (_name != null)
                    return "dyce:" + _name.Replace(" ", "");

                _id = "dyce:" + Guid.NewGuid().ToString();

                return _id;
            }
            set
            {
                _id = value;
                RaisePropertyChanged(() => ID);
            }
        }

        public abstract IEnumerable<EngineBase> SubEngines { get; }

        [XmlAttribute, DefaultValue(false)]
        public bool IsSelected { get; set; }

        protected EngineBase(string name)
        {
            _name = name;
        }

        protected EngineBase() { }

        public abstract ResultBase Go(int seed);

        public override string ToString() { return "EngingBase: " + Name; }
    }
}