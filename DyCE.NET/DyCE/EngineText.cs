namespace DyCE
{
    public class EngineText : EngineBase {
        private string _text;

        public override string Name 
        {
            get
            {
                return "\"" + Text + "\"";
            }
        }

        public override System.Collections.Generic.IEnumerable<EngineBase> SubEngines { get { return null; } }

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

        public EngineText(string text) { Text = text; }

        public override ResultBase Go(int seed)
        {
            //TODO: Parse text for sub-engines and replace with results.
            return new ResultText(this, Text, seed);
        }

        public override string ToString() { return "\"" + Text + "\""; }
    }
}