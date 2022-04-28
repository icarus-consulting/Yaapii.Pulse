using System;

namespace Yaapii.Pulse.Sensor
{
    /// <summary>
    /// A sensor which forwards a signal to a given action.
    /// </summary>
    public sealed class SnsAct : ISensor
    {
        private readonly Action<ISignal> act;
        private readonly Func<bool> isAlive;
        private readonly Func<bool> isActive;

        /// <summary>
        /// A sensor which forwards a signal to a given action.
        /// </summary>
        public SnsAct(Action<ISignal> act) : this(act, () => true, () => true)
        { }

        /// <summary>
        /// A sensor which forwards a signal to a given action.
        /// </summary>
        public SnsAct(Action<ISignal> act, Func<bool> isAlive) : this(act, isAlive, isAlive)
        { }

        /// <summary>
        /// A sensor which forwards a signal to a given action.
        /// </summary>
        public SnsAct(Action<ISignal> act, Func<bool> isAlive, Func<bool> isActive)
        {
            this.act = act;
            this.isAlive = isAlive;
            this.isActive = isActive;
        }

        public bool IsActive()
        {
            return this.isActive();
        }

        public bool IsDead()
        {
            return !this.isAlive();
        }

        public void Trigger(ISignal signal)
        {
            this.act(signal);
        }
    }
}
