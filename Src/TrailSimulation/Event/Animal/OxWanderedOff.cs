﻿using System;
using TrailSimulation.Entity;
using TrailSimulation.Game;

namespace TrailSimulation.Event
{
    [DirectorEvent(EventCategory.Animal)]
    public sealed class OxWanderedOff : EventProduct
    {
        /// <summary>
        ///     Creates a new instance of an event product with the specified event type for reference purposes.
        /// </summary>
        /// <param name="category">
        ///     what type of event this will be, used for grouping and filtering and triggering events by type rather than type of.
        /// </param>
        public OxWanderedOff(EventCategory category) : base(category)
        {
        }

        /// <summary>
        ///     Fired when the event handler associated with this enum type triggers action on target entity. Implementation is
        ///     left completely up to handler.
        /// </summary>
        /// <param name="sourceEntity">
        ///     Entities which the event is going to directly affect. This way there is no confusion about
        ///     what entity the event is for. Will require casting to correct instance type from interface instance.
        /// </param>
        public override void Execute(IEntity sourceEntity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Fired when the simulation would like to render the event, typically this is done AFTER executing it but this could
        ///     change depending on requirements of the implementation.
        /// </summary>
        /// <returns>Text user interface string that can be used to explain what the event did when executed.</returns>
        protected override string OnRender()
        {
            throw new NotImplementedException();
        }
    }
}