using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Yaapii.Atoms.Scalar;
using Yaapii.JSON;

namespace Yaapii.Pulse.Signal
{
    /// <summary>
    /// A json of an event signal.
    /// </summary>
    public sealed class SignalJson : IJSON
    {
        private readonly ScalarOf<JToken> token;
        private readonly IJSON queryableJson;

        /// <summary>
        /// A json of an event signal.
        /// </summary>
        public SignalJson(ISignal signal)
        {
            this.token = new ScalarOf<JToken>(() =>
            {
                return
                    new JObject(
                        new JProperty("head", this.HeadJson(signal)),
                        new JProperty("props", this.PropsJson(signal))
                    );
            });
            this.queryableJson = new JSONOf(this.token);
        }

        /// <summary>
        /// A json of an event signal.
        /// </summary>
        public IJSON Node(string jsonPath)
        {
            return this.queryableJson;
        }

        /// <summary>
        /// A json of an event signal.
        /// </summary>
        public IList<IJSON> Nodes(string jsonPath)
        {
            return this.queryableJson.Nodes(jsonPath);
        }

        public JToken Token()
        {
            return this.token.Value();
        }

        public string Value(string jsonPath)
        {
            return this.queryableJson.Value(jsonPath);
        }

        public IList<string> Values(string jsonPath)
        {
            return this.queryableJson.Values(jsonPath);
        }

        private JArray PropsJson(ISignal signal)
        {
            var result = new JArray();
            foreach (var prop in signal.Props().Keys)
            {
                result.Add(
                    new JObject(
                        new JProperty("key", prop),
                        new JProperty("value", signal.Props()[prop])
                    )
                );
            }
            return result;
        }

        private JArray HeadJson(ISignal signal)
        {
            var result = new JArray();
            foreach (var prop in signal.Head().Keys)
            {
                result.Add(
                    new JObject(
                        new JProperty("key", prop),
                        new JProperty("value", signal.Head()[prop])
                    )
                );
            }
            return result;
        }
    }
}
