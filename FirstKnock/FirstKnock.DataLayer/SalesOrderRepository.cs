using FirstKnock.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstKnock.DataLayer
{
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private static SalesOrder GetSalesOrder(int salesOrderId)
        {
            return Database.SalesOrders.SingleOrDefault(s => s.SalesOrderId == salesOrderId);
        }

        private static SalesOrderViewModel ConvertToViewSalesOrder(SalesOrder order)
        {
            var view = new SalesOrderViewModel()
            {
                SalesOrderId = order.SalesOrderId,
                CustomerName = order.CustomerName,
                PONumber = order.PONumber,
                Another = "From ViewModel"
            };
            return view;
        }

        public List<SalesOrderViewModel> GetAll()
        {
            var items = (from s in Database.SalesOrders
                         select ConvertToViewSalesOrder(s)).ToList();
            return items;
        }

        public void Delete(int salesOrderId)
        {
            SalesOrder order = GetSalesOrder(salesOrderId);
            if (order != null)
            {
                Database.SalesOrders.Remove(order);
            }
        }

        public void Insert(SalesOrder salesOrder)
        {
            if (salesOrder != null)
            {
                Database.SalesOrders.Add(salesOrder);
            }
        }

        public void Update(SalesOrder salesOrder)
        {
            if (salesOrder != null)
            {
                SalesOrder orderFromDB = GetSalesOrder(salesOrder.SalesOrderId);
                if (orderFromDB != null)
                {
                    orderFromDB.PONumber = salesOrder.PONumber;
                    orderFromDB.CustomerName = salesOrder.CustomerName;
                }
            }
        }

        public SalesOrderViewModel GetItem(int salesOrderId)
        {
            SalesOrder order = GetSalesOrder(salesOrderId);
            var view = ConvertToViewSalesOrder(order);
            return view;
        }
    }
}
