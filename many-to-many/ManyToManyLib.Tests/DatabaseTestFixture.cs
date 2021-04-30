using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ManyToManyLib.Tests
{
    public class DatabaseTestFixture : IDisposable
    {
        private static object dbLock = new();
        private static bool dbInitialized;

        public DatabaseTestFixture()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            this.Connection = new SqlConnection(config.GetSection("DefaultConnection").Value);
            this.SeedDb();
            this.Connection.Open();
        }
        
        public DbConnection Connection { get; set; }
        public SqlContext CreateContext()
        {
            return new(
                new DbContextOptionsBuilder<SqlContext>()
                    .LogTo(msg => Debug.WriteLine(msg), LogLevel.Information)
                    .UseSqlServer(this.Connection.ConnectionString)
                    .Options);
        }

        public void Dispose() => this.Connection.Dispose();

        private void SeedDb()
        {
            lock (dbLock)
            {
                if (dbInitialized)
                {
                    return;
                }

                using (SqlContext context = CreateContext())
                {
                    context.Database.SetCommandTimeout(TimeSpan.FromSeconds(5));
                    context.Database.EnsureCreated();
                }
            }
        }
    }
}
