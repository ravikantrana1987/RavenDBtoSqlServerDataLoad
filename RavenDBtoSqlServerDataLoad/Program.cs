// See https://aka.ms/new-console-template for more information
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;
using RavenDBtoSqlServerDataLoad;
using RavenDBtoSqlServerDataLoad.Abstractions;
using RavenDBtoSqlServerDataLoad.DapperQuery;
using RavenDBtoSqlServerDataLoad.Mappers;
using RavenDBtoSqlServerDataLoad.SQLServerEntities;
using System.IO;

namespace RavenDBtoSqlServerDataLoad
{ 
    public class Program
    { 
        private readonly IMapper mapper;
        private readonly RavenDBContext context;
        private readonly IDapperHelper dapperHelper;
        private static IDocumentStore store;

        public Program(IMapper _mapper, RavenDBContext _context, IDapperHelper _dapperHelper)
        {
            mapper = _mapper;
            context = _context;
            dapperHelper = _dapperHelper;
        } 

        public static async Task Main(string[] args)
        {
            store = RavenDBStore.InitializeStore();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider(); 
            var program = serviceProvider.GetService<Program>();

            Console.WriteLine("Choose below Option -");
            Console.WriteLine("A - Insert RavenDB data to SQL");
            Console.WriteLine("B - Insert C# object to SQL through Dapper");
            Console.WriteLine("Please Enter option : ");
            var option = Console.ReadLine();

            switch (option?.ToLower())
            {
                case "a":
                    await program.TransferDataAsync();
                    break;
                case "b":
                    await program.InsertCustomerList();
                    break;
                default:
                    Console.WriteLine("Invalid code");
                    break;
            }
            

            //await program.InsertCustomerList();

            ////await program.TransferDataAsync();
            Console.WriteLine("Data syn completed!!");
            Console.ReadKey();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath( Path.Combine( Directory.GetCurrentDirectory(), "Configurations"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddSingleton<ISqlConnectionFactory,SqlConnectionFactory>();
            services.AddDbContext<RavenDBContext>();
            services.AddSingleton<IDapperHelper,DapperHelper>();
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


        public async Task<Customer> GetCustomerFromSQL(int customerId)
        { 
            return await dapperHelper.GetCustomer(customerId);
        }

        public async Task InsertCustomerList()
        {
            await dapperHelper.InsertCustomerList();
        }
    }
}

