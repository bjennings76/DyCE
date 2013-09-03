using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using DyCE.Annotations;
using GalaSoft.MvvmLight;

namespace DyCE
{
    /// <summary>
    /// Base class for all DyCE Engines.
    /// </summary>
    [XmlInclude(typeof(EngineObject)), 
    XmlInclude(typeof(EngineList)), 
    XmlInclude(typeof(EngineText)), 
    XmlInclude(typeof(EngineRef)), 
    XmlInclude(typeof(EngineRange)), 
    XmlInclude(typeof(EngineNumber))]
    public abstract class EngineBase : ViewModelBase
    {
        // dyce.Bear.Weapon

        [XmlAttribute("ID")]
        public string IDSaved { get { return _id; } set { ID = value; } }

        private string _id;
        /// <summary>
        /// Engine's reference ID. User + Bag + ID create the unique reference for any engine.
        /// </summary>
        [XmlIgnore]
        public virtual string ID
        {
            get { return string.IsNullOrWhiteSpace(_id) ? Name.Replace(" ", "") : _id; }
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

        protected abstract string _resultTemplateDefault { get; }
        
        private string _resultTemplate;

        [XmlElement("Template"), UsedImplicitly]
        public string ResultTemplateSaved
        {
            get { return _resultTemplate == _resultTemplateDefault ? null : _resultTemplate; }
            set { _resultTemplate = value; }
        }

        /// <summary>
        /// This template text will be used to construct the result object's 'ToString' function using StringTemplate.
        /// </summary>
        [XmlIgnore]
        public string ResultTemplate
        {
            get { return _resultTemplate ?? (_resultTemplate = _resultTemplateDefault); }
            set
            {
                _resultTemplate = value;
                RaisePropertyChanged(() => ResultTemplate);
                RaiseEngineChanged();
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

        public virtual string URL { get { return DyCEBag.GetEngineURL(this); } }

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
            if (string.IsNullOrWhiteSpace(name))
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
}
