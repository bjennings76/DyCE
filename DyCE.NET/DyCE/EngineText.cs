using System;

namespace DyCE
{
    public class EngineText : EngineBase {

        private string _name;
        public override string Name 
        {
            get
            {
                if (_name != null)
                    return _name;

                var lines = Text.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length == 1)
                    return "\"" + lines[0] + "\"";

                if (lines.Length > 1)
                    return "\"" + lines[0] + "...\"";

                return "[empty]";
            }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private string _text;
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

        public override ResultBase Go(int seed)
        {
            //TODO: Parse text for sub-engines and replace with results.
            return new ResultText(this, Text, seed);
        }

        public override string ToString() { return "\"" + Text + "\""; }
    }
}