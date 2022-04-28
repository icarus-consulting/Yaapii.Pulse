using System;
using System.Collections.Generic;

namespace Yaapii.Pulse.Signal
{
    /// <summary>
    /// A dead signal.
    /// </summary>
    public sealed class DeadSignal : ISignal
    {
        public IDictionary<string, string> Head()
        {
            throw new InvalidOperationException($"Cannot access head of a dead signal.");
        }

        public IDictionary<string, string> Props()
        {
            throw new InvalidOperationException($"Cannot access props of a dead signal.");
        }
    }
}
