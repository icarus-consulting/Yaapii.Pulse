using Yaapii.Atoms.Map;

namespace Yaapii.Pulse.Signal
{
    /// <summary>
    /// Put in signal head to make it yaapii internal.
    /// </summary>
    public sealed class SigInternal : KvpEnvelope
    {
        /// <summary>
        /// Put in signal head to make it yaapii internal.
        /// </summary>
        public SigInternal() : base(
            new SigScope("internal")
        )
        { }
    }
}
