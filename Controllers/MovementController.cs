using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TTBWeb_Asp.net.Database;
using TTBWeb_Asp.net.Models;

namespace TTBWeb_Asp.net.Controllers
{
    public class MovementController : Controller
    {
        private readonly MovementContext _movementContext;
        public MovementController(MovementContext movementContext, IOptions<AppSettings> appSettings)
        {
            _movementContext = movementContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("movements")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Movements(Movement movement)
        {
            var movements = new List<Movement>();
            if (string.IsNullOrEmpty(movement.Name))
            {
                movements = _movementContext.GetFirstFiveMovements();
                //movements = database.Movements.FromSqlRaw<List<Movement>>("SELECT * FROM movements LIMIT 5").ToList();
            }
            else
            {
                movements = _movementContext.GetMovementsWithNameLike(movement);
            }
            if (movements.Count != 0)
            {
                return Json(movements);
            }
            return Json(movements);

        }

        [Route("movements/new")]
        [HttpPost]
        public ActionResult AddMovement(Movement movement)
        {
            //  database.AddMovement(movement);
            // return Json(database.GetMovementsWithName(movement));
            return Json("0");

        }

    }
}
