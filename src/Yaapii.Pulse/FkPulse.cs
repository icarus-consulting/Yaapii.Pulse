using System;
using System.Collections.Generic;
using System.Text;

namespace Yaapii.Pulse
{
    /// <summary>
    /// A fake pulse
    /// </summary>
    public sealed class FkPulse : IPulse
    {
        private readonly Action<ISensor> connect;
        private readonly Action<ISignal> send;

        /// <summary>
        /// A fake pulse
        /// </summary>
        public FkPulse() : this(
            sensor => { },
            signal => { }
        )
        { }

        /// <summary>
        /// A fake pulse
        /// </summary>
        public FkPulse(Action<ISensor> connect) : this(
            connect,
            signal => { }
        )
        { }

        /// <summary>
        /// A fake pulse
        /// </summary>
        public FkPulse(Action<ISignal> send) : this(
            sensor => { },
            send
        )
        { }

        /// <summary>
        /// A fake pulse
        /// </summary>
        public FkPulse(Action<ISensor> connect, Action<ISignal> send)
        {
            this.connect = connect;
            this.send = send;
        }

        public void Connect(ISensor sensor)
        {
            this.connect(sensor);
        }

        public void Send(ISignal evt)
        {
            this.send(evt);
        }
    }
}
