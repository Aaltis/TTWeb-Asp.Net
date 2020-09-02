using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TTBWeb_Asp.net.Models;

namespace TTBWeb_Asp.net.Database
{
    interface DatabaseInterface
    {
        public void InitDatabase(IConfiguration configuration);

        public List<MovementModel> GetFirstFiveMovements();
        public List<MovementModel> GetMovementsWithNameLike(MovementModel movement);

    }
}
