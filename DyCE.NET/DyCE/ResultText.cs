using System.Collections.Generic;
using Antlr4.StringTemplate;

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

        public ResultText(EngineText engineObject, string text, int seed) : base(engineObject, seed)
        {
            _text = text;
        }

        public override IEnumerable<ResultBase> SubResults { get { return null; } }

        public override string ToString()
        {
            var template = new Template(Text);
            template.Add("db", DyCEBag.Instance);
            return template.Render();
        }
    }
}