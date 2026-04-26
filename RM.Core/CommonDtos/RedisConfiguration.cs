using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class RedisConfiguration
    {
        public bool UseRedis { get; set; } = false;
        public double Expiration { get; set; } = 10;
        public string Server { get; set; }

        public DistributedCacheEntryOptions RedisOptions
        {
            get
            {
                return new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Expiration),
                    // SlidingExpiration = TimeSpan.FromMinutes(2) 
                };
            }
        }

    }
}
