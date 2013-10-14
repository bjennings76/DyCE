using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Antlr4.StringTemplate;
using Antlr4.StringTemplate.Misc;
using GalaSoft.MvvmLight;

namespace DyCE
{
    public abstract class ResultBase : ViewModelBase {
        public EngineBase Engine { get; private set; }
        public string DisplayName { get { return string.Format("Results of {0}", Engine.DisplayName); } }
        public string ResultText { get { return ToString(); } }
        protected int _seed { get; private set; }
        public readonly List<ResultError> Errors = new List<ResultError>(); 

        protected ResultBase(EngineBase engine, int seed)
        {
            Engine = engine;
            _seed = seed;
            Engine.Changed += EngineChanged;
            Engine.SubscribeToChange(() => Engine.Name, NameChanged);
        }

        private void EngineChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(() => DisplayName);
            RaisePropertyChanged(() => ResultText);
            RaisePropertyChanged(() => SubResults);
        }

        private void NameChanged(EngineBase sender)
        {
            RaisePropertyChanged(() => DisplayName);
        }

        public List<ResultBase> SubResults { get { return GetAllSubResults(); } }

        private ResultDB _results;

        public string URL { get { return Engine.URL + "&seed=" + _seed; } }

        public virtual ResultBase this[string propertyName]
        {
            get
            {
                Errors.Add(new ResultError(string.Concat("Property '", propertyName, "' not found on engine ", Engine.Name)));
                return null;
            }
        }

        private List<ResultBase> GetAllSubResults()
        {
            return _results == null ? GetSubResults().ToList() : new List<ResultBase>(_results.Results.Concat(GetSubResults()));
        }

        protected virtual IEnumerable<ResultBase> GetSubResults() { return new List<ResultBase>(); }

        public override sealed string ToString()
        {
            if (Engine.ResultTemplate == null)
                return base.ToString();

            Errors.Clear();

            try
            {
                _results = new ResultDB(_seed);
                var template = new Template(Engine.ResultTemplate.Replace(".$", "$").Replace("$this$", Engine.Name), '$', '$');
                template.Add("this", this);
                template.Add("dyce", _results);
                template.Group.RegisterRenderer(typeof(object), new BasicFormatRenderer());
                StringWriter writer = new StringWriter();
                ErrorListener listener = new ErrorListener();
                template.Write(writer, listener);

                if (listener.HasErrors)
                    Errors.AddRange(listener.Errors);

                if (Errors.Any())
                    return Errors.JoinToString(" / ");

                if (_results.Results.Any())
                    RaisePropertyChanged(() => SubResults);

                return PostProcess(writer.ToString());
            }
            catch (Exception ex)
            {
                Errors.Add(new ResultError(ex, Engine.ResultTemplate));
                return Errors.JoinToString(" / ");
            }
        }


        private Regex _anRegex = new Regex(@"\[a/an\] ([aeiou])");
        private Regex _aRegex = new Regex(@"\[a/an\]");

        // TODO: Use modularized/extendable post process rules.
        private string PostProcess(string resultText)
        {
            if (!resultText.Contains("["))
                return resultText;

            // Find [a/an]
            if (resultText.Contains("[a/an]"))
            {
                resultText = _anRegex.Replace(resultText, "an $1");
                resultText = _aRegex.Replace(resultText, "a");
            }

            return resultText;
        }

        private class ErrorListener : ITemplateErrorListener
        {
            public bool HasErrors { get { return Errors.Any(); } }
            public readonly List<ResultError> Errors = new List<ResultError>(); 

            public void CompiletimeError(TemplateMessage msg) { Errors.Add(new ResultError(msg)); }

            public void RuntimeError(TemplateMessage msg) { Errors.Add(new ResultError(msg)); }

            public void IOError(TemplateMessage msg) { Errors.Add(new ResultError(msg)); }

            public void InternalError(TemplateMessage msg) { Errors.Add(new ResultError(msg)); }
        }
    }

    public class ResultError
    {
        private string _text { get; set; }
        private string _details { get; set; }

        public ResultError(string text, string details = null)
        {
            _text = text;
            _details = details;
        }

        public ResultError(Exception ex, string details = null)
        {
            _text = ex.Message;
            _details = details;
        }

        public ResultError(TemplateMessage msg)
        {
            _details = msg.ToString();
            var lines = _details.Replace("context [anonymous] 1:1 ", "").Split('\n');
            const string searchString = "System.Exception: ";
            if (lines.Any() && lines[0].Contains(searchString))
                _text = lines[0].Substring(lines[0].IndexOf(searchString) + searchString.Length);
            else if (lines.Any())
                _text = lines.FirstOrDefault();
            else
                _text = _details;
        }

        public string ErrorText
        {
            get
            {
                return String.Concat("*** Template Error: ", _text, " ***");
            }
        }

        public string VerboseErrorText
        {
            get
            {
                return _details.IsNullOrEmpty() ? ErrorText : String.Concat("*** Template Error: ", _text, " ***\r\n\r\n", _details);                
            }
        }

        public override string ToString() { return ErrorText; }
    }
}
