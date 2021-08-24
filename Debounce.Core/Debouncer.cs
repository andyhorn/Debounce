using System;
using System.Timers;

namespace Debounce.Core
{
    /// <summary>
    /// Debounces an <see cref="Action"/> to prevent over-calling.
    /// This wrapper will execute the action only after a specified interval since the
    /// last Debounce call.
    /// </summary>
    public class Debouncer
    {
        private readonly Action _action;
        private readonly Timer _timer;

        /// <summary>
        /// Initiailizes a new instance of the <see cref="Debouncer"/> class.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to be executed.</param>
        /// <param name="intervalMs">The interval required before execution can proceed.</param>
        public Debouncer(Action action, int intervalMs)
        {
            _action = action;
            _timer = new Timer(intervalMs);
            _timer.Elapsed += OnTimerElapsed;
        }

        /// <summary>
        /// Calls the supplied action after the interval has elapsed.
        /// </summary>
        public void Debounce()
        {
            _timer?.Stop();
            _timer?.Start();
        }

        private void OnTimerElapsed(object _, EventArgs __)
        {
            if (_timer == null)
            {
                return;
            }

            _timer?.Stop();
            _action.Invoke();
        }
    }
}
