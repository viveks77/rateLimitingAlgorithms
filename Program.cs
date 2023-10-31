using RateLimiterAlogrithms;

//RunTokenBucketAlgorithm();// Key 2, 3 requests allowed (tokens were refilled)
//RunLeakyBucketAlgorithm();
//RunFixedWindowAlgorithm();
RunSlidingWindowLogAlgorithm();

static void RunTokenBucketAlgorithm()
{
    Console.WriteLine("Token Bucket Algorithm");
    TokenBucketAlgorithm rateLimiter = new();

    // Test rate limiting for different keys
    TestRateLimit(rateLimiter, 1, 3); // Key 1, 3 requests allowed
    TestRateLimit(rateLimiter, 2, 3); // Key 2, 3 requests allowed
    TestRateLimit(rateLimiter, 1, 3); // Key 1, 3 requests allowed (tokens were refilled)

    // Wait for a while to allow token bucket to refill
    Thread.Sleep(11000); // Wait for 11 seconds (1 second more than refill time)

    TestRateLimit(rateLimiter, 1, 3); // Key 1, 3 requests allowed (tokens were refilled)
    TestRateLimit(rateLimiter, 2, 3);

}

static void RunLeakyBucketAlgorithm()
{
    Console.WriteLine("Leaky Bucket Algorithm");
    LeakyBucketAlgorithm rateLimiter = new();

    // Test rate limiting for different keys
    TestRateLimit(rateLimiter, 1, 4); // Key 1, 4 requests allowed
    TestRateLimit(rateLimiter, 2, 4); // Key 2, 4 requests allowed
    TestRateLimit(rateLimiter, 1, 4); // Key 1, 4 requests allowed (tokens were refilled)

    // Wait for a while to allow token bucket to refill
    Thread.Sleep(6000); // Wait for 6 seconds

    TestRateLimit(rateLimiter, 1, 4); // Key 1, 4 requests allowed (tokens were refilled)
    TestRateLimit(rateLimiter, 2, 4); // Key 2, 4 requests allowed (tokens were refilled)
}

static void RunFixedWindowAlgorithm()
{
    Console.WriteLine("Fixed window algorithm");
    FixedWindowAlgorithm rateLimiter = new();

    TestRateLimit(rateLimiter, 1, 4);
    TestRateLimit(rateLimiter, 2, 4);
    TestRateLimit(rateLimiter, 1, 6);
    TestRateLimit(rateLimiter, 2, 6);
    Thread.Sleep(10000);
    TestRateLimit(rateLimiter, 1, 6);
}


static void RunSlidingWindowLogAlgorithm()
{
    Console.WriteLine("Sliding Window Log Algorithm");
    SlidingWindowLogAlgorithm rateLimiter = new SlidingWindowLogAlgorithm();

    // Test rate limiting for different keys
    TestRateLimit(rateLimiter, 1, 10); // Key 1, 10 requests allowed
    TestRateLimit(rateLimiter, 2, 10); // Key 2, 10 requests allowed
    TestRateLimit(rateLimiter, 1, 10); // Key 1, 10 requests allowed (sliding window)

    // Wait for the time window to slide
    Thread.Sleep(11000); // Wait for 11 seconds (1 second more than the window size)

    TestRateLimit(rateLimiter, 1, 10); // Key 1, 10 requests allowed (new sliding window)
    TestRateLimit(rateLimiter, 2, 10);
}

static void TestRateLimit(IAlgorithm rateLimiter, int key, int requests)
{
    // Print out the key being tested
    Console.WriteLine($"Testing key {key}:");

    // Loop through each request
    for (int i = 1; i <= requests; i++)
    {
        // Call the rate limiting method with the given key and store the result
        bool allowed = rateLimiter.rateLimiting(key);

        // Print out the result of each request, either allowed or denied
        if (allowed)
        {
            Console.WriteLine($"Request {i}: Allowed");
        }
        else
        {
            Console.WriteLine($"Request {i}: Denied");
        }
    }

    // Print out an empty line for spacing
    Console.WriteLine();
}