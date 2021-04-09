using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vitta_Testing_Task
{
    public class Order
    {
        
        public int id { get; }
        public DateTime date { get; }
        public decimal sum { get; }
        public decimal toPayment { get; }

        public Order(int id,DateTime date, decimal sum, decimal toPayment)
        {
            this.id = id;
            this.date = date;
            this.sum = sum;
            this.toPayment = toPayment;
        }
    }
}
