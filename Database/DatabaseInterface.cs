using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TTBWeb_Asp.net.Models;

namespace TTBWeb_Asp.net.Database
{
    interface DatabaseInterface
    {

        public List<Movement> GetFirstFiveMovements();
        public List<Movement> GetMovementsWithNameLike(Movement movement);

    }
}
