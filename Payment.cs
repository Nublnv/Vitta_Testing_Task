using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vitta_Testing_Task
{
    public class Payment
    {
        public int moneyId { get; }
        public int orderId { get; }
        public decimal sum { get; }

        public Payment(int moneyId, int orderId, decimal sum)
        {
            this.moneyId = moneyId;
            this.orderId = orderId;
            this.sum = sum;
        }
    }
}
