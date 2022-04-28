using System.Collections.Generic;

namespace Yaapii.Pulse
{
    /// <summary>
    /// A signal which is sent over the pulse and can be read by sensors.
    /// </summary>
    public interface ISignal
    {
        IDictionary<string, string> Head();
        IDictionary<string, string> Props();
    }
}
