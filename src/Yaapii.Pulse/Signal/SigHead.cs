using System;
using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Pulse.Signal
{
    /// <summary>
    /// Head of a signal.
    /// </summary>
    public partial class SigHead : Atoms.Map.MapInputEnvelope
    {
        /// <summary>
        /// Head of a signal.
        /// </summary>
        public SigHead(string entity, string category, string eventName, params IKvp[] additional) : this(
            new Atoms.Enumerable.Joined<IKvp>(
                new ManyOf<IKvp>(
                    new KvpOf("entity", entity),
                    new KvpOf("category", category),
                    new KvpOf("event", eventName)
                ),
                additional
            )
        )
        { }
        /// <summary>
        /// Head of a signal.
        /// </summary>
        public SigHead(string entity, string category, string eventName, IEnumerable<IKvp> additional) : this(
            new Atoms.Enumerable.Joined<IKvp>(
                new ManyOf<IKvp>(
                    new KvpOf("entity", entity),
                    new KvpOf("category", category),
                    new KvpOf("event", eventName)
                ),
                additional
            )
        )
        { }

        /// <summary>
        /// Head of a signal.
        /// </summary>
        private SigHead(IEnumerable<IKvp> kvps) : base(() =>
            new MapInputOf(
                new Yaapii.Atoms.Enumerable.Joined<IKvp>(
                    kvps,
                    new KvpOf("timestamp", DateTime.Now.ToUniversalTime().ToLongTimeString())
                )
            )
        )
        { }

        /// <summary>
        /// Checks if the signal head matches.
        /// </summary>
        public sealed class Is : ScalarEnvelope<bool>
        {
            /// <summary>
            /// Checks if the signal head matches.
            /// </summary>
            public Is(ISignal sig, string entity, string category, string eventName) : base(() =>
            {
                var head = sig.Head();
                return head["entity"] == entity && head["category"] == category && head["event"] == eventName;
            })
            { }
        }

        /// <summary>
        /// Checks if the signal head matches.
        /// </summary>
        public sealed class Includes : ScalarEnvelope<bool>
        {
            /// <summary>
            /// Checks if the signal head matches.
            /// </summary>
            public Includes(ISignal sig, string key, string value)  : this(sig, new KvpOf(key, value))
            { }

            /// <summary>
            /// Checks if the signal head matches.
            /// </summary>
            public Includes(ISignal sig, params IKvp[] contents) : base(() =>
            {
                var result = true;
                var head = sig.Head();
                foreach(var content in contents)
                {
                    if(!head.ContainsKey(content.Key()) || head[content.Key()] != content.Value())
                    {
                        result = false;
                        break;
                    }
                }
                return result;
            })
            { }
        }
    }
}
