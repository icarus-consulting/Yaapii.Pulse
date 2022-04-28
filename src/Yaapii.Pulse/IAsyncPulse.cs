using System.Threading.Tasks;

namespace Yaapii.Pulse
{
    /// <summary>
    /// An async pulse where all events are sent over.
    /// </summary>
    public interface IAsyncPulse
    {
        Task Send(ISignal evt);

        void Connect(ISensor sensor);
    }
}
