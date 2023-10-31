class SlidingWindowCounterAlgorithm
{
    class Window 
    {
        public int key { get; set; }

        public int limit { get; set; }

        public int size { get; set; }

        public Dictionary<int, int> requests { get; set; }
    }

}