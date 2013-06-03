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
        private readonly EngineText _engine;

        public string Text { get { return _engine.Text; } }

        public ResultText(EngineText engine, int seed) : base(engine, seed)
        {
            _engine = engine;
            engine.Changed += engine_Changed;
        }

        void engine_Changed(object sender, System.EventArgs e)
        {
            RaisePropertyChanged(() => Text);
        }

        public override IEnumerable<ResultBase> SubResults { get { return null; } }
    }
}