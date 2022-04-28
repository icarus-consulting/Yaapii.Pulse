using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.List;
using Yaapii.Pulse.Sensor;
using Yaapii.Pulse.Signal;

#pragma warning disable MaxPublicMethodCount // a public methods count maximum

namespace Yaapii.Pulse.Test
{

    public class ParallelPulseTests
    {
        [Fact]
        public void TriggersSensors()
        {
            var triggered = false;
            var finish = new Task(() => { });

            new ParallelPulse(
                new LiveList<ISensor>(
                    new FkSensor(sig => { triggered = true; finish.Start(); })
                )
            ).Send(
                new SignalOf(
                    new SigHead("entity", "purpose", "try to trigger")
                )
            );

            finish.Wait();

            Assert.True(triggered);
        }

        [Fact]
        public void IsThreadSafe()
        {
            var triggered = false;
            var finished = new Task(() => { });
            var sensors =
                new List<ISensor>(
                    new Repeated<ISensor>(
                        new FkSensor(
                            sig => { triggered = true; if (finished.Status == TaskStatus.Created) finished.Start(); },
                            () =>
                            {
                                return
                                    new Random(
                                        BitConverter.ToInt32(
                                            Guid.NewGuid().ToByteArray(),
                                            0
                                        )
                                    ).Next(0, 100) > 10;
                            }
                        ),
                        1024
                    )
                );

            var pulse =
                new ParallelPulse(
                    sensors
                );

            Parallel.For(0, 256, (idx) =>
                pulse.Send(
                    new SignalOf(
                        new SigHead("entity", "purpose", "try to trigger")
                    )
                )
            );

            finished.Wait(60000);

            Assert.True(triggered);
        }

        [Fact]
        public void SlowSensorsDontBlock()
        {
            int triggered = 0;
            var finish = new Task(() => { });
            var sensors =
                new List<ISensor>(
                    new ManyOf<ISensor>(
                        new FkSensor(sig => System.Threading.Thread.Sleep(120000)),
                        new FkSensor(sig => { triggered++; finish.Start(); })
                    )
                );

            var pulse =
                new ParallelPulse(
                    sensors
                );

            pulse.Send(
                new SignalOf(
                    new SigHead("entity", "purpose", "try to trigger")
                )
            );

            finish.Wait();

            Assert.Equal(1, triggered);
        }

        [Fact]
        public void WorksParallel()
        {
            List<int> threads = new List<int>();
            var finished = new Task(() => { });

            var pulse =
                new ParallelPulse(
                    new LiveList<ISensor>(
                        new Repeated<ISensor>(
                            new FkSensor(sig =>
                            {
                                lock (threads)
                                {
                                    var thread = System.Threading.Thread.CurrentThread.ManagedThreadId;
                                    if (!threads.Contains(thread))
                                    {
                                        threads.Add(thread);
                                        finished.Start();
                                    }
                                }
                            }),
                            1024
                        )
                    )
                );

            Parallel.For(0, 256, (idx) =>
            {
                pulse.Send(
                    new SignalOf(
                        new SigHead("entity", "purpose", "try to trigger")
                    )
                );
            });

            finished.Wait(new TimeSpan(0, 0, 60));

            Assert.InRange(threads.Count, 1, int.MaxValue);
        }

        [Fact]
        public void DoesNotTriggerDead()
        {
            var triggered = false;

            new ParallelPulse(
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
            var active = true;
            var wait = new Task(() => { });
            var sensors =
                new List<ISensor>()
                {
                    new FkSensor(sig => { triggered++; active = false; wait.Start(); }, () => active)
                };

            var pulse = new ParallelPulse(sensors);
            pulse.Send(
                new SignalOf(
                    new SigHead("entity", "purpose", "try to trigger")
                )
            );

            wait.Wait();
            wait = new Task(() => { });

            pulse.Send(
                new SignalOf(
                    new SigHead("entity", "purpose", "ensure its not triggered")
                )
            );
            wait.Wait(1000);

            Assert.Equal(1, triggered);
        }

        [Fact]
        public void ChecksSensorActive()
        {
            var done = false;
            var wait = new Task(() => { });
            new ParallelPulse(
                new ListOf<ISensor>(
                    new FkSensor(
                        (sig) => { },
                        alive: () => true,
                        active: () =>
                        {
                            done = true;
                            wait.Start();
                            return false;
                        }
                    )
                )
            ).Send(
                new SignalOf(
                    new SigHead("entity", "purpose", "event name")
                )
            );
            wait.Wait(new TimeSpan(0, 1, 0));
            Assert.True(done);
        }

        [Fact]
        public void CleansUpOnConnect()
        {
            var result = "";
            var wait = new Task(() => { });
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
                        wait.Start();
                    },
                    alive: () => true
                );
            var pulse = new ParallelPulse();
            pulse.Connect(deadSensor);
            pulse.Connect(aliveSensor);
            pulse.Send(
                new SignalOf(
                    new SigHead("entity", "purpose", "event name")
                )
            );
            wait.Wait(new TimeSpan(0, 1, 0));
            Assert.Equal("b", result);
        }
    }
}
