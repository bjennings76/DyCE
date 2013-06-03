using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using GalaSoft.MvvmLight.Command;

namespace DyCE
{
    [XmlRoot("EngineList")]
    public class EngineList : EngineBase
    {
        protected override string _resultTemplateDefault { get { return "$this.Result$"; } }

        /// <summary>
        /// List of sub-engines referenced by this engine.
        /// </summary>
        public override IEnumerable<EngineBase> SubEngines { get { return Items; } }

        private readonly ObservableCollection<EngineBase> _items = new ObservableCollection<EngineBase>();
        /// <summary>
        /// The items representing the list this engine selects from.
        /// </summary>
        [XmlElement("Engine")]
        public ObservableCollection<EngineBase> Items { get { return _items; } }


        /// <summary>
        /// Command to add a new Object Engine.
        /// </summary>
        public RelayCommand AddEngineObjectCommand { get { return new RelayCommand(() => Items.Add(new EngineObject("New Object Engine"))); } }

        /// <summary>
        /// Command to add a new List Engine.
        /// </summary>
        public RelayCommand AddEngineListCommand { get { return new RelayCommand(() => Items.Add(new EngineList("New List Engine"))); } }

        /// <summary>
        /// Command to add a new Text Engine.
        /// </summary>
        public RelayCommand AddEngineTextCommand { get { return new RelayCommand(() => Items.Add(new EngineText("New Text Engine"))); } }

        /// <summary>
        /// Command to delete an engine from the list.
        /// </summary>
        public RelayCommand<EngineBase> DeleteCommand { get { return new RelayCommand<EngineBase>(engine => Items.Remove(engine)); } }


        /// <summary>
        /// Creates a new List Engine using a list of supplied items.
        /// </summary>
        /// <param name="items">A list of supplied strings, engines, or lists to choose from.</param>
        public EngineList(IEnumerable<object> items) : this(null, items) { }

        /// <summary>
        /// Creates a new List Engine using a name and list of supplied items.
        /// </summary>
        /// <param name="name">A name used for the engine. The engine's ID is derived from this name.</param>
        /// <param name="items">A list of supplied strings, engines, or lists to choose from.</param>
        public EngineList(string name, IEnumerable<object> items = null)
            : base(name)
        {
            if (items == null) return;

            foreach (var item in items)
                Add(item);
        }

        /// <summary>
        /// Creates a new empty List Engine instance.
        /// </summary>
        public EngineList() { }

        /// <summary>
        /// Adds an item (string, Engine, or sub-lists of items) to the Items list.
        /// </summary>
        /// <param name="item">The item to add. Can be a string, Engine, or list of items.</param>
        public void Add(object item)
        {
            if (item is EngineBase)
                Items.Add(item as EngineBase);
            else if (item is string)
                Items.Add(new EngineText(item as string));
            else if (item is IEnumerable<object>)
                Items.Add(new EngineList(item as IEnumerable<object>));
            else
                throw new Exception("Unknown item type: " + item);
        }

        /// <summary>
        /// Returns an Engine Result based on the supplied seed number.
        /// </summary>
        /// <param name="seed">The seed number which will allow the engine to repeatedly return the same 'random' result.</param>
        /// <returns>The engine's result choice based on the seed number supplied.</returns>
        public override ResultBase Go(int seed) { return new ResultList(this, seed); }

        /// <summary>
        /// Gets the display name of the List Engine.
        /// </summary>
        /// <returns>The display name of the List Engine.</returns>
        public override string ToString() { return Name + " List"; }
    }
}
