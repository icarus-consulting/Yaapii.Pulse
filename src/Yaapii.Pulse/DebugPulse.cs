
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Yaapii.Atoms.Text;

namespace Yaapii.Pulse
{
    /// <summary>
    /// A pulse which periodically logs stats of the signals it transported.
    /// </summary>
    public sealed class DebugPulse : IPulse
    {
        private readonly IPulse origin;
        private readonly IDictionary<string, long> timesAbsolute;
        private readonly IDictionary<string, long> timesMean;
        private readonly IDictionary<string, long> count;
        private readonly Task reporting;

        /// <summary>
        /// A pulse which periodically logs stats of the signals it transported.
        /// </summary>
        public DebugPulse(IPulse origin)
        {
            this.origin = origin;
            this.timesAbsolute = new Dictionary<string, long>();
            this.timesMean = new Dictionary<string, long>();
            this.count = new Dictionary<string, long>();
            this.reporting =
                new TaskFactory().StartNew(() =>
                {
                    while (true)
                    {
                        Debug.WriteLine("");
                        Debug.WriteLine("#### Pulse Report ####");
                        Debug.WriteLine("");
                        foreach (var signal in this.count.Keys.ToArray())
                        {
                            var mean = this.timesMean.ContainsKey(signal) ? this.timesMean[signal].ToString() : "?";
                            var abs = this.timesAbsolute.ContainsKey(signal) ? this.timesAbsolute[signal].ToString() : "?";
                            Debug.WriteLine(
                                new Paragraph(
                                    "",
                                    $">>> [{signal}] >>>",
                                    $"  calls        : {this.count[signal]}",
                                    $"  absolute time: {abs}",
                                    $"  mean time    : {mean}"
                                ).AsString()
                            );
                        }
                        System.Threading.Thread.Sleep(1000);
                    }
                });
        }

        public void Connect(ISensor sensor)
        {
            this.origin.Connect(sensor);
        }

        public void Send(ISignal evt)
        {
            var head = $"{evt.Head()["entity"]}/{evt.Head()["category"]}/{evt.Head()["event"]}";
            if (!this.count.ContainsKey(head))
            {
                this.timesAbsolute[head] = 0;
                this.timesMean[head] = 0;
                this.count[head] = 0;
            }
            var sw = new Stopwatch();

            sw.Start();
            this.origin.Send(evt);
            sw.Stop();

            this.count[head]++;
            this.timesAbsolute[head] += sw.ElapsedMilliseconds;

            this.timesMean[head] = (this.timesMean[head] * (this.count[head] - 1) + sw.ElapsedMilliseconds) / (this.count[head]);
        }
    }
}
