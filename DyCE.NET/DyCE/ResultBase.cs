using System;
using System.Collections.Generic;
using Antlr4.StringTemplate;
using GalaSoft.MvvmLight;

namespace DyCE
{
    public abstract class ResultBase : ViewModelBase {
        public EngineBase Engine { get; set; }
        public string Name { get { return Engine.Name; } }
        public string DisplayName { get { return ToString(); } }
        public int Seed { get; set; }

        public ResultBase(EngineBase engine, int seed)
        {
            Engine = engine;
            Seed = seed;
            Engine.Changed += EngineChanged;
            Engine.SubscribeToChange(() => Engine.Name, NameChanged);
        }

        private void EngineChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(() => Name);
            RaisePropertyChanged(() => DisplayName);
            RaisePropertyChanged(() => SubResults);
        }

        private void NameChanged(EngineBase sender)
        {
            RaisePropertyChanged(() => Name);
            RaisePropertyChanged(() => DisplayName);
        }

        public abstract IEnumerable<ResultBase> SubResults { get; }

        public override sealed string ToString()
        {
            if (Engine.ResultTemplate == null)
                return base.ToString();

            try
            {
                var template = new Template(Engine.ResultTemplate, '$', '$');
                template.Add("this", this);
                template.Add("dyce", new ResultDB(Seed));
                template.Group.RegisterRenderer(typeof(object), new BasicFormatRenderer());
                var result = template.Render();
                return result;
            }
            catch (Exception ex)
            {
                return "*** Template Error: " + ex.Message + " ***\r\n\r\n" + Engine.ResultTemplate;
            }
        }
    }

    public class ResultList : ResultBase
    {
        private readonly EngineList _engine;

        public ResultList(EngineList engine, int seed) : base(engine, seed) { _engine = engine; }

        public ResultBase Result
        {
            get
            {
                var rand = new Random(Seed);

                if (_engine.Items.Count < 1)
                    return new ResultEmpty(_engine, rand.Next());

                int index = rand.Next(0, _engine.Items.Count);

                return _engine.Items[index].Go(rand.Next());
            }
        }

        public override IEnumerable<ResultBase> SubResults { get { return new List<ResultBase> {Result}; } }
    }
}