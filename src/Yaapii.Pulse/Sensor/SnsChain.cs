using System.Collections.Generic;
using Yaapii.Atoms.Enumerable;

namespace Yaapii.Pulse.Sensor
{
    /// <summary>
    /// A chain of sensors, which triggers every given sensor on receiving a signal.
    /// </summary>
    public sealed class SnsChain : ISensor
    {
        private readonly IEnumerable<ISensor> sensors;

        /// <summary>
        /// A chain of sensors, which triggers every given sensor on receiving a signal.
        /// </summary>
        public SnsChain(ISensor first, params ISensor[] additional) : this(new Joined<ISensor>(new ManyOf<ISensor>(first), additional))
        { }

        /// <summary>
        /// A chain of sensors, which triggers every given sensor on receiving a signal.
        /// </summary>
        public SnsChain(params ISensor[] sensors) : this(new ManyOf<ISensor>(sensors))
        { }

        /// <summary>
        /// A chain of sensors, which triggers every given sensor on receiving a signal.
        /// </summary>
        public SnsChain(IEnumerable<ISensor> sensors)
        {
            this.sensors = sensors;
        }

        public bool IsActive()
        {
            return true;
        }

        public bool IsDead()
        {
            return false;
        }

        public void Trigger(ISignal signal)
        {
            foreach(var sensor in this.sensors)
            {
                if (sensor.IsActive())
                {
                    sensor.Trigger(signal);
                }
            }
        }
    }
}
