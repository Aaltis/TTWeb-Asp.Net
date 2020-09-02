using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TTBWeb_Asp.net.Database;
using TTBWeb_Asp.net.Models;

namespace TTBWeb_Asp.net.Controllers
{
    public class MovementController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly SqlLiteDatabaseImplementation database;
        public MovementController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            database = new SqlLiteDatabaseImplementation();
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("movements")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Movements(MovementModel movement)
        {
            var movements = new List<MovementModel>();
            if (string.IsNullOrEmpty(movement.Name))
            {
                movements = database.GetFirstFiveMovements();
            }
            else
            {
                movements = database.GetMovementsWithNameLike(movement);
            }
            if (movements.Count != 0)
            {
                return Json(movements);
            }
            return Json(movements);

        }

        [Route("movements/new")]
        [HttpPost]
        public ActionResult AddMovement(MovementModel movement)
        {
            database.AddMovement(movement);
            return Json(database.GetMovementsWithName(movement));

        }

    }
}
