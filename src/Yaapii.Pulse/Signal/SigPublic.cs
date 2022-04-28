using Yaapii.Atoms.Map;

namespace Yaapii.Pulse.Signal
{
    /// <summary>
    /// Put in signal head to make it public.
    /// </summary>
    public sealed class SigPublic : KvpEnvelope
    {
        /// <summary>
        /// Put in signal head to make it public.
        /// </summary>
        public SigPublic() : base(
            new SigScope("public")
        )
        { }
    }
}
