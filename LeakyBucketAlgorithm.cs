// Define a class for the leaky bucket algorithm
using RateLimiterAlogrithms;
using System.Collections.Concurrent;

class LeakyBucketAlgorithm: IAlgorithm
{

    // Declare a list of buckets
    ConcurrentDictionary<int,Bucket> buckets = new();

    // Define a method to check if a request is allowed or denied based on the key
    public bool rateLimiting(int key)
    {
        // Find the bucket with the given key in the list, or create a new one if not found
        Bucket bucket = buckets.GetOrAdd(key, key => new() { key = key});

        // Get the current time
        DateTime currentTime = DateTime.Now;

        // Calculate the elapsed time since the last leakage in seconds
        double elapsedSeconds = (currentTime - bucket.lastLeakage).TotalSeconds;

        // Calculate the amount of data leaked based on the outgoing rate
        int leakage = (int)(elapsedSeconds * bucket.outgoingRate);

        // Update the volume of the bucket by subtracting the leakage, and make sure it is not negative
        bucket.volume = Math.Max(0, bucket.volume - leakage);

        // Update the last leakage time to the current time
        bucket.lastLeakage = currentTime;

        // Check if the volume of the bucket is less than the size, meaning that there is space for more data
        if (bucket.volume < bucket.size)
        {
            // Increment the volume by one, representing one unit of data added to the bucket
            bucket.volume++;

            return true;
        }

        return false;
    }

    // Define a class for each bucket object
    class Bucket
    {

        // Declare a property for the key of the bucket, which identifies it uniquely in the list
        public int key { get; set; }

        // Declare a property for the size of the bucket, which represents its maximum capacity in units of data
        public int size { get; set; } = 3;

        // Declare a property for the volume of the bucket, which represents its current amount of data in units of data
        public int volume { get; set; } = 0;

        // Declare a property for the outgoing rate of the bucket, which represents how fast data leaks out of the bucket in units of data per second
        public int outgoingRate { get; set; } = 1;

        // Declare a property for the last leakage time of the bucket, which represents when was the last time data leaked out of the bucket 
        public DateTime lastLeakage { get; set; } = DateTime.Now;
    }
}
