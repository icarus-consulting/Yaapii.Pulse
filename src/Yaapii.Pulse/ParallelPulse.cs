using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yaapii.Pulse
{
    /// <summary>
    /// A simple pulse which will inform active sensors.
    /// it will not cleanup unused sensors.
    /// </summary>
    public sealed class ParallelPulse : IPulse
    {
        private readonly IList<ISensor> sensors;
        private readonly TaskFactory schedule;

        /// <summary>
        /// A simple pulse which will inform active sensors.
        /// </summary>
        public ParallelPulse() : this(new List<ISensor>())
        { }

        /// <summary>
        /// A simple pulse which will inform active sensors.
        /// </summary>
        public ParallelPulse(IList<ISensor> sensors)
        {
            this.sensors = sensors;
            this.schedule = new TaskFactory();
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

            this.schedule.StartNew(() =>
            {
                var deads = new List<ISensor>();
                Parallel.ForEach(snapshot, (sensor) =>
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
                        lock (deads)
                        {
                            deads.Add(sensor);
                        }
                    }
                });
                Cleanup(deads);
            });
        }

        private void Cleanup(IList<ISensor> deadSensors)
        {
            Parallel.ForEach(deadSensors, (sensor) =>
            {
                lock (this.sensors)
                {
                    if (this.sensors.Contains(sensor))
                    {
                        this.sensors.Remove(sensor);
                    }
                }
            });
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
