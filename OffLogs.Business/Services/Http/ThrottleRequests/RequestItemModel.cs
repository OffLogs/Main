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
        /// The time when requests counting was started
        /// </summary>
        public DateTime CountStartTime { get; protected set; }

        public int Counter { get; protected set; }

        public int MaxCounterValue { get; protected set; }

        public bool IsCounterTooBig 
        { 
            get => Counter > MaxCounterValue; 
        }

        private object _lock = new { };

        public RequestItemModel(int maxCounter)
        {
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
