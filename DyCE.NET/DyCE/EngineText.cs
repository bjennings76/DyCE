using System;
using System.Xml.Serialization;

namespace DyCE
{
    public class EngineText : EngineBase {
        private string _text;
        [XmlText]
        public string Text
        {
            get { return _text; } 
            set
            {
                _text = value;
                RaisePropertyChanged(() => Text);
                RaisePropertyChanged(() => Name);
            }
        }

        public override System.Collections.Generic.IEnumerable<EngineBase> SubEngines { get { return null; } }

        public EngineText(string text, string name = null) : base(name) { Text = text; }
        public EngineText() { }

        public override ResultBase Go(int seed)
        {
            //TODO: Parse text for sub-engines and replace with results.
            return new ResultText(this, Text, seed);
        }

        public override string ToString()
        {
            if (Name != null)
                return Name;

            var lines = Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 1)
                return "\"" + lines[0] + "\"";

            if (lines.Length > 1)
                return "\"" + lines[0] + "...\"";

            return "[empty]";
        }
    }
}