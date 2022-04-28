namespace Yaapii.Pulse.Sensor
{
    /// <summary>
    /// Senses something in the signal.
    /// </summary>
    public interface ICondition
    {
        bool Matches(ISignal sig);
    }
}
