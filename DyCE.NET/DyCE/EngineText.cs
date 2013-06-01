using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace DyCE
{
    public class EngineText : EngineBase {
        private string _text;
        [XmlText]
        public string Text
        {
            get { return _text; } 
            set
            {
                _text = value;
                RaisePropertyChanged(() => Text);
                RaisePropertyChanged(() => Name);
                RaisePropertyChanged(() => DisplayName);
                RaiseEngineChanged();
            }
        }

        private readonly Regex _engineRegex = new Regex(@"\$dyce.(?<engine>[^;$.]+)");
        public override IEnumerable<EngineBase> SubEngines
        {
            get
            {
                if (_text == null)
                    return null;

                var matches = _engineRegex.Matches(_text);

                if (matches.Count == 0)
                    return null;

                var engines = new List<EngineBase>();

                foreach (Match match in matches)
                {

                    var subEngine = DyCEBag.GetEngine(match.Groups["engine"].Value);

                    if (subEngine != null && engines.All(e => e.ID != subEngine.ID))
                        engines.Add(subEngine);
                }

                return engines;
            }
        }

        public EngineText(string text, string name = null) : base(name) { Text = text; }
        public EngineText() { }

        public override ResultBase Go(int seed) { return new ResultText(this, seed); }

        public override string ToString()
        {
            if (Name != null)
                return Name;

            if (Text == null)
                return "";

            var lines = Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 1)
                return "\"" + lines[0] + "\"";

            if (lines.Length > 1)
                return "\"" + lines[0] + "...\"";

            return "[empty]";
        }

        public override void Add(object item) { throw new NotImplementedException(); }
    }
}