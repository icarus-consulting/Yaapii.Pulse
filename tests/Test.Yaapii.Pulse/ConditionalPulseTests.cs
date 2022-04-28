using Xunit;
using Yaapii.Pulse.Sensor;
using Yaapii.Pulse.Signal;

namespace Yaapii.Pulse.Test
{
    public sealed class ConditionalPulseTests
    {
        [Fact]
        public void DelegatesSignals()
        {
            var result = false;

            var pulse = new SyncPulse();
            var conditional = new ConditionalPulse(pulse, () => true);
            conditional.Connect(new SnsAct((sig) => result = true));
            conditional.Send(
                new SignalOf(
                    new SigHead("test", "check", "microphone"))
                );

            Assert.True(result);
        }

        [Fact]
        public void PreventsDelegationIfInactive()
        {
            var result = false;

            var pulse = new SyncPulse();
            var conditional = new ConditionalPulse(pulse, () => false);
            conditional.Connect(new SnsAct((sig) => result = true));
            conditional.Send(
                new SignalOf(
                    new SigHead("test", "check", "microphone"))
                );

            Assert.False(result);
        }
    }
}
