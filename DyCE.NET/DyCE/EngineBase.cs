using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;

namespace DyCE
{
    [XmlInclude(typeof(EngineObject)), XmlInclude(typeof(EngineList)), XmlInclude(typeof(EngineText)), XmlInclude(typeof(EngineRef))]
    public abstract class EngineBase : ViewModelBase
    {
        // dyce:Bear.Weapon
        private string _id;
        [XmlAttribute]
        public virtual string ID
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

        [XmlAttribute("Name")]
        public virtual string NameSaved { get { return _name; } set { Name = value; } }

        private string _name;
        [XmlIgnore]
        public string Name
        {
            get { return _name ?? _id; }
            set
            {
                SetID(value);
                RaisePropertyChanged(() => Name);
                RaisePropertyChanged(() => ID);
            }
        }

        public abstract IEnumerable<EngineBase> SubEngines { get; }

        [XmlAttribute, DefaultValue(false)]
        public bool IsSelected { get; set; }

        protected EngineBase() { }

        protected EngineBase(string name) { SetID(name); }

        private void SetID(string name)
        {
            if (name == null)
                return;

            _id = name.Replace(" ", "");
            if (_id != name)
                _name = name;
        }

        public abstract ResultBase Go(int seed);

        public override string ToString() { return "EngingBase: " + Name; }
    }
}