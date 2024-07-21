using Dapper;
using RavenDBtoSqlServerDataLoad.Abstractions;
using RavenDBtoSqlServerDataLoad.SQLServerEntities;

namespace RavenDBtoSqlServerDataLoad.DapperQuery
{
    public class DapperHelper : IDapperHelper
    {
        private readonly ISqlConnectionFactory _SqlConnectionFactory;

        public DapperHelper(ISqlConnectionFactory connectionFactory)
        {
            _SqlConnectionFactory = connectionFactory;
        }

        public async Task<Customer> GetCustomer(int customerId)
        {
            using var connection = _SqlConnectionFactory.CreateConnection();
            var customerResponse = await connection.QueryFirstOrDefaultAsync<Customer>(
                """
                    SELECT Id,FirstName,LastName, CustomerId
                    FROM Customers WHERE Id = @CustomerId
                """,
                new
                {
                    CustomerId = customerId
                });

            if (customerResponse is null)
            {
                throw new Exception("No Customer Found!!");
            }
            return customerResponse;
        }

        public async Task InsertCustomerList()
        {            
            using var connection = _SqlConnectionFactory.CreateConnection();
            foreach (var customer in GetSeedCustomerList())
            { 
                customer.Id = await connection.QuerySingleAsync<int>(
                    """
                        INSERT INTO [RavenDB].[dbo].[Customers] ( 
                        [FirstName]
                        ,[LastName]
                        ,[CustomerId])
                        OUTPUT INSERTED.Id
                        values (@FirstName,@LastName,@CustomerId);
                        SELECT  @@IDENTITY AS Id;
                    """,
                    new { 
                        customer.FirstName,
                        customer.LastName,
                        customer.CustomerId 
                    }); 
                 

                foreach (var order in customer.Orders)
                { 
                    order.CustomerId = customer.Id;  
                    int orderId = await connection.QuerySingleAsync<int>
                    (
                        """
                            INSERT INTO ORDERS (OrderDate, CustomerId, OrderId) 
                            OUTPUT INSERTED.ID 
                            VALUES (@OrderDate, @CustomerId, @OrderId);
                        """,
                        new { 
                            order.OrderDate,
                            order.CustomerId,
                            order.OrderId 
                        });
                    Console.WriteLine($"Inserted Order Id  - {orderId}");

                    foreach (var item in order.OrderItems)
                    {
                        item.OrderId = orderId;
                        await connection.ExecuteAsync(
                            """
                                INSERT INTO OrderItems (ProductId, Quantity, Price,  OrderId)   
                                VALUES (@ProductId, @Quantity, @Price,  @OrderId) 
                            """,
                            new
                            {
                                item.ProductId,
                                item.Quantity,
                                item.Price,
                                item.OrderId
                            });
                    } 
                }  
                Console.WriteLine("Customer record inserted completed"); 
            }
        }

        private List<Customer> GetSeedCustomerList()
        {
            return new List<Customer>
            {
                new Customer
                {
                    FirstName = "F1",
                    LastName = "L1",
                    CustomerId="Cust/F1",
                    Orders =new List<Order>
                    {
                        new Order
                        {
                            OrderDate = DateTime.Now,
                            OrderId = "O1",
                            OrderItems = new List<OrderItem>
                            {
                                new OrderItem
                                {
                                    Quantity = 1,
                                    ProductId="P1",
                                    Price=1
                                },
                                new OrderItem
                                {
                                    Quantity = 1,
                                    ProductId="P2",
                                    Price=2
                                }
                            }
                        },
                        new Order
                        {
                            OrderDate = DateTime.Now,
                            OrderId = "O2",
                            OrderItems = new List<OrderItem>
                            {
                                new OrderItem
                                {
                                    Quantity = 1,
                                    ProductId="P3",
                                    Price=3
                                },
                                new OrderItem
                                {
                                    Quantity = 1,
                                    ProductId="P4",
                                    Price=4
                                }
                            }
                        }
                    }
                },
                new Customer
                {
                    FirstName = "F2",
                    LastName = "L2",
                    CustomerId="Cust/F2",
                    Orders =new List<Order>
                    {
                        new Order
                        {
                            OrderDate = DateTime.Now,
                            OrderId = "O3",
                            OrderItems = new List<OrderItem>
                            {
                                new OrderItem
                                {
                                    Quantity = 1,
                                    ProductId="P3",
                                    Price=3
                                },
                                new OrderItem
                                {
                                    Quantity = 1,
                                    ProductId="P4",
                                    Price=4
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
