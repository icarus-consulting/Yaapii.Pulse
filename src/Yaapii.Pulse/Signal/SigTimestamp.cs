using System;
using System.Globalization;
using Yaapii.Atoms.Map;

namespace Yaapii.Pulse.Signal
{
    /// <summary>
    /// Puts the current timestamp into the signal.
    /// </summary>
    public sealed class SigTimestamp : KvpEnvelope
    {
        /// <summary>
        /// Puts the current timestamp into the signal.
        /// "public" signals will be broadcasted to the outside by <see cref="SigRBroadcastSensor"/>.
        /// </summary>
        public SigTimestamp() : base(
            new KvpOf("timestamp", DateTime.Now.ToString(CultureInfo.InvariantCulture))
        )
        { }
    }
}
