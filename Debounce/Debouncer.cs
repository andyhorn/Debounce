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
        private readonly Action<object> _action;
        private readonly object _parameter;
        private DispatcherTimer _timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="Debouncer"/> class.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to be executed.</param>
        /// <param name="parameter">An optional parameter for the action.</param>
        public Debouncer(Action<object> action, object parameter = null)
        {
            _action = action;
            _parameter = parameter;
        }

        /// <summary>
        /// Sets up the debounce interval for the supplied <see cref="Action"/>.
        /// </summary>
        /// <param name="intervalMs">The number of milliseconds that must elapse before the action is invoked.</param>
        /// <param name="priority">The priority at which the dispatcher is set to invoke the action.</param>
        /// <param name="dispatcher">The dispatcher which will invoke the action; will use the current dispatcher if none is supplied.</param>
        public void Debounce(int intervalMs, DispatcherPriority priority = DispatcherPriority.ApplicationIdle, Dispatcher dispatcher = null)
        {
            // Stop any existing timer - this is the core of the "debounce" functionality.
            _timer?.Stop();
            _timer = null;

            // Use the "current" dispatcher if none was provided.
            dispatcher ??= Dispatcher.CurrentDispatcher;

            // Create a new timer to invoke the action after the timer has expired.
            _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(intervalMs), priority, OnExecute, dispatcher);
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
            _action.Invoke(_parameter);
        }
    }
}
