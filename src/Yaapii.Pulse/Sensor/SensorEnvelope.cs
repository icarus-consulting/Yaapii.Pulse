using System;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Pulse.Sensor
{
    /// <summary>
    /// An envelope for a sensor.
    /// </summary>
    public abstract class SensorEnvelope : ISensor
    {
        private readonly IScalar<ISensor> sensor;

        /// <summary>
        /// An envelope for a sensor.
        /// </summary>
        public SensorEnvelope(Func<ISensor> sensor) : this(new ScalarOf<ISensor>(sensor))
        { }

        /// <summary>
        /// An envelope for a sensor.
        /// </summary>
        public SensorEnvelope(IScalar<ISensor> sensor)
        {
            this.sensor = sensor;
        }

        public bool IsActive()
        {
            return this.sensor.Value().IsActive();
        }

        public bool IsDead()
        {
            return this.sensor.Value().IsDead();
        }

        public void Trigger(ISignal signal)
        {
            this.sensor.Value().Trigger(signal);
        }
    }
}
