using System;
using System.Collections.Generic;

namespace Yaapii.Pulse.Sensor
{
    /// <summary>
    /// Sense something in the props.
    /// </summary>
    public sealed class PropCondition : ICondition
    {
        private readonly Func<IDictionary<string, string>, bool> sense;

        /// <summary>
        /// Sense something in the props.
        /// </summary>
        public PropCondition(string key, Func<string, bool> matches) : this((head) =>
            head.ContainsKey(key) && matches(head[key])
        )
        { }

        public PropCondition(Func<IDictionary<string, string>, bool> sense)
        {
            this.sense = sense;
        }

        public bool Matches(ISignal sig)
        {
            return this.sense(sig.Props());
        }
    }
}
