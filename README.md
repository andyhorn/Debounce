# Debounce
A WPF class library for a Debouncer utility.

## Overview
The `Debouncer` class prevents an `Action` from executing "too many times" or "too frequently."

It does this by requiring that a specified interval is reached after the last function call before execution can proceed.
For example, with a debounce interval of 500ms, a function can be called rapidly but will only execute (once) after 500ms has passed since the _last_ function call.

## Definition
```
public class Debouncer
{
    public Debouncer(Action<object> action, int intervalMs, object parameter = null);
    public DispatcherPriority Priority { get; set; }
    public Dispatcher Dispatcher { get; set; }
    public void Debounce();
}
```

## Usage
```
using System;
using System.Threading;

namespace DebounceDemo
{
    class Program
    {
        private const int IntervalMs = 500;
        private static bool _executed = false;
        
        static void Main(string[] args)
        {
            var random = new Random();
            
            // Initialize the Debouncer object with the function and interval
            var debouncer = new Debouncer(Function, IntervalMs);
            
            while (!_executed)
            {
                var delay = random.Next(1000);
                
                Thread.Sleep(delay);
                
                // Call the Debounce method as often as you want, it will only execute the function
                // if there is a pause long enough to reach the specified interval (in this case, 500ms).
                debouncer.Debounce();
            }
        }
        
        // This function will execute if 500ms elapses after a Debounce call. It will then set
        // the flag to true, which will cause the while loop in Main to exit.
        static void Function()
        {
            _executed = true;
            Console.WriteLine("Function executed!");
        }
    }
}
```

