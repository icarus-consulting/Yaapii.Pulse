namespace Yaapii.Pulse
{
    /// <summary>
    /// A pulse where all events are sent over.
    /// </summary>
    public interface IPulse
    {
        void Send(ISignal evt);

        void Connect(ISensor sensor);
    }
}
