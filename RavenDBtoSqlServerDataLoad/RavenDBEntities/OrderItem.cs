using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenDBtoSqlServerDataLoad.RavenDBEntities
{
    public class OrderItem
    {
        public string OrderId { get; set; } 
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
