using System;
using Microsoft.AspNetCore.Mvc;
using Parking.Server.Models;
using Parking.Server.Controllers.DAO;

namespace Parking.Server.Controllers
{
    
    public class ParkingController: Controller
    {
        ParkingDao dao;

        [HttpGet("v1/obter-vagas")]
        public IActionResult ObterVagas() { 
            dao = new ParkingDao();
            return Ok(dao.GetVagas()); 

        }

        [HttpGet("v1/inserir-tabela-preco")]
        public IActionResult InsereTabelaPreco() { 
            
            return Ok( new Car(Cor.Amarelo,"Gol","XXX-3030",null)); 

        }

        [HttpGet("v1/obter-tabela-preco")]
        public IActionResult ObterTabelas() { 
            
            dao = new ParkingDao();

            return Ok(dao.GetTabelaPreco()); 

        }
    }
}