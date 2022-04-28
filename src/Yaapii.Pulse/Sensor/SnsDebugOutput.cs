using System.Diagnostics;

namespace Yaapii.Pulse.Sensor
{
    /// <summary>
    /// Writes signals to console.
    /// </summary>
    public sealed class SnsDebugOutput : ISensor
    {
        /// <summary>
        /// Writes signals to console.
        /// </summary>
        public SnsDebugOutput()
        { }

        public bool IsActive()
        {
            return true;
        }

        public bool IsDead()
        {
            return false;
        }

        public void Trigger(ISignal signal)
        {
            var head = signal.Head();
            var props = signal.Props();

            Debug.WriteLine("--- Signal ---");
            Debug.WriteLine($"{head["entity"]}/{head["category"]}/{head["name"]}");
            Debug.WriteLine(" # head");
            foreach (var key in head.Keys)
            {
                if (key != "entity" && key != "category" && key != "name")
                {
                    Debug.WriteLine($"  {key}:{head[key]}");
                }
            }
            Debug.WriteLine(" # props");
            foreach (var key in props.Keys)
            {
                Debug.WriteLine($"  {key}:{props[key]}");
            }

            Debug.WriteLine("--- /Signal ---");
        }
    }
}
