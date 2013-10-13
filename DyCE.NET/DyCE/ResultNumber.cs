using System;
using System.Collections.Generic;

namespace DyCE
{
    /// <summary>
    /// Represents a result of the Number Engine.
    /// </summary>
    public class ResultNumber : ResultBase
    {
        /// <summary>
        /// Internal reference to the parent engine. Let's us avoid casting ResultBase's Engine property.
        /// </summary>
        private readonly EngineNumber _engine;

        /// <summary>
        /// Creates a new Number Engine Result object from the supplied Number Engine and seed number.
        /// </summary>
        /// <param name="engine">Engine from which to generate the result.</param>
        /// <param name="seed">The seed number which allows this result to always return the same value.</param>
        public ResultNumber(EngineNumber engine, int seed) : base(engine, seed) { _engine = engine; }

        /// <summary>
        /// The numberical result of this Number Result object.
        /// </summary>
        public int Result { get { return new Random(_seed).Next(_engine.Min, _engine.Max + 1); } }
    }
}
