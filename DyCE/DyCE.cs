using System;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace DynamicContent
{

    //  Name: Bears
    //  Title Result: Brown Bear
    //  Detail Result:
    //      This Brown Bear has <Property ID="FurType">brown fur</Property>, <Property ID="ClawType"/>, and <Property ID="Cave Type"/>.
    //      It likes <Property ID="BearHobbies"/>, <Property ID="BearHobbies"/>, and <Property ID="BearHobbies"/>.
    //      I has <Property ID="Skin"/>, <Property ID="Teeth"/>, <Property ID="Nose"/>, <Property ID="Ears"/>, <Property ID="Eye Type"/>,
    //      and <Property ID="Internal Organs"/>.
    //
    //  Some items won't have multiples. List is a type of property.

    public class DyCE : INotifyPropertyChanged
    {

        #region Bindable Properties

        private string _name;
        public string Name 
        {
            get { return _name; }
            
            set 
            {
                _name = value;
                PropertyChanged.Notify(() => Name);
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
                PropertyChanged.Notify(() => ID);
            }
        }

        private FileInfo _file;
        [XmlIgnore]
        public FileInfo File
        {
            get 
            {
                if (_file == null)
                    _file = new FileInfo(@".\Engines\" + Name + ".xml");

                return _file;
            }
            set
            {
                _file = value;
                PropertyChanged.Notify(() => File);
            }
        }

        [XmlElement("TextPiece")]
        public ObservableCollection<TextPiece> TextPieces = new ObservableCollection<TextPiece>();

        #endregion

        public DyCEBag Bag { internal get; set; }

        public virtual ObservableCollection<DyCE> SubDyCE 
        {
            get
            {
                ObservableCollection<DyCE> dyceList = new ObservableCollection<DyCE>();

                foreach (TextPiece piece in TextPieces)
                    if (piece.Type == FragmentType.DyCE)
                        dyceList.Add(Bag[piece.Fragment]);

                return dyceList;
            }
        }

        public DyCE() { }

        public DyCE(string name, DyCEBag bag)
        {
            Bag = bag;
            Name = name;
        }

        public string Go()
        {
            return Go(new Random());
        }
        public string Go(int seed)
        {
            return Go(new Random(seed));
        }
        public string Go(Random r)
        {
            return Name;
        }


        public string GoFormula
        {
            get
            {
                string content = "";
                foreach (TextPiece piece in TextPieces)
                    content += piece.Fragment;

                return content;
            }

            set
            {
                //do stuff with the new value here
            }
        }

        public string GoDetails(string seedText)
        {
            return GoDetails(new Random(Convert.ToInt32(seedText)));
        }
        public string GoDetails(int seedNum)
        {
            return GoDetails(new Random(seedNum));
        }

        public string GoDetails(Random r)
        {
            string result = "";

            foreach (TextPiece piece in TextPieces)
            {
                if (piece.Type == FragmentType.Text)
                    result += piece.Fragment;
                else if (piece.Type == FragmentType.DyCE)
                    result += Bag.Go(piece.Fragment, r);
            }

            return result;
        }

        public string GoDetailsFormula
        {
            get { return "Details"; }
            set { }
        }


        #region ICommands

        private bool CanAddProperty = true;

        internal void AddProperty()
        {
            AddText("Test");
        }

        RelayCommand _addPropertyCommand;
        public ICommand AddPropertyCommand
        {
            get
            {
                if (_addPropertyCommand == null)
                    _addPropertyCommand = new RelayCommand(param => this.AddProperty(), param => this.CanAddProperty);

                return _addPropertyCommand;
            }
        }

        #endregion




        public enum FragmentType { Text, DyCE }

        public class TextPiece : INotifyPropertyChanged
        {
            private FragmentType _type = FragmentType.Text;
            [XmlAttribute]
            public FragmentType Type
            {
                get { return _type; }
                set
                {
                    _type = value;
                    PropertyChanged.Notify(() => Type);
                }
            }

            private string _fragment = "";
            [XmlText]
            public string Fragment
            {
                get { return _fragment; }
                set
                {
                    _fragment = value;
                    PropertyChanged.Notify(() => Fragment);
                }
            }

            public TextPiece() { }

            public TextPiece(string fragment)
            {
                Fragment = fragment;
            }

            public TextPiece(FragmentType type, string fragment)
            {
                Type = type;
                Fragment = fragment;
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public void AddText()
        {
            AddText("test");
        }

        public void AddText(string fragment)
        {
            TextPieces.Add(new TextPiece(fragment));
            PropertyChanged.Notify(() => GoFormula);
        }

        public void AddSubDyCE(string DyCEID)
        {
            TextPieces.Add(new TextPiece(FragmentType.DyCE, DyCEID));
        }




        public override string ToString()
        {
            return Name + " (" + base.ToString() + ")";
        }

        public virtual event PropertyChangedEventHandler PropertyChanged;
    }
}
