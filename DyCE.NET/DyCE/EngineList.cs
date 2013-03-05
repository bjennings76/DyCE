using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Serialization;
using GalaSoft.MvvmLight.Command;

namespace DyCE
{
    [XmlRoot("EngineList")]
    public class EngineList : EngineBase
    {
        public override IEnumerable<EngineBase> SubEngines { get { return Items; } }

        private readonly ObservableCollection<EngineBase> _items = new ObservableCollection<EngineBase>();
        [XmlElement("Engine")]
        public ObservableCollection<EngineBase> Items { get { return _items; } }

        private int _cyclePrime;
        [XmlIgnore]
        public int CyclePrime
        {
            get
            {
                if (_cyclePrime == 0) 
                    _cyclePrime = Randomize.GetRandomPrime(Items.Count);
                return _cyclePrime;
            }
        }

        private int _lastIndex = -1;

        [XmlIgnore]
        public int LastIndex
        {
            get
            {
                if (_lastIndex == -1)
                    _lastIndex = Randomize.GetNextIndex(0, Items.Count, CyclePrime);

                if (_lastIndex == 0)
                    _cyclePrime = Randomize.GetRandomPrime(Items.Count);

                return _lastIndex;
            }
        }

        #region ICommands

        private const bool _canAddItem = true;

        private void AddItem() { Add("Test"); }
        private void Add(object item)
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

        RelayCommand _addItemCommand;
        public ICommand AddItemCommand { get { return _addItemCommand ?? (_addItemCommand = new RelayCommand(AddItem, () => _canAddItem)); } }

        #endregion

        private static readonly List<int> _shuffledIndexes = new List<int>();
        private static int _shuffleIndex;

        public EngineList(IEnumerable<object> items) : this(null, items) { }

        public EngineList(string name, IEnumerable<object> items):base(name)
        {
            foreach (var item in items)
                Add(item);
        }

        public EngineList(string name):base(name) { }

        public EngineList() { }

        public RelayCommand<EngineBase> DeleteCommand { get { return new RelayCommand<EngineBase>(DeleteProperty); } }
        public void DeleteProperty(EngineBase engine) { Items.Remove(engine); }

        public override ResultBase Go(int seed)
        {
            var rand = new Random(seed);

            if (Items.Count < 1)
                return new ResultEmpty(this, rand.Next());

            if (Items.Count != _shuffledIndexes.Count)
            {
                _shuffledIndexes.Clear();
                for (int i = 0; i < Items.Count; i++)
                    _shuffledIndexes.Add(i);
                Reshuffle(rand);
            }

            _shuffleIndex++;

            if (_shuffleIndex >= Items.Count)
                Reshuffle(rand);

            return Items[_shuffledIndexes[_shuffleIndex]].Go(rand.Next());
        }

        private void Reshuffle(Random rand)
        {
            _shuffleIndex = 0;
            _shuffledIndexes.Shuffle(rand);
        }

        public void AddItems(params string[] itemNames)
        {
            foreach (var item in itemNames)
                Items.Add(new EngineText(item));
        }

        public void AddWithDetail(string name, string desc) { Items.Add(new EngineObject("TextWithDetail", new EngineProperty("Text", name), new EngineProperty("TextDetail", desc))); }

        public override string ToString()
        {
            return Name + " List"; //Items.Select(e => e.ToString()).Aggregate((s1, s2) => s1 + ", " + s2); 
        }
    }
}
