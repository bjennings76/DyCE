namespace DyCE
{
    public class ResultText : ResultBase
    {
        public ResultText(EngineBase engineObject, string text, int seed) : base(engineObject)
        {
            _text = text;
        }

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

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Engine.Name))
                return Engine.Name + " Result: " + Text;
            
            return Text;
        }
    }
}