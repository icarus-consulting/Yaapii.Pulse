namespace Yaapii.Pulse
{
    /// <summary>
    /// A dead pulse which does nothing.
    /// </summary>
    public sealed class DeadPulse : IPulse
    {
        /// <summary>
        /// A dead pulse which does nothing.
        /// </summary>
        public DeadPulse()
        { }

        public void Connect(ISensor sensor)
        { }

        public void Send(ISignal evt)
        { }
    }
}
