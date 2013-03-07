using System.Collections.Generic;
using Antlr4.StringTemplate;

namespace DyCE
{
    public class ResultText : ResultBase
    {
        public string Text
        {
            get
            {
                var engine = Engine as EngineText;
                return engine != null ? engine.Text : null;
            }
        }

        public ResultText(EngineText engineObject, int seed) : base(engineObject, seed)
        {
            engineObject.Changed += engineObject_Changed;
        }

        void engineObject_Changed(object sender, System.EventArgs e)
        {
            RaisePropertyChanged(() => Text);
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