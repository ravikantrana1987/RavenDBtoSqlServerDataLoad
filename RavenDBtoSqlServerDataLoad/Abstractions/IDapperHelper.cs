using RavenDBtoSqlServerDataLoad.SQLServerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RavenDBtoSqlServerDataLoad.Abstractions
{
    public interface IDapperHelper
    {
        Task<Customer> GetCustomer(int customerId);
        Task InsertCustomerList();
    }
}
