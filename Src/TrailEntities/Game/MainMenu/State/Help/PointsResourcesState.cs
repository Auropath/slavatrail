﻿using System;
using System.Text;
using TrailEntities.Scoring;
using TrailEntities.Simulation;
using TrailEntities.Widget;

namespace TrailEntities.Game
{
    /// <summary>
    ///     Second panel on point information, shows how the number of resources you end the game with contribute to your final
    ///     score.
    /// </summary>
    public sealed class PointsResourcesState : DialogState<MainMenuInfo>
    {
        /// <summary>
        ///     This constructor will be used by the other one
        /// </summary>
        public PointsResourcesState(IModeProduct gameMode, MainMenuInfo userData) : base(gameMode, userData)
        {
        }

        /// <summary>
        ///     Defines what type of dialog this will act like depending on this enumeration value. Up to implementation to define
        ///     desired behavior.
        /// </summary>
        protected override DialogType DialogType
        {
            get { return DialogType.Prompt; }
        }

        /// <summary>
        ///     Fired when dialog prompt is attached to active game mode and would like to have a string returned.
        /// </summary>
        protected override string OnDialogPrompt()
        {
            var _pointsItems = new StringBuilder();
            _pointsItems.Append($"{Environment.NewLine}On Arriving in Oregon{Environment.NewLine}{Environment.NewLine}");
            _pointsItems.Append($"The resources you arrive with will{Environment.NewLine}");
            _pointsItems.Append($"help you get started in the new{Environment.NewLine}");
            _pointsItems.Append($"land. You receive points for each{Environment.NewLine}");
            _pointsItems.Append($"item you bring safely to Oregon.{Environment.NewLine}{Environment.NewLine}");

            // Build up the table of resource points and how they work for player.
            var partyTable = ScoreRegistry.ResourcePoints.ToStringTable(
                new[] {"Resources of Party", "Points per SimulationItem"},
                u => u.ToString(),
                u => u.PointsAwarded
                );

            // Print the table of how resources earn points.
            _pointsItems.AppendLine(partyTable);
            return _pointsItems.ToString();
        }

        /// <summary>
        ///     Fired when the dialog receives favorable input and determines a response based on this. From this method it is
        ///     common to attach another state, or remove the current state based on the response.
        /// </summary>
        /// <param name="reponse">The response the dialog parsed from simulation input buffer.</param>
        protected override void OnDialogResponse(DialogResponse reponse)
        {
            //ParentMode.CurrentState = new PointsOccupationState(ParentMode, UserData);
            SetState(typeof (PointsOccupationState));
        }
    }
}