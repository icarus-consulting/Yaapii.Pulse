using System.Collections.Generic;

namespace Yaapii.Pulse
{
    /// <summary>
    /// Sends the last <paramref name="size"/> transmitted signals to a newly connected sensor
    /// </summary>
    public sealed class ReplayedPulse : IPulse
    {
        private readonly List<ISignal> signals;
        private readonly IPulse origin;
        private readonly int size;

        /// <summary>
        /// Sends the last <paramref name="size"/> transmitted signals to a newly connected sensor
        /// </summary>
        public ReplayedPulse(IPulse origin, int size = 5)
        {
            this.signals = new List<ISignal>();
            this.origin = origin;
            this.size = size;
        }
        public void Connect(ISensor sensor)
        {
            this.origin.Connect(sensor);
            if (sensor.IsActive())
            {
                foreach (var signal in this.signals)
                {
                    sensor.Trigger(signal);
                }
            }
        }

        public void Send(ISignal evt)
        {
            if(signals.Count == this.size)
            {
                signals.RemoveAt(0);
            }
            signals.Add(evt);
            this.origin.Send(evt);
        }
    }
}
