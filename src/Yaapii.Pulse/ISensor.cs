namespace Yaapii.Pulse
{
    /// <summary>
    /// A sensor which reacts to a specific signal.
    /// </summary>
    public interface ISensor
    {
        /// <summary>
        /// Trigger the given signal.
        /// </summary>
        /// <param name="signal"></param>
        void Trigger(ISignal signal);

        /// <summary>
        /// Inactive sensors should be ignored.
        /// </summary>
        bool IsActive();

        /// <summary>
        /// Dead sensors should be removed.
        /// </summary>
        bool IsDead();
    }
}
