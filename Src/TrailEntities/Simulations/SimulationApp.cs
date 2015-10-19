﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using TrailCommon;

namespace TrailEntities
{
    public abstract class SimulationApp : ISimulation
    {
        private readonly ClientPipe _client;
        private readonly Randomizer _random;
        private readonly ServerPipe _server;
        private readonly Timer _tickTimer;
        private List<IMode> _modes;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:TrailGame.SimulationApp" /> class.
        /// </summary>
        protected SimulationApp()
        {
            _random = new Randomizer((int) DateTime.Now.Ticks & 0x0000FFF);
            TotalTicks = 0;
            TickPhase = "*";
            _modes = new List<IMode>();

            // Create timer for every second, enabled by default, hook elapsed event.
            _tickTimer = new Timer(1000);
            _tickTimer.Elapsed += OnTickTimerFired;

            // Do not allow timer to automatically tick, this prevents it spawning multiple threads, enable the timer.
            _tickTimer.AutoReset = false;
            _tickTimer.Enabled = true;

            _server = new ServerPipe();
            _client = new ClientPipe();
        }

        public IServerPipe Server
        {
            get { return _server; }
        }

        public IClientPipe Client
        {
            get { return _client; }
        }

        public string TickPhase { get; private set; }

        public void RemoveMode(ModeType mode)
        {
            // Ensure the mode exists as active mode.
            if (ActiveMode != null &&
                ActiveMode.Mode != mode)
                return;

            // Ensure modes list contains active mode.
            if (!_modes.Contains(ActiveMode))
                return;

            // Remove the mode from list.
            _modes.Remove(ActiveMode);

            // Check if there are any modes after removal.
            if (ActiveMode != null)
                ModeChangedEvent?.Invoke(ActiveMode.Mode);
        }

        public void StartGame()
        {
            NewgameEvent?.Invoke();
        }

        public bool IsClosing { get; private set; }

        public IMode ActiveMode
        {
            get
            {
                if (_modes.Count <= 0)
                    return null;

                var lastMode = _modes[_modes.Count - 1];
                return lastMode;
            }
        }

        public string ActiveModeName
        {
            get
            {
                if (_modes.Count <= 0)
                    return "None";

                var lastMode = _modes[_modes.Count - 1];
                return lastMode.Mode.ToString();
            }
        }

        public ReadOnlyCollection<IMode> Modes
        {
            get { return new ReadOnlyCollection<IMode>(_modes); }
        }

        public Randomizer Random
        {
            get { return _random; }
        }

        public uint TotalTicks { get; private set; }

        public event NewGame NewgameEvent;
        public event EndGame EndgameEvent;
        public event TickSim TickEvent;
        public event ModeChanged ModeChangedEvent;

        public void AddMode(ModeType mode)
        {
            // Create new mode, check if it is in mode list.
            var changeMode = OnModeChanging(mode);
            if (_modes.Contains(changeMode))
                return;

            _modes.Add(changeMode);
            ModeChangedEvent?.Invoke(changeMode.Mode);
        }

        public void CloseSimulation()
        {
            // Allow any data structures to save themselves.
            Console.WriteLine("Closing...");
            IsClosing = true;
            OnDestroy();
        }

        public uint TotalClients
        {
            get { return (uint) _server.Clients.Count; }
        }

        protected abstract GameMode OnModeChanging(ModeType mode);

        private void OnTickTimerFired(object Sender, ElapsedEventArgs e)
        {
            Tick();

            // Allow the timer to tick again now that we have finished working.
            _tickTimer.Enabled = true;
        }

        private void Tick()
        {
            // Increase the tick count.
            TotalTicks++;

            if (TotalTicks == 1)
            {
                OnFirstTick();
            }

            TickPhase = TickVisualizer(TickPhase);

            // Fire tick event for any subscribers to see and overrides for inheriting classes.
            TickEvent?.Invoke(TotalTicks);
            OnTick();
        }

        protected virtual void OnFirstTick()
        {
            // Server processes game logic, client sends command for server to process. Together they ward off thread-locking.
            _server.Start();
            _client.Start();
        }

        /// <summary>
        ///     Used for showing player that simulation is ticking on main view.
        /// </summary>
        private static string TickVisualizer(string currentPhase)
        {
            switch (currentPhase)
            {
                case @"*":
                    return @"|";
                case @"|":
                    return @"/";
                case @"/":
                    return @"-";
                case @"-":
                    return @"\";
                case @"\":
                    return @"|";
                default:
                    return "*";
            }
        }

        public virtual void OnDestroy()
        {
            // Destroy named pipe inter-process communication system.
            _server.Stop();
            _client.Stop();

            _modes.Clear();
            EndgameEvent?.Invoke();
        }

        protected virtual void OnTick()
        {
            // Process top-most game mode logic.
            TickModes();
        }

        private void TickModes()
        {
            // Only tick if there are modes to tick.
            if (_modes.Count <= 0)
                return;

            // Only top-most game mode gets ticking action.
            var lastMode = _modes[_modes.Count - 1];
            lastMode?.TickMode();
        }
    }
}