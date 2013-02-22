namespace DyCE
{
    public class ResultProperty: ResultBase
    {
        public string Name { get; set; }

        public ResultBase ValueResult { get; set; }

        public ResultProperty(EngineProperty engineObject, int seed) : base(engineObject)
        {
            Name = engineObject.Name;
            ValueResult = engineObject.ValueEngine.Go(seed);
        }

        public override string ToString() { return Name + ": " + ValueResult; }
    }
}