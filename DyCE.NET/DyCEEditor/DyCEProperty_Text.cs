using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace DynamicContent
{
    //public class DyCEProperty_Text : DyCEProperty
    //{
    //    [XmlElement("TextPiece")]
    //    public ObservableCollection<TextPiece> TextPieces = new ObservableCollection<TextPiece>();

    //    public enum FragmentType { Text, DyCE, Property }

    //    public class TextPiece
    //    {
    //        [XmlAttribute]
    //        public FragmentType Type { get; set; }

    //        [XmlText]
    //        public string Fragment { get; set; }
    //    }

    //    public override string Go(Random r)
    //    {
    //        string result = "";
    //        foreach (TextPiece piece in TextPieces)
    //        {
    //            if (piece.Type == FragmentType.Text)
    //                result += piece.Fragment;
    //            else if (piece.Type == FragmentType.Property)
    //                result += Engine.GetProperty(piece.Fragment).Go(r);
    //            else if (piece.Type == FragmentType.DyCE)
    //                result += Bag.GetDyCEngine(piece.Fragment).GetTitle(r);
    //        }

    //        return result;
    //    }
    //}
}
