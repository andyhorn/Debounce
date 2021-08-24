using System;
using System.Windows.Threading;

namespace Debounce
{
    /// <summary>
    /// Debounces an <see cref="Action"/>, preventing it from being called too frequently.
    /// The action will only execute once calls have stopped for a given duration.
    /// </summary>
    public class Debouncer
    {
        private readonly Action _action;
        private readonly int _interval;
        private DispatcherTimer _timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Debouncer"/> class.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to be executed.</param>
        public Debouncer(Action action, int intervalMs)
        {
            _action = action;
            _interval = intervalMs;
        }

        /// <summary>
        /// Gets or sets the dispatcher priority.
        /// </summary>
        public DispatcherPriority Priority { get; set; }
            = DispatcherPriority.ApplicationIdle;

        /// <summary>
        /// Gets or sets the dispatcher used to invoke the function.
        /// </summary>
        public Dispatcher Dispatcher { get; set; }

        /// <summary>
        /// Sets up the debounce interval for the supplied <see cref="Action"/>.
        /// </summary>
        public void Debounce()
        {
            // Stop any existing timer - this is the core of the "debounce" functionality.
            _timer?.Stop();
            _timer = null;

            // Use the "current" dispatcher if none was provided.
            Dispatcher ??= Dispatcher.CurrentDispatcher;

            // Create a new timer to invoke the action after the timer has expired.
            _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(_interval), Priority, OnExecute, Dispatcher);
            _timer.Start();
        }

        private void OnExecute(object _, EventArgs __)
        {
            if (_timer == null)
            {
                return;
            }

            // Stop the current timer and invoke the action.
            _timer?.Stop();
            _timer = null;
            _action.Invoke();
        }
    }
}
