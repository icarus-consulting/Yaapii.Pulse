using System.Collections.Generic;
using Yaapii.Atoms.List;

namespace Yaapii.Pulse
{
    /// <summary>
    /// A simple pulse which will inform active sensors.
    /// </summary>
    public sealed class SyncPulse : IPulse
    {
        private readonly IList<ISensor> sensors;

        /// <summary>
        /// A simple pulse which will inform active sensors.
        /// </summary>
        public SyncPulse(params ISensor[] sensors) : this(new ListOf<ISensor>(sensors))
        { }

        /// <summary>
        /// A simple pulse which will inform active sensors.
        /// </summary>
        public SyncPulse() : this(new List<ISensor>())
        { }

        /// <summary>
        /// A simple pulse which will inform active sensors.
        /// </summary>
        public SyncPulse(ISensor sensor) : this(new LiveList<ISensor>(sensor))
        { }

        /// <summary>
        /// A simple pulse which will inform active sensors.
        /// </summary>
        public SyncPulse(IList<ISensor> sensors)
        {
            this.sensors = new List<ISensor>(sensors);
        }

        public void Connect(ISensor sensor)
        {
            this.sensors.Add(sensor);
            Cleanup(DeadSensors());
        }

        public void Send(ISignal signal)
        {
            IList<ISensor> snapshot;
            lock (this.sensors)
            {
                snapshot = new List<ISensor>(this.sensors);
            }

            var deads = new List<ISensor>();
            foreach (var sensor in snapshot)
            {
                if (!sensor.IsDead())
                {
                    if (sensor.IsActive())
                    {
                        sensor.Trigger(signal);
                    }
                }
                else
                {
                    deads.Add(sensor);
                }
            }
            Cleanup(deads);
        }

        private void Cleanup(IList<ISensor> deadSensors)
        {
            foreach(var sensor in deadSensors)
            {
                lock (this.sensors)
                {
                    if (this.sensors.Contains(sensor))
                    {
                        this.sensors.Remove(sensor);
                    }
                }
            }
        }

        private IList<ISensor> DeadSensors()
        {
            IList<ISensor> snapshot;
            lock (this.sensors)
            {
                snapshot = new List<ISensor>(this.sensors);
            }

            var dead = new List<ISensor>();
            foreach (var sensor in snapshot)
            {
                if (sensor.IsDead())
                {
                    dead.Add(sensor);
                }
            }
            return dead;
        }
    }
}
