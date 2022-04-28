using System;

namespace Yaapii.Pulse.Sensor
{
    /// <summary>
    /// A simple condition.
    /// </summary>
    public sealed class SimpleCondition : ICondition
    {
        private readonly Func<ISignal, bool> result;

        /// <summary>
        /// A fake condition.
        /// </summary>
        public SimpleCondition(Func<ISignal, bool> result)
        {
            this.result = result;
        }

        public bool Matches(ISignal sig)
        {
            return this.result(sig);
        }
    }
}
