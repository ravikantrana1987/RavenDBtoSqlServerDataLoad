using Raven.Client.Documents;
using RavenDBtoSqlServerDataLoad.RavenDBEntities;

namespace RavenDBtoSqlServerDataLoad
{
    public class SeedRavenDB
    {
        private static IDocumentStore store;


        public static async Task SeedDataAsync()
        {
            store = RavenDBStore.InitializeStore();
            using (var session = store.OpenAsyncSession())
            {
                var customers = GetSeedData();

                foreach (var customer in customers)
                {
                    await session.StoreAsync(customer);
                }


                /*
                foreach (var customer in GetCustomers()) 
                {
                    await session.StoreAsync(customer);
                }

                foreach (var order in GetOrders())
                {
                    await session.StoreAsync(order);
                }

                foreach (var items in GetOrderItems())
                {
                    await session.StoreAsync(items);
                }

                foreach (var product in GetProducts())
                {
                    await session.StoreAsync(product);
                }
                */
                await session.SaveChangesAsync();
            }
        }

        private static List<Customer> GetSeedData()
        {
            // Define the seed data
            var customers = new List<Customer>
            {
                new Customer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Orders = new List<Order>
                    {
                        new Order
                        {
                            OrderId = "orders/1-A",
                            OrderDate = DateTime.Now.AddDays(-1),
                            OrderItems = new List<OrderItem>
                            {
                                new OrderItem
                                {
                                    ProductId = "products/1-A",
                                    Quantity = 2,
                                    Price = 20.0
                                },
                                new OrderItem
                                {
                                    ProductId = "products/2-A",
                                    Quantity = 1,
                                    Price = 15.0
                                }
                            }
                        },
                        new Order
                        {
                            OrderId = "orders/2-A",
                            OrderDate = DateTime.Now,
                            OrderItems = new List<OrderItem>
                            {
                                new OrderItem
                                {
                                    ProductId = "products/3-A",
                                    Quantity = 3,
                                    Price = 30.0
                                }
                            }
                        }
                    }
                },
                new Customer
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Orders = new List<Order>
                    {
                        new Order
                        {
                            OrderId = "orders/3-A",
                            OrderDate = DateTime.Now.AddDays(-2),
                            OrderItems = new List<OrderItem>
                            {
                                new OrderItem
                                {
                                    ProductId = "products/4-A",
                                    Quantity = 1,
                                    Price = 50.0
                                }
                            }
                        }
                    }
                }
            };

            return customers;

        }

        private static List<Customer> GetCustomers()
        {
            return new List<Customer>
            {
                new Customer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Orders = new List<Order>{ }
                },
                new Customer
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                }
            };
        }

        private static List<Order> GetOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    CustomerId = "customers/1-A",
                    OrderDate = DateTime.Now.AddDays(-1)
                },
                new Order
                {
                    CustomerId = "customers/2-A",
                    OrderDate = DateTime.Now.AddDays(-2)
                }
            };
        }

        private static List<OrderItem> GetOrderItems()
        {
            return new List<OrderItem>
            {
                 new OrderItem
                {
                    OrderId = "orders/1-A",
                    ProductId = "products/1-A",
                    Quantity = 2,
                    Price = 20.0
                },
                new OrderItem
                {
                    OrderId = "orders/1-A",
                    ProductId = "products/2-A",
                    Quantity = 1,
                    Price = 15.0
                },
                new OrderItem
                {
                    OrderId = "orders/2-A",
                    ProductId = "products/3-A",
                    Quantity = 3,
                    Price = 30.0
                }
            };
        }

        private static List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product
                {  
                    ProductName ="Product1"
                },
                new Product
                {  
                    ProductName ="Product2"
                },
                new Product
                {  
                     ProductName ="Product3"
                }
            };
        }


    }

     
         
}
