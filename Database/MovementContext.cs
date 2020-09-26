using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TTBWeb_Asp.net.Models;

namespace TTBWeb_Asp.net.Database
{
    public class MovementContext : DbContext, DatabaseInterface
    {
        public DbSet<Movement> Movements { get; set; }

        public MovementContext(DbContextOptions options)
         : base(options)
        {
            Database.EnsureCreated();
        }


        //We need to do little roundabout to do seeding for database.
        public static void SeedMovements(IConfiguration configuration)
        {
            var _optionsBuilder = new DbContextOptionsBuilder<MovementContext>().UseNpgsql(configuration.GetConnectionString("PostgreSql"));
            MovementContext movementContext = new MovementContext(_optionsBuilder.Options);
            var exercisesString = File.ReadAllLines(configuration.GetSection("AppSettings").GetSection("MovementFile").Value);
            List<Movement> movements = new List<Movement>();
            for (int i = 0; i < exercisesString.Length; i++)
            {
                var splitMovementString = exercisesString[i].Split(",");

                movements.Add(new Movement(splitMovementString[0].Trim(), splitMovementString[1].Trim()));
            }
            movementContext.AddRange(movements);
            movementContext.SaveChanges();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        public List<Movement> GetFirstFiveMovements()
        {

            return Movements.FromSqlRaw("SELECT * FROM movements LIMIT 5").AsNoTracking().ToList();
        }

        public List<Movement> GetMovementsWithNameLike(Movement movement)
        {
            return new List<Movement>();
            //  return Movements.<List<Movement>>("SELECT * FROM movements LIMIT 5");
        }
    }
}