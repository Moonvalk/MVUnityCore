using System;
using System.Collections.Generic;
using Moonvalk.Accessory;
using Moonvalk.Systems;

namespace Moonvalk.Utility
{
    /// <summary>
    /// A configurable timer object with additional functionality for referencing tasks.
    /// </summary>
    public class MVTimer : IQueueUpdatable
    {
        #region Data Fields
        /// <summary>
        /// The duration in seconds this timer takes to complete.
        /// </summary>
        protected float _duration;

        /// <summary>
        /// The time remaining in seconds.
        /// </summary>
        protected float _timeRemaining;

        /// <summary>
        /// The current timer state.
        /// </summary>
        protected MVTimerState _currentState = MVTimerState.Idle;

        /// <summary>
        /// A map of all tasks that need to be completed by each available timer state.
        /// </summary>
        protected Dictionary<MVTimerState, InitValue<List<Action>>> _functions;
        #endregion

        #region Public Getters/Setters
        /// <summary>
        /// Returns true when this MVTimer is complete.
        /// </summary>
        /// <value>Returns a boolean value representing whether this Timer has completed.</value>
        public bool IsComplete
        {
            get
            {
                return this._currentState == MVTimerState.Complete;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor taking no additional properties.
        /// </summary>
        public MVTimer()
        {
            this.initialize();
        }

        /// <summary>
        /// Constructor that allows the user to set a duration.
        /// </summary>
        /// <param name="duration_">The duration in seconds that this timer will run for.</param>
        public MVTimer(float duration_)
        {
            this.initialize();
            this.Duration(duration_);
        }

        /// <summary>
        /// Constructor that allows the user to start the Timer immediately.
        /// </summary>
        /// <param name="duration_">The duration in seconds that this timer will run for.</param>
        /// <param name="start_">Set to true if this Timer should start immediately.</param>
        public MVTimer(float duration_, bool start_)
        {
            this.initialize();
            this.Duration(duration_);
            if (start_)
            {
                this.Start();
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the duration of this Timer.
        /// </summary>
        /// <param name="duration_">The duration in seconds that need to elapse while this Timer runs.</param>
        /// <returns>Returns this MVTimer object.</returns>
        public MVTimer Duration(float duration_)
        {
            this._duration = duration_;
            return this;
        }

        /// <summary>
        /// Adds Actions that will be run when this MVTimer is complete.
        /// </summary>
        /// <param name="tasksToAdd_">Array of Actions to add for this state.</param>
        /// <returns>Returns this MVTimer object.</returns>
        public MVTimer OnComplete(params Action[] tasksToAdd_)
        {
            this.addTasks(MVTimerState.Complete, tasksToAdd_);
            return this;
        }

        /// <summary>
        /// Adds Actions that will be run when this MVTimer is started.
        /// </summary>
        /// <param name="tasksToAdd_">Array of Actions to add for this state.</param>
        /// <returns>Returns this MVTimer object.</returns>
        public MVTimer OnStart(params Action[] tasksToAdd_)
        {
            this.addTasks(MVTimerState.Start, tasksToAdd_);
            return this;
        }

        /// <summary>
        /// Adds Actions that will be run when this MVTimer is updated.
        /// </summary>
        /// <param name="tasksToAdd_">Array of Actions to add for this state.</param>
        /// <returns>Returns this MVTimer object.</returns>
        public MVTimer OnUpdate(params Action[] tasksToAdd_)
        {
            this.addTasks(MVTimerState.Update, tasksToAdd_);
            return this;
        }

        /// <summary>
        /// Starts this Timer with the latest configured settings.
        /// </summary>
        /// <returns>This MVTimer object.</returns>
        public MVTimer Start()
        {
            this._timeRemaining = this._duration;
            if (this._timeRemaining <= 0f)
            {
                this._currentState = MVTimerState.Complete;
                return this;
            }
            this._currentState = MVTimerState.Start;
            this.handleTasks(_currentState);
            (Global.GetSystem<MVTimerSystem>() as MVTimerSystem).Add(this);
            return this;
        }

        /// <summary>
        /// Stops this Timer.
        /// </summary>
        public void Stop()
        {
            this._currentState = MVTimerState.Stopped;
        }

        /// <summary>
        /// Resumes this Timer from wherever last left off.
        /// </summary>
        public void Resume()
        {
            this._currentState = MVTimerState.Update;
        }

        /// <summary>
        /// Updates this Timer.
        /// </summary>
        /// <param name="deltaTime_">Duration of time taken since last and current game tick.</param>
        /// <returns>Returns true when actively running and false once complete.</returns>
        public bool Update(float deltaTime_)
        {
            if (this._currentState == MVTimerState.Complete)
            {
                return false;
            }
            if (this._currentState == MVTimerState.Stopped || this._currentState == MVTimerState.Idle)
            {
                return true;
            }

            this._currentState = MVTimerState.Update;
            this.handleTasks(this._currentState);
            bool complete = this.runTimer(deltaTime_);
            if (complete)
            {
                this._currentState = MVTimerState.Complete;
                this.handleTasks(this._currentState);
                return false;
            }
            return true;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initializes this object.
        /// </summary>
        protected void initialize()
        {
            this._functions = new Dictionary<MVTimerState, InitValue<List<Action>>>();
            foreach (MVTimerState state in Enum.GetValues(typeof(MVTimerState)))
            {
                this._functions.Add(state, new InitValue<List<Action>>(() => { return new List<Action>(); }));
            }
        }

        /// <summary>
        /// Adds an array of new Actions to a TweenState.
        /// </summary>
        /// <param name="state_">The TweenState to add tasks for.</param>
        /// <param name="tasksToAdd_">The tasks to add.</param>
        protected void addTasks(MVTimerState state_, params Action[] tasksToAdd_)
        {
            foreach (Action task in tasksToAdd_)
            {
                _functions[state_].Value.Add(task);
            }
        }

        /// <summary>
        /// Handles all tasks for the specified state.
        /// </summary>
        /// <param name="state_">The state to run tasks for.</param>
        protected void handleTasks(MVTimerState state_)
        {
            foreach (Action action in _functions[state_].Value)
            {
                action();
            }
        }

        /// <summary>
        /// Runs this MVTimer object.
        /// </summary>
        /// <param name="deltaTime_">Duration of time between last and current game tick.</param>
        /// <returns>Returns true when complete or false when actively running.</returns>
        protected bool runTimer(float deltaTime_)
        {
            this._timeRemaining -= deltaTime_;
            if (this._timeRemaining <= 0f)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
