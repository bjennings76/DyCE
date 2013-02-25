using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Serialization;

namespace DyCE
{
    public class EngineList : EngineBase
    {

        private readonly ObservableCollection<EngineBase> _items = new ObservableCollection<EngineBase>();
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

        internal void AddItem() { Items.Add(new EngineText("Test")); }

        RelayCommand _addItemCommand;
        public ICommand AddItemCommand { get { return _addItemCommand ?? (_addItemCommand = new RelayCommand(param => AddItem(), param => _canAddItem)); } }

        #endregion

        public override bool CanRun { get { return Items.Count > 0; } }

        private static readonly List<int> _shuffledIndexes = new List<int>();
        private static int _shuffleIndex;
        public EngineList(string name) { Name = name; }

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

        public override string ToString() { return Items.Select(e => e.ToString()).Aggregate((s1, s2) => s1 + ", " + s2); }
    }
}
