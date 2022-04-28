using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yaapii.Pulse.Sensor
{
    /// <summary>
    /// Sense something in the head.
    /// </summary>
    public sealed class HeadCondition : ICondition
    {
        private readonly Func<IDictionary<string, string>, bool> sense;

        /// <summary>
        /// Sense if the key in the head matches the value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public HeadCondition(string key, string value) : this(
            key,
            v => v == value
        )
        { }

        /// <summary>
        /// Sense something in the head.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="matches"></param>
        public HeadCondition(string key, Func<string, bool> matches) : this((head) =>
            head.ContainsKey(key) && matches(head[key])
        )
        { }

        public HeadCondition(Func<IDictionary<string, string>, bool> sense)
        {
            this.sense = sense;
        }

        public bool Matches(ISignal sig)
        {
            return this.sense(sig.Head());
        }
    }
}
