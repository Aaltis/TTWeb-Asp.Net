using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.Linq;
using System.IO;
using TTBWeb_Asp.net.Models;

using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System;

namespace TTBWeb_Asp.net.Database
{
    public class SqlLiteDatabaseImplementation : DbConfiguration, DatabaseInterface
    {

        public SqlLiteDatabaseImplementation()
        {
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);
            SetProviderServices("System.Data.SQLite", (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)));
        }

        public List<Movement> GetFirstFiveMovements()
        {
            SQLiteDatabaseContext context = new SQLiteDatabaseContext();
            var movements = context.MovementModels.SqlQuery("SELECT * FROM movements LIMIT 5");
            return movements.ToList();
       
        }

        public List<Movement> GetMovementsWithNameLike(Movement movement)
        {
            SQLiteDatabaseContext context = new SQLiteDatabaseContext();
            var movements = context.MovementModels.Where(m => m.Name.Contains(movement.Name)).ToList();
            return movements;
        }
        public List<Movement> GetMovementsWithName(Movement movement)
        {
            SQLiteDatabaseContext context = new SQLiteDatabaseContext();
            var movements = context.MovementModels.Where(m => m.Name==movement.Name).ToList();
            return movements;
        }
        internal void AddMovement(Movement movement)
        {
            SQLiteDatabaseContext context = new SQLiteDatabaseContext();
            context.MovementModels.Add(movement);
            context.SaveChanges();
        }

        public void InitDatabase(IConfiguration configuration)
        {
            if (CreateDatabaseIfNotExists(configuration)) return;

            SQLiteDatabaseContext context = new SQLiteDatabaseContext();

            var exercisesString = File.ReadAllLines(configuration.GetSection("AppSettings").GetSection("MovementFile").Value);
            List<Movement> movements = new List<Movement>();
            for (int i = 0; i < exercisesString.Length; i++)
            {
                var splitMovementString = exercisesString[i].Split(",");

                movements.Add(new Movement(splitMovementString[0].Trim(), splitMovementString[1].Trim()));
            }
            context.MovementModels.AddRange(movements);
            context.SaveChanges();

        }

      
        //Database cannot be created with context so we do it old way.
        private bool CreateDatabaseIfNotExists(IConfiguration configuration)
        {
            var databaseFilename = configuration.GetSection("AppSettings").GetSection("DatabaseFile").Value;
            if (!File.Exists(databaseFilename))
            {
                SQLiteConnection.CreateFile(databaseFilename);
            }
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), databaseFilename);
            string cs = $"URI=file:{fullPath}";

            using var con = new SQLiteConnection(cs);
            con.Open();

            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='movements'";
            SQLiteDataReader reader = cmd.ExecuteReader();

            string databaseStatus = "";
            while (reader.Read())
            {

                databaseStatus = reader[0].ToString();

            }
            reader.Close();
            reader.Dispose();
            if (databaseStatus == "1") return true;

            cmd.CommandText = "create table movements (id integer primary key,name varchar(100), type varchar(100))";
            cmd.ExecuteNonQuery();
            return false;
        }
    }
}
