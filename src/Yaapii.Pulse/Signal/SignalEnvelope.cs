using System;
using System.Collections.Generic;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Pulse.Signal
{
    /// <summary>
    /// Envelope for Signals.
    /// </summary>
    public class SignalEnvelope : ISignal
    {
        private readonly ScalarOf<ISignal> signal;

        /// <summary>
        /// Envelope for Signals.
        /// </summary>
        public SignalEnvelope(Func<ISignal> signal)
        {
            this.signal = new ScalarOf<ISignal>(signal);
        }

        public IDictionary<string, string> Head()
        {
            return this.signal.Value().Head();
        }

        public IDictionary<string, string> Props()
        {
            return this.signal.Value().Props();
        }
    }
}
