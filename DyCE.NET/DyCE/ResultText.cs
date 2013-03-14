using System;
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
            if (Text == null)
                return base.ToString();

            try
            {
                var template = new Template(Text, '$', '$');
                template.Add("dyce", this);
                var result = template.Render();
                return result;
            }
            catch (Exception ex)
            {
                return "*** Template Error: " + ex.Message + " ***\r\n\r\n" + Text;
            }
        }
    }
}