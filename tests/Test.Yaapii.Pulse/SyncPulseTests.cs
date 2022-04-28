using System;
using System.Threading.Tasks;
using Xunit;
using Yaapii.Atoms.List;
using Yaapii.Pulse;
using Yaapii.Pulse.Sensor;
using Yaapii.Pulse.Signal;

namespace Yaapii.API.Tmx.Pulse.Test
{
    public class SyncPulseTests
    {
        [Fact]
        public void TriggersSensors()
        {
            var triggered = false;

            new SyncPulse(
                new LiveList<ISensor>(
                    new FkSensor(sig => triggered = true)
                )
            ).Send(
                new SignalOf(
                    new SigHead("entity", "purpose", "try to trigger")
                )
            );

            Assert.True(triggered);
        }

        [Fact]
        public void DoesNotTriggerDead()
        {
            var triggered = false;

            new SyncPulse(
                new LiveList<ISensor>(
                    new FkSensor(sig => { triggered = true; }, false)
                )
            ).Send(
                new SignalOf(
                    new SigHead("entity", "purpose", "try to trigger")
                )
            );

            Assert.False(triggered);
        }

        [Fact]
        public void RemovesInactive()
        {
            var triggered = 0;
            var sensors =
                new LiveList<ISensor>(
                    new FkSensor(sig => { triggered++; }, () => triggered == 0)
                );

            var pulse = new SyncPulse(sensors);
            pulse.Send(
                new SignalOf(
                    new SigHead("entity", "purpose", "trigger removal")
                )
            );

            pulse.Send(
                new SignalOf(
                    new SigHead("entity", "purpose", "ensure its not triggered")
                )
            );

            Assert.Equal(1, triggered);
        }

        [Fact]
        public void IgnoresInactiveSensors()
        {
            var triggered = false;
            new SyncPulse(
                new ListOf<ISensor>(
                    new FkSensor(
                        (sig) =>
                        {
                            triggered = true;
                        },
                        alive: true,
                        active: false
                    )
                )
            ).Send(
                new SignalOf(
                    new SigHead("entity", "purpose", "event name")
                )
            );
            Assert.False(triggered);
        }

        [Fact]
        public void CleansUpOnConnect()
        {
            var result = "";
            var deadSensor =
                new FkSensor(
                    (sig) => {
                        result += "a";
                    },
                    alive: () => false
                );
            var aliveSensor =
                new FkSensor(
                    (sig) => {
                        result += "b";
                    },
                    alive: () => true
                );
            var pulse = new SyncPulse();
            pulse.Connect(deadSensor);
            pulse.Connect(aliveSensor);
            pulse.Send(
                new SignalOf(
                    new SigHead("entity", "purpose", "event name")
                )
            );
            Assert.Equal("b", result);
        }
    }
}
