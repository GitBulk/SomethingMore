using FirstKnock.Domain;
using System;
using System.Collections.Generic;

namespace FirstKnock.DataLayer
{
    public interface ISalesOrderRepository
    {
        List<SalesOrderViewModel> GetAll();

        SalesOrderViewModel GetItem(int salesOrderId);

        void Delete(int salesOrderId);

        void Insert(SalesOrder salesOrder);

        void Update(SalesOrder salesOrder);
    }
}
