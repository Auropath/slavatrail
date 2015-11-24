﻿using System;

namespace TrailSimulation.Game
{
    public sealed class River : Location
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:TrailCommon.Location" /> class.
        /// </summary>
        public River(string name) : base(name)
        {
        }

        public override Mode Mode
        {
            get { return Mode.RiverCrossing; }
        }

        public int Depth
        {
            get { throw new NotImplementedException(); }
        }

        public int FerryCost
        {
            get { throw new NotImplementedException(); }
        }

        public void CrossRiver()
        {
            throw new NotImplementedException();
        }
    }
}