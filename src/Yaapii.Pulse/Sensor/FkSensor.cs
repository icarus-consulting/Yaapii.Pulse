using System;

namespace Yaapii.Pulse.Sensor
{
    /// <summary>
    /// A fake sensor.
    /// </summary>
    public sealed class FkSensor : ISensor
    {
        private readonly Action<ISignal> trigger;
        private readonly Func<bool> alive;
        private readonly Func<bool> active;

        /// <summary>
        /// A fake sensor.
        /// </summary>
        public FkSensor(Action<ISignal> trigger, bool alive = true, bool active = true) : this(
            trigger,
            () => alive,
            () => active
        )
        { }

        /// <summary>
        /// A fake sensor.
        /// </summary>
        public FkSensor(Action<ISignal> trigger, Func<bool> alive) : this(
            trigger,
            alive,
            alive
        )
        { }

        /// <summary>
        /// A fake sensor.
        /// </summary>
        public FkSensor(Action<ISignal> trigger, Func<bool> alive, Func<bool> active)
        {
            this.trigger = trigger;
            this.alive = alive;
            this.active = active;
        }

        public bool IsActive()
        {
            return this.active();
        }

        public bool IsDead()
        {
            return !this.alive();
        }

        public void Trigger(ISignal signal)
        {
            this.trigger(signal);
        }
    }
}
