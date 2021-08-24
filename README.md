# Debounce
Class libraries containing debouncers for .Net Core and WPF projects.

## Overview
The `Debouncer` class prevents an `Action` from executing "too many times" or "too frequently."

It does this by requiring that a specified interval is reached after the last function call before execution can proceed.
For example, with a debounce interval of 500ms, a function can be called rapidly but will only execute (once) after 500ms has passed since the _last_ function call.

## Definition

### Debounce.Core

This version of the debouncer is designed to run in any .Net Core project.

```
public class Debouncer
{
    public Debouncer(Action action, int intervalMs);
    public void Debounce();
}
```

### Debounce.WPF

This version of the debouncer is designed to run the supplied action on the UI thread in a WPF project.

```
public class Debouncer
{
    public Debouncer(Action action, int intervalMs);
    public DispatcherPriority Priority { get; set; }
    public Dispatcher Dispatcher { get; set; }
    public void Debounce();
}
```

## Usage

The example below is very simplified and can be run with either version of the debouncer.

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
            var debouncer = new Debouncer(() => {
                // This function will only execute after the interval has elapsed after the last call to .Debounce.
                _executed = true;
                Console.WriteLine("Function executed!");
            }, IntervalMs);
            
            while (!_executed)
            {
                var delay = random.Next(1000);
                
                Thread.Sleep(delay);
                
                // Call the Debounce method as often as you want, it will only execute the function
                // if there is a pause long enough between calls to reach the specified interval (in this case, 500ms).
                debouncer.Debounce();
            }
        }
    }
}
```

