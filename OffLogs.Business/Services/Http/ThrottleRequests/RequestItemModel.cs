using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffLogs.Business.Services.Http.ThrottleRequests
{
    internal class RequestItemModel
    {
        public long ItemId { get; set; }

        /// <summary>
        /// The period during which the counter is increased
        /// </summary>
        public TimeSpan CountingPeriod { get; protected set; }

        /// <summary>
        /// The time when requests counting was started
        /// </summary>
        public DateTime CountStartTime { get; protected set; }

        public int Counter { get; protected set; }

        public int MaxCounterValue { get; protected set; }

        public bool IsCounterTooBig 
        { 
            get => Counter > MaxCounterValue; 
        }

        public bool IsPeriodOver
        {
            get
            {
                lock (_lock)
                {
                    return DateTime.Now - CountStartTime > CountingPeriod;
                }
            }
        }

        private object _lock = new { };

        public RequestItemModel(long itemId, int maxCounter, TimeSpan countingPeriod)
        {
            ItemId = itemId;
            CountingPeriod = countingPeriod;
            MaxCounterValue = maxCounter;
            Counter = 0;
            CountStartTime = DateTime.Now;
        }

        public void ResetCounter()
        {
            lock(_lock)
            {
                CountStartTime = DateTime.Now;
                Counter = 0;
            }
        }

        public void Increase()
        {
            Counter++;
        }
    }
}
