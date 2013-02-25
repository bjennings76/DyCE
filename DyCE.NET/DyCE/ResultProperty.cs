namespace DyCE
{
    public class ResultProperty: ResultBase
    {
        public ResultBase ValueResult { get; set; }

        public ResultProperty(EngineProperty engineObject, int seed) : base(engineObject, seed)
        {
            ValueResult = engineObject.ValueEngine.Go(seed);
        }

        public override string ToString() { return Name + ": " + ValueResult; }
    }
}