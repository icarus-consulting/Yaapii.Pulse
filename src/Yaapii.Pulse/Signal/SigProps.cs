using System;
using System.Collections.Generic;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Pulse.Signal
{
    /// <summary>
    /// Props of a signal.
    /// </summary>
    public sealed class SigProps : MapInputEnvelope
    {
        public SigProps(IDictionary<string, string> props) : this(
            new Mapped<string, IKvp>(
                key => new KvpOf(key, () => props[key]),
                props.Keys
            )
        )
        { }

        /// <summary>
        /// Props of a signal.
        /// </summary>
        public SigProps(params string[] props) : this(
            new ManyOf<IKvp>(() =>
            {
                if (props.Length % 2 != 0)
                {
                    throw new ArgumentException($"You must specify an even number of props, because props are always key: value.");
                }
                IList<IKvp> result = new List<IKvp>();
                for (var i = 0; i < props.Length; i++)
                {
                    result.Add(new KvpOf(props[i], props[++i]));
                }
                return result;
            })
        )
        { }

        /// <summary>
        /// Props of a signal.
        /// </summary>
        public SigProps(IKvp prop, params IKvp[] additional) : this(new Atoms.Enumerable.Joined<IKvp>(additional, prop))
        { }

        /// <summary>
        /// Props of a signal.
        /// </summary>
        /// <param name="props"></param>
        public SigProps(IEnumerable<IKvp> props) : base(() =>
             new MapInputOf(
                 new Atoms.Enumerable.Joined<IKvp>(
                     props
                 )
             )
        )
        { }

        /// <summary>
        /// Checks if the signal props inlcude given props.
        /// </summary>
        public sealed class Includes : ScalarEnvelope<bool>
        {
            /// <summary>
            /// Checks if the signal props inlcude given props.
            /// </summary>
            public Includes(ISignal sig, string key, string value) : this(
                sig, new KvpOf(key, value)
            )
            { }

            /// <summary>
            /// Checks if the signal props inlcude given props.
            /// </summary>
            public Includes(ISignal sig, params IKvp[] contents) : base(() =>
            {
                var result = true;
                var props = sig.Props();
                foreach (var content in contents)
                {
                    if (!props.ContainsKey(content.Key()) || props[content.Key()] != content.Value())
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
