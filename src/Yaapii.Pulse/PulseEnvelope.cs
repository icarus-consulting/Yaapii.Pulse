using System;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Pulse
{
    /// <summary>
    /// An envelope for a pulse.
    /// </summary>
    public abstract class PulseEnvelope : IPulse
    {
        private readonly IScalar<IPulse> pulse;

        /// <summary>
        /// An envelope for a pulse.
        /// </summary>
        public PulseEnvelope(Func<IPulse> pulse) : this(new ScalarOf<IPulse>(pulse))
        { }

        /// <summary>
        /// An envelope for a pulse.
        /// </summary>
        public PulseEnvelope(IScalar<IPulse> pulse)
        {
            this.pulse = pulse;
        }

        public void Connect(ISensor sensor)
        {
            this.pulse.Value().Connect(sensor);
        }

        public void Send(ISignal evt)
        {
            this.pulse.Value().Send(evt);
        }
    }
}
