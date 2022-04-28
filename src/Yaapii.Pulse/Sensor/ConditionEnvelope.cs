using System;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Pulse.Sensor
{
    /// <summary>
    /// An envelope for a signal condition.
    /// </summary>
    public abstract class ConditionEnvelope : ICondition
    {
        private readonly IScalar<ICondition> condition;

        /// <summary>
        /// An envelope for a signal condition.
        /// </summary>
        public ConditionEnvelope(Func<ICondition> condition) : this(new ScalarOf<ICondition>(condition))
        { }

        /// <summary>
        /// An envelope for a signal condition.
        /// </summary>
        public ConditionEnvelope(IScalar<ICondition> condition)
        {
            this.condition = condition;
        }

        public bool Matches(ISignal sig)
        {
            return this.condition.Value().Matches(sig);
        }
    }
}
