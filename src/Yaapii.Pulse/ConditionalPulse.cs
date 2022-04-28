using System;

namespace Yaapii.Pulse
{
    /// <summary>
    /// A pulse which will only trigger sensors (if connected using this object)
    /// and signals when the given condition is true.
    /// </summary>
    public sealed class ConditionalPulse : IPulse
    {
        private readonly IPulse origin;
        private readonly Func<bool> condition;

        /// <summary>
        /// A pulse which will only trigger sensors (if connected using this object)
        /// and signals when the given condition is true.
        /// </summary>
        public ConditionalPulse(IPulse origin, Func<bool> condition)
        {
            this.origin = origin;
            this.condition = condition;
        }

        public void Connect(ISensor sensor)
        {
            this.origin.Connect(
                new ConditionalSensor(sensor, this.condition)
            );
        }

        public void Send(ISignal evt)
        {
            if (this.condition())
            {
                this.origin.Send(evt);
            }
        }
    }
}
