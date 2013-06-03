using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace DyCE
{
    /// <summary>
    /// Random number engine.
    /// </summary>
    public class EngineNumber : EngineBase
    {
        /// <summary>
        /// Internal minimum number.
        /// </summary>
        private int _min = 1;

        /// <summary>
        /// Internal maximum number.
        /// </summary>
        private int _max = 10;

        /// <summary>
        /// Minimum number of range. Defaults to '1'.
        /// </summary>
        [XmlAttribute, DefaultValue(1)]
        public int Min
        {
            get { return _min; } 
            set
            {
                _min = value;

                if (_min > _max)
                    Max = _min;

                RaisePropertyChanged(() => Min);
                RaisePropertyChanged(() => DisplayName);
                RaiseEngineChanged();
            }
        }

        /// <summary>
        /// Maximum number of range. Defaults to '10'.
        /// </summary>
        [XmlAttribute, DefaultValue(10)]
        public int Max
        {
            get { return _max; } 
            set
            {
                _max = value;

                if (_max < _min)
                    Min = _max;

                RaisePropertyChanged(() => Max);
                RaisePropertyChanged(() => DisplayName);
                RaiseEngineChanged();
                _limitDelay.Start();
            }
        }

        private readonly DispatcherTimer _limitDelay;

        /// <summary>
        /// Variable that olds the current maximum display amount for the slider UI.
        /// </summary>
        private int _sliderMax = 100;

        private const int _sliderLimit = 1000000;

        /// <summary>
        /// Returns the dynamicly adjusted range for slider UI.
        /// </summary>
        public int SliderMax
        {
            get
            {
                while (_max < _sliderMax/10 && _sliderMax > 10)
                {
                    _sliderMax = Math.Max(_sliderMax/10, 10);

                    if (_sliderMax <= 0)
                        Debugger.Break();

                }
                while (_max >= _sliderMax && _sliderMax < _sliderLimit)
                {
                    int newVal = Math.Min(_sliderLimit, _sliderMax * 10);

                    if (newVal <= 0)
                        Debugger.Break();

                    _sliderMax = newVal;
                }

                return _sliderMax;
            }
        }

        /// <summary>
        /// Returns the tick frequency for slider UI.
        /// </summary>
        public int SliderTicks { get { return _sliderMax/10; } }

        /// <summary>
        /// Returns a list of sub-engines. There are no sub-engines for a random number range.
        /// </summary>
        public override IEnumerable<EngineBase> SubEngines { get { return null; } }
        
        /// <summary>
        /// Creates a new empty Number Engine.
        /// </summary>
        public EngineNumber()
        {
            _limitDelay = new DispatcherTimer();
            _limitDelay.Interval = TimeSpan.FromSeconds(1);
            _limitDelay.Tick += _limitDelay_Tick;
        }

        /// <summary>
        /// Creates a new Number Engine with the supplied name and optional min and max values.
        /// </summary>
        /// <param name="name">The name of the new engine. Will also be used for the engine's reference ID.</param>
        /// <param name="max">The maximum range for the random numer. (Optional, defaults to '10'.)</param>
        public EngineNumber(string name, int max = 10):base(name)
        {
            _max = max;
            _limitDelay = new DispatcherTimer();
            _limitDelay.Interval = TimeSpan.FromSeconds(1);
            _limitDelay.Tick += _limitDelay_Tick;
        }

        /// <summary>
        /// Creates a new Number Engine with the supplied name and optional min and max values.
        /// </summary>
        /// <param name="name">The name of the new engine. Will also be used for the engine's reference ID.</param>
        /// <param name="min">The minimum range for the random number.</param>
        /// <param name="max">The maximum range for the random number.</param>
        public EngineNumber(string name, int min, int max):this(name, max)
        {
            _min = min;
        }

        void _limitDelay_Tick(object sender, EventArgs e)
        {
            _limitDelay.Stop();
            RaisePropertyChanged(() => SliderMax);
            RaisePropertyChanged(() => SliderTicks);
        }

        /// <summary>
        /// Returns the actual final number based on a supplied seed number.
        /// </summary>
        /// <param name="seed">The seed number which will allow the engine to repeatedly return the same 'random' result.</param>
        /// <returns>The final number based on the supplied seed number.</returns>
        public int GoNum(int seed) { return new ResultNumber(this, seed).Result; }

        /// <summary>
        /// Returns the engine's Number Result based on the supplied seed number.
        /// </summary>
        /// <param name="seed">The seed number which will allow the engine to repeatedly return the same 'random' result.</param>
        /// <returns>The engine's Number Result object based on the seed number supplied.</returns>
        public override ResultBase Go(int seed) { return new ResultNumber(this, seed); }

        /// <summary>
        /// Returns the name of this engine or, if anonymous, the range of the engine.
        /// </summary>
        /// <returns>The name or, if anonymous, text showing the range of the Number Engine.</returns>
        public override string ToString() { return Name ?? "Number Range: " + Min + " to " + Max; }
    }
}