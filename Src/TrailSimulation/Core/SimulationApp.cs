﻿namespace TrailSimulation.Core
{
    /// <summary>
    ///     Base simulation application class object. This class should not be declared directly but inherited by actual
    ///     instance of game controller.
    /// </summary>
    public abstract class SimulationApp
    {
        /// <summary>
        ///     Determines if the dynamic menu system should show the command names or only numbers. If false then only numbers
        ///     will be shown.
        /// </summary>
        public const bool SHOW_COMMANDS = false;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:TrailGame.SimulationApp" /> class.
        /// </summary>
        protected SimulationApp()
        {
            // Default run-level specification.
            RunLevel = SimulationRunlevel.Initialize;

            // Ticker module allows us to convert system tick pulses in steady stream of seconds.
            Ticker = new TickerModule();
            Ticker.FirstSimulationTickEvent += Ticker_FirstSimulationTickEvent;
            Ticker.SimulationTickEvent += Ticker_SimulationTickEvent;
            Ticker.SystemTickEvent += Ticker_SystemTickEvent;

            // Create modules needed for managing simulation.
            Randomizer = new RandomizerModule();
            WindowManager = new WindowModule();
            TextRender = new RenderingModule();

            // Input manager needs event hook for knowing when buffer is sent.
            InputManager = new InputModule();
            InputManager.InputManagerSendCommandEvent += InputManager_InputManagerSendCommandEvent;
        }

        /// <summary>
        ///     Determines the current state of the overall simulation.
        /// </summary>
        public SimulationRunlevel RunLevel { get; private set; }

        /// <summary>
        ///     Keeps track of how many times the underlying system ticks, uses this data to create pulses of one second for the
        ///     simulation to sync itself to from any number of input ticks.
        /// </summary>
        internal TickerModule Ticker { get; private set; }

        /// <summary>
        ///     Used for rolling the virtual dice in the simulation to determine the outcome of various events.
        /// </summary>
        internal RandomizerModule Randomizer { get; private set; }

        /// <summary>
        ///     Keeps track of the currently attached game mode, which one is active, and getting text user interface data.
        /// </summary>
        internal WindowModule WindowManager { get; private set; }

        /// <summary>
        ///     Handles input from the users keyboard, holds an input buffer and will push it to the simulation when return key is
        ///     pressed.
        /// </summary>
        public InputModule InputManager { get; private set; }

        /// <summary>
        ///     Shows the current state of the simulation as text only interface (TUI). Uses default constants if the attached mode
        ///     or state does not override this functionality and it is ticked.
        /// </summary>
        public RenderingModule TextRender { get; private set; }

        /// <summary>
        ///     Fired when the input manager wants to send a command to the currently running game simulation.
        /// </summary>
        /// <param name="command">Command that wants to be passed into active game mode.</param>
        private void InputManager_InputManagerSendCommandEvent(string command)
        {
            WindowManager.ActiveMode?.SendCommand(command);
        }

        /// <summary>
        ///     Fired when the underlying system, engine, potato ticks the simulation.
        /// </summary>
        private void Ticker_SystemTickEvent()
        {
            // Sends commands if queue has any.
            InputManager?.Tick();

            // Back buffer for only sending text when changed.
            TextRender?.Tick();

            // Rolls virtual dice.
            Randomizer?.Tick();
        }

        /// <summary>
        ///     Fired when the simulation deems a second has gone by from the averaged stream of system ticks.
        /// </summary>
        /// <param name="simTicks">Total number of seconds that have passed by.</param>
        private void Ticker_SimulationTickEvent(ulong simTicks)
        {
            // Changes game mode and state when needed.
            WindowManager?.Tick();
        }

        /// <summary>
        ///     Fired on the first actual simulation tick, this would be a system tick but helps us initialize the plumping of the
        ///     simulation.
        /// </summary>
        private void Ticker_FirstSimulationTickEvent()
        {
            OnFirstTick();
        }

        /// <summary>
        ///     Fired when the ticker receives the first system tick event.
        /// </summary>
        protected abstract void OnFirstTick();

        /// <summary>
        ///     Fired when the simulation is closing and needs to clear out any data structures that it created so the program can
        ///     exit cleanly.
        /// </summary>
        public void Destroy()
        {
            // TODO: Replace with attribute and reflection based initialization for simulation modules.
            OnBeforeDestroy();

            // OnModuleDestroy window manager.
            WindowManager.OnModuleDestroy();
            WindowManager = null;

            // OnModuleDestroy input manager.
            InputManager.InputManagerSendCommandEvent -= InputManager_InputManagerSendCommandEvent;
            InputManager.OnModuleDestroy();
            InputManager = null;

            // OnModuleDestroy text renderer.
            TextRender.OnModuleDestroy();
            TextRender = null;

            // OnModuleDestroy the ticker.
            Ticker.FirstSimulationTickEvent -= Ticker_FirstSimulationTickEvent;
            Ticker.SimulationTickEvent -= Ticker_SimulationTickEvent;
            Ticker.SystemTickEvent -= Ticker_SystemTickEvent;
            Ticker.OnModuleDestroy();
            Ticker = null;

            // OnModuleDestroy the randomizer.
            Randomizer.OnModuleDestroy();
            Randomizer = null;
        }

        /// <summary>
        ///     Called when simulation is about to destroy itself, but right before it actually does it.
        /// </summary>
        protected abstract void OnBeforeDestroy();

        /// <summary>
        ///     Called by external forces from the simulation such as underlying operating system, game engine, potato, etc.
        /// </summary>
        public void Tick()
        {
            // Converts pulses from OS into stream of seconds.
            Ticker.Tick();
        }
    }
}