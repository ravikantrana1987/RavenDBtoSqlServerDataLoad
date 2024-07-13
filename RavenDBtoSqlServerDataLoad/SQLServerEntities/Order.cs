﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenDBtoSqlServerDataLoad.SQLServerEntities
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
