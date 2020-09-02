using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.IO;
using TTBWeb_Asp.net.Models;

namespace TTBWeb_Asp.net.Database
{
    class DatabaseContext : DbContext
    {
        public DatabaseContext() :
            base(new SQLiteConnection()
            {
                ConnectionString = new SQLiteConnectionStringBuilder() { DataSource = GetConnection(), ForeignKeys = true }.ConnectionString
            }, true)
        {
        }
   
        public static string GetConnection()
        {
            return $"{Path.Combine(Directory.GetCurrentDirectory(), new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetSection("AppSettings").GetSection("DatabaseFile").Value)}";
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<MovementModel> MovementModels { get; set; }
    }
}
