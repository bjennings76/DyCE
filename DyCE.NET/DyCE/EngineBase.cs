using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using GalaSoft.MvvmLight;

namespace DyCE
{
    [XmlInclude(typeof(EngineObject)), XmlInclude(typeof(EngineList)), XmlInclude(typeof(EngineText)), XmlInclude(typeof(EngineRef))]
    public abstract class EngineBase : ViewModelBase
    {
        // dyce.Bear.Weapon
        private string _id;
        /// <summary>
        /// Engine's reference ID. User + Bag + ID create the unique reference for any engine.
        /// </summary>
        [XmlAttribute]
        public virtual string ID
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged(() => ID);
            }
        }

        /// <summary>
        /// The engine's name that is saved into the XML file. This is null if it's the same as the ID.
        /// </summary>
        [XmlAttribute("Name")]
        public virtual string NameSaved { get { return _name; } set { Name = value; } }

        private string _name;
        /// <summary>
        /// The engine's readable name. This is the same as the ID if the ID doesn't have spaces.
        /// </summary>
        [XmlIgnore]
        public string Name
        {
            get { return _name ?? _id; }
            set
            {
                SetID(value);
                RaisePropertyChanged(() => Name);
                RaisePropertyChanged(() => ID);
                RaisePropertyChanged(() => DisplayName);
                RaiseEngineChanged();
            }
        }

        /// <summary>
        /// Read only name to display in lists and in the editor. Uses 'ToString()' to get the name modifications.
        /// </summary>
        public string DisplayName { get { return ToString(); } }

        private string _resultTemplate;

        /// <summary>
        /// This template text will be used to construct the result object's 'ToString' function using StringTemplate.
        /// </summary>
        public string ResultTemplate
        {
            get { return _resultTemplate; } 
            set
            {
                _resultTemplate = value;
                RaisePropertyChanged(() => ResultTemplate);
            }
        }

        /// <summary>
        /// List of engines referenced by this engine.
        /// </summary>
        public abstract IEnumerable<EngineBase> SubEngines { get; }

        private EngineBase _selectedSubEngine;
        /// <summary>
        /// Currently selected sub-engine for use by engine editors.
        /// </summary>
        [XmlIgnore]
        public EngineBase SelectedSubEngine
        {
            get { return _selectedSubEngine; } 
            set
            {
                _selectedSubEngine = value;
                RaisePropertyChanged(() => SelectedSubEngine);
                RaisePropertyChanged(() => SelectedSubEngine.SelectedSubEngine);
            }
        }

        #region Changed [event]

        /// <summary>
        /// Occurs when the engine's properties change. Good for refreshing engine results on an engine change.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Raises the engine changed event.
        /// </summary>
        protected void RaiseEngineChanged()
        {
            EventHandler handler = Changed;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// Creates an instance of a basic DyCE engine.
        /// </summary>
        protected EngineBase() { }

        /// <summary>
        /// Creates an instance of a DyCE engine using the supplied name.
        /// </summary>
        /// <param name="name">The name of the engine. Will be used as the name and ID reference (with spaces removed).</param>
        protected EngineBase(string name) { SetID(name); }

        private void SetID(string name)
        {
            if (name == null)
                return;

            _id = name.Replace(" ", "");

            _name = _id != name ? name : null;
        }

        public bool Has(EngineBase subEngine)
        {
            // If this is the same engine, then yes, we have that subEngine (and it would be self-referential)
            if (this == subEngine)
                return true;

            // If SubEngines are null, then we do not have this subEngine.
            if (SubEngines == null)
                return false;

            // If there are any SubEngines that have this engine as a subEngine, then we have this engine.
            return SubEngines.Any(e => e.Has(subEngine));
        }

        public abstract ResultBase Go(int seed);

        public override string ToString() { return "EngingBase: " + Name; }
    }

    public class EngineNumber : EngineBase {
        public override IEnumerable<EngineBase> SubEngines { get { throw new NotImplementedException(); } }
        public override ResultBase Go(int seed) { throw new NotImplementedException(); }
    }
}