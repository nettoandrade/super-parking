using Microsoft.AspNetCore.Mvc;
using Parking.Server.Models;

namespace Parking.Server.Controllers
{
    public class ParkingController: Controller
    {
        [HttpGet("v1/obter-vagas")]
        public IActionResult ObterVagas() { 
            
            return Ok( new Car(Cor.Amarelo,"Gol","XXX-3030",null)); 

        }
    }
}