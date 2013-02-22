using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DynamicContent
{
    //public abstract class DyCEProperty : BindableObject
    //{
    //    public enum DyCEPropertyFlag { Normal, ResultTitle, ResultDetails }

    //    private string _name;
    //    public string Name 
    //    {
    //        get { return _name; }
    //        set
    //        {
    //            _name = value;
    //            RaisePropertyChanged("Name");
    //        }
    //    }

    //    private string _id;
    //    public string ID 
    //    {
    //        get { return _id; }
    //        set
    //        {
    //            _id = value;
    //            RaisePropertyChanged("ID");
    //        }
    //    }

    //    private DyCEPropertyFlag _flag = DyCEPropertyFlag.Normal;
    //    public DyCEPropertyFlag Flag 
    //    {
    //        get { return _flag; }
    //        set
    //        {
    //            _flag = value;
    //            RaisePropertyChanged("Flag");
    //        }
    //    }

    //    private DyCEBag _bag;
    //    public DyCEBag Bag { set { _bag = value; } internal get { return _bag; } }

    //    private DyCE _engine;
    //    public DyCE Engine { set { _engine = value; } internal get { return _engine; } }

    //    public abstract string Go(Random r);

    //    public DyCEProperty() { }

    //    public override string ToString()
    //    {
    //        return Name + " (" + base.ToString() + ")";
    //    }
    //}
}
