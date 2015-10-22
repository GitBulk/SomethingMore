using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstKnock.Domain
{
    public class Database
    {
        public static List<SalesOrder> SalesOrders = new List<SalesOrder>()
        {
            new SalesOrder{ SalesOrderId = 1, CustomerName = "Ronaldo", PONumber = "123" },
            new SalesOrder{ SalesOrderId = 2, CustomerName = "Messi", PONumber = "456" },
            new SalesOrder{ SalesOrderId = 3, CustomerName = "Rooney", PONumber = "789" },
            new SalesOrder{ SalesOrderId = 4, CustomerName = "Beckham", PONumber = "012" },
        };
    }
}
