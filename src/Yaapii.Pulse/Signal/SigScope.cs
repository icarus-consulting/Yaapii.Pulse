using Yaapii.Atoms.Map;

namespace Yaapii.Pulse.Signal
{
    /// <summary>
    /// Sets the 'scope' for the signal.
    /// "public" signals will be broadcasted to the outside by <see cref="SigRBroadcastSensor"/>.
    /// </summary>
    public sealed class SigScope : KvpEnvelope
    {
        /// <summary>
        /// Sets the 'scope' for the signal.
        /// "public" signals will be broadcasted to the outside by <see cref="SigRBroadcastSensor"/>.
        /// </summary>
        public SigScope(string scope) : base(
            new KvpOf("scope", scope)
        )
        { }
    }
}
