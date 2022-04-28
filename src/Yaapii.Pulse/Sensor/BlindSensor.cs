namespace Yaapii.Pulse.Sensor
{
    /// <summary>
    /// A sensor which cannot sense anything.
    /// </summary>
    public sealed class BlindSensor : ISensor
    {
        public void Dispose()
        { }

        public bool IsActive()
        {
            return false;
        }

        public bool IsDead()
        {
            return true;
        }

        public void Trigger(ISignal signal)
        { }
    }
}
