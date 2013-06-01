using System;
using System.Collections.Generic;
using System.Globalization;
using Antlr4.StringTemplate;

namespace DyCE
{
    public class BasicFormatRenderer : IAttributeRenderer
    {
        public string ToString(object obj, string formatString, CultureInfo culture)
        {
            switch (formatString)
            {
                case "toUpper":
                    var s = obj.ToString().ToCharArray();
                    s[0] = char.ToUpper(s[0]);
                    return new string(s);

                case "toLower":
                    return obj.ToString().ToLower();

                case "toCaps":
                    return obj.ToString().ToUpper();

                default:
                    return obj.ToString();
            }
        }
    }
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
                template.Group.RegisterRenderer(typeof(object), new BasicFormatRenderer());
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