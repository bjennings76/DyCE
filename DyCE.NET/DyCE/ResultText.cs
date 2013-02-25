namespace DyCE
{
    public class ResultText : ResultBase
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                RaisePropertyChanged(() => Text);
            }
        }

        public StringTemplate

        public ResultText(EngineBase engineObject, string text, int seed) : base(engineObject, seed)
        {
            _text = text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}