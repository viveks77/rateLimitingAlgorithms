using RateLimiterAlogrithms;
using System.Collections.Concurrent;

class FixedWindowAlgorithm : IAlgorithm
{
    // A dictionary to store windows associated with different keys
    ConcurrentDictionary<int, Window> windows = new();

    // Method to check if a request is allowed within the fixed time window
    public bool rateLimiting(int key)
    {
        // Retrieve or add a window for the specified key
        Window window = windows.GetOrAdd(key, x => new Window { key = x });

        // Get the current time
        DateTime currentTime = DateTime.Now;

        // Check if the time elapsed since the window start is greater than or equal to the window duration
        if (currentTime.Subtract(window.windowStart).TotalSeconds >= window.windowDuration)
        {
            // Reset the request count and update the window start to the current time
            window.requests = 0;
            window.windowStart = currentTime;
        }

        // If the count of requests within the window is below the limit, allow the request
        if (window.requests < window.limit)
        {
            // Increment the request count and allow the request
            window.requests++;
            return true;
        }

        // If the count exceeds the limit, deny the request
        return false;
    }

    // Class representing a window associated with a key
    class Window
    {
        public int key { get; set; } // Identifier for the window (e.g., a user or client key)
        public int windowDuration { get; set; } = 10; // Duration of the fixed time window (in seconds)
        public DateTime windowStart { get; set; } = DateTime.Now; // Timestamp for the start of the window
        public int limit { get; set; } = 10; // Maximum number of requests allowed within the window
        public int requests { get; set; } = 0; // Current count of requests made within the window
    }
}