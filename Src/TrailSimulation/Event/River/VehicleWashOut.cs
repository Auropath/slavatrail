﻿using System;
using System.Collections.Generic;
using System.Text;
using TrailSimulation.Entity;
using TrailSimulation.Game;

namespace TrailSimulation.Event
{
    /// <summary>
    ///     Player forded the river and it was to deep, they have been washed out by the current and some items destroyed.
    /// </summary>
    [DirectorEvent(EventCategory.RiverCross, false)]
    public sealed class VehicleWashOut : EventItemDestroyer
    {
        /// <summary>
        ///     Creates a new instance of an event product with the specified event type for reference purposes.
        /// </summary>
        /// <param name="category">
        ///     what type of event this will be, used for grouping and filtering and triggering events by type rather than type of.
        /// </param>
        public VehicleWashOut(EventCategory category) : base(category)
        {
        }

        /// <summary>
        ///     Fired by the item destroyer event prefab before items are destroyed.
        /// </summary>
        /// <param name="destroyedItems"></param>
        protected override string OnPostDestroyItems(IDictionary<Entities, int> destroyedItems)
        {
            return destroyedItems.Count > 0
                ? $"in the loss of:{Environment.NewLine}"
                : $"in no loss of items.{Environment.NewLine}";
        }

        /// <summary>
        ///     Fired by the item destroyer event prefab after items are destroyed.
        /// </summary>
        protected override string OnPreDestroyItems()
        {
            var _eventText = new StringBuilder();
            _eventText.AppendLine("Vehicle was washed");
            _eventText.AppendLine("out when attempting to");
            _eventText.Append("ford the river results");
            return _eventText.ToString();
        }
    }
}