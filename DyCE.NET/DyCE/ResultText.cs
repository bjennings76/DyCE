using System.Collections.Generic;
using System.Globalization;
using Antlr4.StringTemplate;

namespace DyCE
{
    public class BasicFormatRenderer : IAttributeRenderer
    {
        public string ToString(object obj, string formatString, CultureInfo culture)
        {
            string objString = obj.ToString();

            if (formatString == null)
                return objString;

            switch (formatString.ToLower())
            {
                case "toupper":
                    var s = objString.ToCharArray();
                    s[0] = char.ToUpper(s[0]);
                    return new string(s);

                case "tolower":
                    return objString.ToLower();

                case "tocaps":
                    return objString.ToUpper();

                default:
                    int objInt;

                    if (int.TryParse(objString, out objInt))
                        return objInt.ToString(formatString);

                    return objString;
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

        public override ResultBase this[string propertyName]
        {
            get { return null; }
        }
    }
}
