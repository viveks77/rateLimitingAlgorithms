// Define a class for the token bucket algorithm
using RateLimiterAlogrithms;
using System.Collections.Concurrent;

public class TokenBucketAlgorithm: IAlgorithm
{
    // Declare a concurrent dictionary of buckets
    ConcurrentDictionary<int, Bucket> buckets = new();

    // Define a method to create a new bucket with the given key
    private Bucket createBucket(int key) => new() { key = key };

    // Define a method to check if a request is allowed or denied based on the key
    public bool rateLimiting(int key)
    {
        // Try to get the existing bucket with the given key from the dictionary, or create a new one if not found
        Bucket bucket = buckets.GetOrAdd(key, x => createBucket(x));

        // Call the method to refill the tokens in the bucket based on the elapsed time and the refill rate
        refillToken(bucket);

        // Check if the bucket has more than zero tokens
        if (bucket.tokens > 0)
        {
            // Decrement the tokens by one, representing one unit of data sent
            bucket.tokens--;

            return true;
        }

        return false;
    }

    // Define a method to refill the tokens in the bucket based on the current time, the refill time, and the refill amount
    private void refillToken(Bucket bucket)
    {
        // Get the current time
        DateTime currentTime = DateTime.Now;

        // Calculate the elapsed time since the last refill in seconds
        double intervalInSeconds = (currentTime - bucket.LastRefillTime).TotalSeconds;

        // Calculate the number of tokens to add based on the elapsed time and the refill time
        int tokensToAdd = (int)(intervalInSeconds / bucket.refillTime);

        // Check if there are any tokens to add
        if (tokensToAdd > 0)
        {
            // Update the number of tokens in the bucket by adding the tokens to add, and make sure it does not exceed the bucket size
            bucket.tokens = Math.Min(bucket.bucketSize, tokensToAdd + bucket.tokens);

            // Update the last refill time to the current time
            bucket.LastRefillTime = currentTime;
        }

    }

    // Define a class for each bucket object
    class Bucket
    {
        // Declare a property for the key of the bucket, which identifies it uniquely in the dictionary
        public int key { get; set; }

        // Declare a property for the number of tokens in the bucket, which represents its current amount of data in units of data
        public int tokens { get; set; } = 3;

        // Declare a property for the size of the bucket, which represents its maximum capacity in units of data
        public int bucketSize { get; set; } = 3;

        // Declare a property for the refill time of the bucket, which represents how often new tokens are added to the bucket in seconds
        public int refillTime { get; set; } = 10;

        // Declare a property for the refill amount of the bucket, which represents how many tokens are added to the bucket each time in units of data
        public int refillAmount { get; set; } = 2;

        // Declare a property for the last refill time of the bucket, which represents when was the last time new tokens were added to the bucket 
        public DateTime LastRefillTime { get; set; } = DateTime.Now;

    }
}
