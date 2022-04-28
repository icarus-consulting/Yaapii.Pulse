using System;
using System.Collections.Generic;
using Yaapii.Atoms.Map;
using Yaapii.Pulse.Signal;

namespace Yaapii.Pulse
{
    /// <summary>
    /// A simple signal.
    /// </summary>
    [Serializable]
    public sealed class SignalOf : ISignal
    {
        private readonly IDictionary<string, string> head;
        private readonly IDictionary<string, string> props;

        /// <summary>
        /// A simple signal.
        /// </summary>
        public SignalOf(SigHead head) : this(head, new MapInputOf())
        { }

        /// <summary>
        /// A simple signal.
        /// </summary>
        public SignalOf(IMapInput head, IMapInput props)
        {
            this.head = new MapOf(head);
            this.props = new MapOf(props);
        }

        public IDictionary<string, string> Head()
        {
            return this.head;
        }

        public IDictionary<string, string> Props()
        {
            return this.props;
        }
    }
}
