using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace DyCE
{
    public class EngineText : EngineBase {
        /// <summary>
        /// The Text Engine's template text.
        /// </summary>
        [XmlText]
        public string Text
        {
            get { return ResultTemplate; }
            set
            {
                ResultTemplate = value;
                RaisePropertyChanged(() => Text);
                RaisePropertyChanged(() => Name);
                RaisePropertyChanged(() => DisplayName);
                RaiseEngineChanged();
            }
        }

        /// <summary>
        /// Regular expression that parses through the template text for top-level engine references.
        /// </summary>
        private readonly Regex _engineRegex = new Regex(@"\$dyce.(?<engine>[^;$.]+)");

        protected override string _resultTemplateDefault { get { return Text; } }

        /// <summary>
        /// List of sub-engines used by this engine parsed from the template text.
        /// </summary>
        public override IEnumerable<EngineBase> SubEngines
        {
            get
            {
                if (ResultTemplate == null)
                    return null;

                var matches = _engineRegex.Matches(ResultTemplate);

                if (matches.Count == 0)
                    return null;

                return matches
                    .Cast<Match>()
                    .Select(match => match.Groups["engine"].Value)
                    .Distinct()
                    .Select(DyCEBag.GetEngine)
                    .Where(engine => engine != null);
            }
        }

        /// <summary>
        /// Creates an new empty Text Engine.
        /// </summary>
        public EngineText() { }

        /// <summary>
        /// Creates a new Text Engine with the supplied template text and optional name.
        /// </summary>
        /// <param name="text">Template text of the new Text Engine.</param>
        /// <param name="name">Optional name for the Text Engine. (The engine's ID is derived from this name.)</param>
        public EngineText(string text, string name = null) : base(name) { Text = text; }

        /// <summary>
        /// Returns an Engine Result based on the supplied seed number.
        /// </summary>
        /// <param name="seed">The seed number which will allow the engine to repeatedly return the same 'random' result.</param>
        /// <returns>The engine's Text Result based on the seed number supplied.</returns>
        public override ResultBase Go(int seed) { return new ResultText(this, seed); }

        /// <summary>
        /// Gets the display name of this Text Engine or text piece if the engine is anonymous and no name is given.
        /// </summary>
        /// <returns>The display name of the Object Engine or text piece if the engine is anonymous and no name is given.</returns>
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
    }
}