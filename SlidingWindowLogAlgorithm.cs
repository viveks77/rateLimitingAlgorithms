using RateLimiterAlogrithms;
using System.Collections.Concurrent;

class SlidingWindowLogAlgorithm : IAlgorithm
{
    // A dictionary to store windows associated with different keys
    ConcurrentDictionary<int, Window> windows = new();

    // Method to check if a request is allowed within the sliding window
    public bool rateLimiting(int key)
    {
        // Retrieve or add a window for the specified key
        Window window = windows.GetOrAdd(key, x => new Window() { key = x });

        // Get the current time
        DateTime currentTime = DateTime.Now;

        // Check if there are timestamps in the queue and if the oldest timestamp is older than the window size
        if (window.requestTimes.Count > 0 && (currentTime - window.requestTimes.Peek()).TotalSeconds > window.size)
        {
            // Remove the oldest timestamp from the queue (sliding the window forward)
            window.requestTimes.Dequeue();
        }

        // If the count of timestamps in the queue is below the window limit, allow the request
        if (window.requestTimes.Count < window.limit)
        {
            // Add the current timestamp to the queue
            window.requestTimes.Enqueue(currentTime);
            return true;
        }

        // If the count exceeds the limit, deny the request
        return false;
    }

    // Class representing a window associated with a key
    class Window
    {
        public int key { get; set; }   // Identifier for the window (e.g., a user or client key)
        public int limit { get; set; } = 10; // Maximum number of requests allowed within the window
        public int size { get; set; } = 10; // Duration of the sliding time window (in seconds)
        public Queue<DateTime> requestTimes { get; set; } = new Queue<DateTime>(); // Queue to store request timestamps
    }
}