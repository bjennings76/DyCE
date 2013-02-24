namespace DyCE
{
    public class EngineText : EngineBase {
        public string Text { get; set; }

        public EngineText(string text) { Text = text; }

        public override ResultBase Go(int seed)
        {
            //TODO: Parse text for sub-engines and replace with results.
            return new ResultText(this, Text, seed);
        }

        public override string ToString() { return "\"" + Text + "\""; }
    }
}