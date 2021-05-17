using System;
using System.Data.SqlClient;
using System.Diagnostics;
using DepthFirstSearchExample;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DepthFirstSearchExampleTests
{
    public class AppDbContextFactory
    {
        public static AppDbContext Create(Type testClass)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            var connectionString = config.GetSection("DefaultConnection").Value;
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            var testDbName = $"{connectionStringBuilder.InitialCatalog}_{testClass.Name}_DFSTest";
            connectionStringBuilder.InitialCatalog = testDbName;

            var contextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionStringBuilder.ConnectionString)
                .LogTo(msg => Debug.WriteLine(msg), LogLevel.Information);

            return new AppDbContext(contextOptionsBuilder.Options);
        }
    }
}
