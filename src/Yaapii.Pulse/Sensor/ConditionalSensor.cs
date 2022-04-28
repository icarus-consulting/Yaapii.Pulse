using System;
using System.Collections.Generic;
using Yaapii.Atoms.Enumerable;
using Yaapii.Pulse.Sensor;

namespace Yaapii.Pulse
{
    /// <summary>
    /// A sensor which only reacts if the given senses match.
    /// </summary>
    public sealed class ConditionalSensor : ISensor
    {
        private readonly ISensor origin;
        private readonly IEnumerable<ICondition> conditions;



        /// <summary>
        /// A sensor which only reacts if the given senses match.
        /// </summary>
        public ConditionalSensor(ISensor origin, Func<bool> condition, params ICondition[] additionalConditions) : this(
            origin, new SimpleCondition(sig => condition()), additionalConditions
        )
        { }


        /// <summary>
        /// A sensor which only reacts if the given senses match.
        /// </summary>
        public ConditionalSensor(ISensor origin, Func<ISignal, bool> condition, params ICondition[] additionalConditions) : this(
            origin, new SimpleCondition(condition), additionalConditions
        )
        { }

        /// <summary>
        /// A sensor which only reacts if the given senses match.
        /// </summary>
        public ConditionalSensor(ISensor origin, ICondition condition, params ICondition[] additionalConditions)
        {
            this.origin = origin;
            this.conditions =
                new Joined<ICondition>(
                    new ManyOf<ICondition>(
                        condition
                    ),
                    additionalConditions
                );
        }

        public bool IsActive()
        {
            return true;
        }

        public bool IsDead()
        {
            return this.origin.IsDead();
        }

        public void Trigger(ISignal signal)
        {
            if (!this.origin.IsDead() && this.origin.IsActive())
            {
                var passes = true;
                foreach (var condition in this.conditions)
                {
                    passes = condition.Matches(signal);
                    if (!passes)
                    {
                        break;
                    }
                }

                if (passes)
                {
                    this.origin.Trigger(signal);
                }
            }
        }
    }
}
