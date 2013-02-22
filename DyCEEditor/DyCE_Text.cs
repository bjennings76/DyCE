using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DynamicContent
{
    //public class DyCE_Text : DyCE, INotifyPropertyChanged
    //{

    //    #region Bindable Properties

    //    public string GoFormula
    //    {
    //        get
    //        {
    //            string content = "";
    //            foreach (TextPiece piece in TextPieces)
    //                content += piece.Fragment;

    //            return content;
    //        }

    //        set
    //        {
    //            //do stuff with the new value here
    //        }
    //    }

    //    public override string GoDetailsFormula
    //    {
    //        get { return "Details"; }
    //        set { }
    //    }

    //    [XmlElement("TextPiece")]
    //    public ObservableCollection<TextPiece> TextPieces = new ObservableCollection<TextPiece>();

    //    #endregion

    //    public DyCE_Text(string name, DyCEBag bag)
    //    {
    //        // TODO: Complete member initialization
    //        Name = name;
    //        Bag = bag;
    //    }

    //    public override ObservableCollection<DyCE> SubDyCE
    //    {
    //        get
    //        {
    //            ObservableCollection<DyCE> dyceList = new ObservableCollection<DyCE>();

    //            foreach (TextPiece piece in TextPieces)
    //                if (piece.Type == FragmentType.DyCE)
    //                    dyceList.Add(Bag[piece.Fragment]);

    //            return dyceList;
    //        }
    //    }

    //    public enum FragmentType { Text, DyCE }

    //    public class TextPiece : INotifyPropertyChanged
    //    {
    //        private FragmentType _type = FragmentType.Text;
    //        [XmlAttribute]
    //        public FragmentType Type 
    //        {
    //            get { return _type; }
    //            set
    //            {
    //                _type = value;
    //                PropertyChanged.Notify(() => Type);
    //            }
    //        }

    //        private string _fragment = "";
    //        [XmlText]
    //        public string Fragment 
    //        {
    //            get { return _fragment; }
    //            set
    //            {
    //                _fragment = value;
    //                PropertyChanged.Notify(() => Fragment);
    //            }
    //        }

    //        public TextPiece() { }

    //        public TextPiece(string fragment)
    //        {
    //            Fragment = fragment;
    //        }

    //        public TextPiece(FragmentType type, string fragment)
    //        {
    //            Type = type;
    //            Fragment = fragment;
    //        }

    //        public event PropertyChangedEventHandler PropertyChanged;
    //    }

    //    public void AddText()
    //    {
    //        AddText("test");
    //    }
        
    //    public void AddText(string fragment)
    //    {
    //        TextPieces.Add(new TextPiece(fragment));
    //        PropertyChanged.Notify(() => GoFormula);
    //    }

    //    public void AddSubDyCE(string DyCEID)
    //    {
    //        TextPieces.Add(new TextPiece(FragmentType.DyCE, DyCEID));
    //    }

    //    public override string Go(Random r)
    //    {
    //        return Name;
    //    }

    //    public override string GoDetails(Random r)
    //    {
    //        string result = "";

    //        foreach (TextPiece piece in TextPieces)
    //        {
    //            if (piece.Type == FragmentType.Text)
    //                result += piece.Fragment;
    //            else if (piece.Type == FragmentType.DyCE)
    //                result += Bag.Go(piece.Fragment, r);
    //        }

    //        return result;
    //    }

    //    internal override void AddProperty()
    //    {
    //        AddText("Test");
    //    }

    //    public override event PropertyChangedEventHandler PropertyChanged;
    //}
}
