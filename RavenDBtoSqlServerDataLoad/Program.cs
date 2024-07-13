// See https://aka.ms/new-console-template for more information
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using RavenDBtoSqlServerDataLoad;
using RavenDBtoSqlServerDataLoad.Mappers;
using RavenDBtoSqlServerDataLoad.SQLServerEntities;

namespace RavenDBtoSqlServerDataLoad
{ 
    public class Program
    { 
        private readonly IMapper mapper;
        private readonly RavenDBContext context;
        private static IDocumentStore store;

        public Program(IMapper _mapper, RavenDBContext _context)
        {
            mapper = _mapper;
            context = _context;
        } 

        public static async Task Main(string[] args)
        {
            store = RavenDBStore.InitializeStore();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var program = serviceProvider.GetService<Program>();
            await program.TransferDataAsync();
            Console.WriteLine("Data syn completed!!");
            Console.ReadKey();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RavenDBContext>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddTransient<Program>();
        }

        public async Task<List<RavenDBEntities.Customer>> GetCustomerFromRavenDbAsync()
        {
            using (var session = store.OpenAsyncSession())
            {
                return await session.Query<RavenDBEntities.Customer>().ToListAsync();
            }
        }

        public async Task InsertCustomerToSqlServerAsync(List<RavenDBEntities.Customer> customer)
        {
            var sqlServerCustomer = mapper.Map<List<Customer>>(customer);
            context.Customers.AddRange(sqlServerCustomer);
            await context.SaveChangesAsync();
        }

        public async Task TransferDataAsync()
        { 
            var customer = await GetCustomerFromRavenDbAsync();
            await InsertCustomerToSqlServerAsync(customer);
        }

    }
}

