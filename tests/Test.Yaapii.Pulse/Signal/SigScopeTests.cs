using Xunit;
using Yaapii.Pulse.Sensor;

namespace Yaapii.Pulse.Signal.Test
{
    public sealed class SigScopeTests
    {
        [Fact]
        public void AddsAccessKey()
        {
            Assert.Equal(
                "public",
                new SignalOf(
                    new SigHead("test1", "test2", "test3",
                        new SigPublic()
                    )
                ).Head()["scope"]
            );
        }

        [Fact]
        public void IsRecognizedByCondition()
        {
            Assert.True(
                new HeadCondition("scope", "public").Matches(
                    new SignalOf(
                        new SigHead("test1", "test2", "test3",
                            new SigPublic()
                        )
                    )
                )
            );
        }
    }
}
