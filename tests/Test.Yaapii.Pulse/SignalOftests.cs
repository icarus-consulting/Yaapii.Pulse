using Xunit;
using Yaapii.Atoms.Map;
using Yaapii.Pulse;

namespace Yaapii.API.Tmx.Pulse.Test
{
    public sealed class SignalOfTests
    {
        [Fact]
        public void BuildsHead()
        {
            Assert.Equal(
                "big",
                new SignalOf(
                    new MapInputOf(new KvpOf("head", "big")),
                    new MapInputOf()
                ).Head()["head"]
            );
        }

        [Fact]
        public void BuildsProps()
        {
            Assert.Equal(
                "erty",
                new SignalOf(
                    new MapInputOf(),
                    new MapInputOf(new KvpOf("prop", "erty"))
                ).Props()["prop"]
            );
        }
    }
}
