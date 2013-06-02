using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;
using GalaSoft.MvvmLight.Command;

namespace DyCE
{
    public class EngineObject : EngineBase
    {
        /// <summary>
        /// Object engine's internal list of properties.
        /// </summary>
        private readonly ObservableCollection<EngineProperty> _properties = new ObservableCollection<EngineProperty>();

        /// <summary>
        /// Object Engine's list of properties.
        /// </summary>
        [XmlElement("Property")]
        public ObservableCollection<EngineProperty> Properties { get { return _properties; } }

        /// <summary>
        /// List of sub-engines used by this engine. Synonymous with 'Properties' for the Object Engine.
        /// </summary>
        public override IEnumerable<EngineBase> SubEngines { get { return Properties; } }

        /// <summary>
        /// Creates a new Object Engine using the supplied name and list of properties.
        /// </summary>
        /// <param name="name">Name of the Object Engine. (Also used to derive the engine's ID.)</param>
        /// <param name="properties"></param>
        public EngineObject(string name, params EngineProperty[] properties)
            : base(name) { _properties = new ObservableCollection<EngineProperty>(properties); }

        /// <summary>
        /// Creates a new empty Object Engine. Usually used in deserialization.
        /// </summary>
        public EngineObject() { }

        /// <summary>
        /// Command to add a new property to the Object Engine.
        /// </summary>
        public RelayCommand CreatePropertyCommand { get { return new RelayCommand(CreateProperty); } }

        /// <summary>
        /// Adds a new property to the Object Engine.
        /// </summary>
        private void CreateProperty()
        {
            var newProperty = new EngineProperty("New Property");
            Properties.Add(newProperty);
            SelectedSubEngine = newProperty;
        }

        /// <summary>
        /// Deletes the selected property from the Object Engine.
        /// </summary>
        public RelayCommand<EngineProperty> DeleteCommand { get { return new RelayCommand<EngineProperty>(prop => Properties.Remove(prop)); } }

        /// <summary>
        /// Command to add a new Engine Object to the current DyCEBag and adds a property that references the new Object Engine.
        /// </summary>
        public RelayCommand AddEngineObjectCommand { get { return new RelayCommand(AddEngineObject); } }

        /// <summary>
        /// Function that creates a new Engine Object, adds it to the current DyCEBag, and adds a property that references the new Object Engine.
        /// </summary>
        private void AddEngineObject() {
            var newEngine = new EngineObject("New Object Engine");
            DB.Instance["General"].Add(newEngine);
            Properties.Add(new EngineProperty("New Property", newEngine)); 
        }

        /// <summary>
        /// Command to add a new annonymous List Engine to this Object Engine's property list.
        /// </summary>
        public RelayCommand AddEngineListCommand { get { return new RelayCommand(() => Properties.Add(new EngineProperty("New Property", new EngineList()))); } }

        /// <summary>
        /// Command to add a new annonymous Text Engine to this Object Engine's property list.
        /// </summary>
        public RelayCommand AddEngineTextCommand { get { return new RelayCommand(() => Properties.Add(new EngineProperty("New Property", new EngineText("New Text Value")))); } }

        /// <summary>
        /// Adds a new item to the property list.
        /// </summary>
        /// <param name="item"></param>
        public void Add(object item)
        {
            if (item is EngineBase)
                Properties.Add(new EngineProperty("New Property", item as EngineBase));
            else if (item is string)
                Properties.Add(new EngineProperty("New Property", item as string));
            else if (item is IEnumerable<object>)
                Properties.Add(new EngineProperty("New Property", new EngineList(item as IEnumerable<object>)));
            else
                throw new Exception("Unknown item type: " + item);
        }

        /// <summary>
        /// This indexer allows access to the sub-engines using runtime property names. Mostly used by the template system. (e.g. 'dyce.ObjectEngine.PropertyName')
        /// </summary>
        /// <param name="propertyName">The name of the Object Engine's property.</param>
        /// <returns>The sub-engine assigned to the referenced property.</returns>
        public EngineBase this[string propertyName] { get { return Properties.FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase)); } }

        /// <summary>
        /// Returns an Engine Result based on the supplied seed number.
        /// </summary>
        /// <param name="seed">The seed number which will allow the engine to repeatedly return the same 'random' result.</param>
        /// <returns>The engine's Object Result choice based on the seed number supplied.</returns>
        public override ResultBase Go(int seed) { return new ResultObject(this, seed); }

        /// <summary>
        /// Gets the display name of the Object Engine.
        /// </summary>
        /// <returns>The display name of the Object Engine.</returns>
        public override string ToString() { return Name + " Object"; }
    }
}