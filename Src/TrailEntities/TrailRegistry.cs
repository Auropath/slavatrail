﻿using System.Collections.Generic;
using TrailCommon;

namespace TrailEntities
{
    /// <summary>
    ///     Complete trails the player can travel on using the simulation. Some are remakes and others new.
    /// </summary>
    public static class TrailRegistry
    {
        /// <summary>
        ///     Creates the original Oregon trail which was in the game this is cloning.
        /// </summary>
        public static IEnumerable<PointOfInterest> OregonTrail()
        {
            var trail = new HashSet<PointOfInterest>
            {
                new LandmarkPoint("Independence", 0, StoreRegistry.MattsStore()),
                new RiverPoint("Kansas River Crossing", 83),
                new RiverPoint("Big Blue River Crossing", 119),
                new LandmarkPoint("Fort Kearney", 250, StoreRegistry.FortKearneyStore()),
                new LandmarkPoint("Chimney Rock", 86),
                new LandmarkPoint("Fort Laramie", 190, StoreRegistry.FortLaramieStore()),
                new LandmarkPoint("Independence Rock", 152),
                new ForkInRoadPoint("South Pass", 219, new List<PointOfInterest>
                {
                    new LandmarkPoint("Fort Bridger", 162, StoreRegistry.FortBridgerStore()),
                    new RiverPoint("Green River Shortcut", 144)
                }),
                new RiverPoint("Green River Crossing", 94),
                new LandmarkPoint("Soda Springs", 57),
                new LandmarkPoint("Fort Hall", 182, StoreRegistry.FortHallStore()),
                new RiverPoint("Snake River Crossing", 114),
                new LandmarkPoint("Fort Boise", 94, StoreRegistry.FortBoiseStore()),
                new ForkInRoadPoint("Blue Mountains", 91, new List<PointOfInterest>
                {
                    new LandmarkPoint("Fort Walla Walla", 120, StoreRegistry.FortWallaWallaStore()),
                    new LandmarkPoint("The Dalles", 146)
                }),
                new LandmarkPoint("Oregon City", 85, null, false)
            };
            return trail;
        }
    }
}