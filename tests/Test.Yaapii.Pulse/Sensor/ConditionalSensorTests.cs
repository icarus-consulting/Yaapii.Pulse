using Xunit;
using Yaapii.Pulse.Signal;

namespace Yaapii.Pulse.Sensor.Test
{
    public sealed class ConditionalSensorTests
    {
        [Fact]
        public void TriggersIfMatched()
        {
            var triggered = false;

            new ConditionalSensor(
                new FkSensor(sig => { triggered = true; }),
                new SimpleCondition(sig => true)
            ).Trigger(
                new SignalOf(
                    new SigHead("entity", "action", "KILLE KILLE")
                )
            );

            Assert.True(triggered);
        }

        [Fact]
        public void DoesNotTriggerOnNotMatching()
        {
            var triggered = false;

            new ConditionalSensor(
                new FkSensor(sig => { triggered = true; }),
                new SimpleCondition(sig => sig.Head().ContainsKey("foo"))
            ).Trigger(
                new SignalOf(
                    new SigHead("entity", "action", "KILLE KILLE")
                )
            );

            Assert.False(triggered);
        }

        [Fact]
        public void IgnoresInactiveSensors()
        {
            var triggered = false;
            new ConditionalSensor(
                new FkSensor(
                    (sig) =>
                    {
                        triggered = true;
                    },
                    alive: true,
                    active: false
                ),
                new SimpleCondition(sig => true)
            ).Trigger(
                new SignalOf(
                    new SigHead("entity", "purpose", "event name")
                )
            );
            Assert.False(triggered);
        }
    }
}
