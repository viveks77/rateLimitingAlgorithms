using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateLimiterAlogrithms
{
    public interface IAlgorithm
    {
        bool rateLimiting(int key);
    }
}
