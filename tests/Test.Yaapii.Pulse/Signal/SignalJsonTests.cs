using Xunit;

namespace Yaapii.Pulse.Signal.Test
{
    public sealed class SignalJsonTests
    {
        [Theory]
        [InlineData("$.head[1].key", "category")]
        [InlineData("$.head[1].value", "kater")]
        public void BuildsHead(string jsonPath, string expected)
        {
            Assert.Equal(
                expected,
                new SignalJson(
                    new SignalOf(
                        new SigHead("entity", "kater", "irrelevant")
                    )
                ).Value(jsonPath)
            );
        }

        [Theory]
        [InlineData("$.props[0].key", "go out to")]
        [InlineData("$.props[0].value", "the fraggles")]
        public void BuildsProps(string jsonPath, string expected)
        {
            Assert.Equal(
                expected,
                new SignalJson(
                    new SignalOf(
                        new SigHead("entity", "event-tests", "my-event"),
                        new SigProps("go out to", "the fraggles")
                    )
                ).Value(jsonPath)
            );
        }
    }
}
