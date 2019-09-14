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
        public IActionResult ObterVagas(string sigla) {
            dao = new ParkingDao();
            return Ok(dao.GetVagas(sigla));

        }

        [HttpGet("v1/obter-tabela-preco")]
        public IActionResult ObterTabelas(string tabela) {

            dao = new ParkingDao();

            return Ok(dao.GetTabelaPreco(tabela));

        }

        [HttpGet("v1/obter-valor-ticket")]
        public IActionResult ObterValorAtualTicket(string ticket){
            dao = new ParkingDao();
            Ticket dadosTicket = dao.GetTicket(ticket);
            TabelaDePreco tabelaPreco = dao.GetTabelaPreco(dadosTicket.tabela);
            Carro carro = dao.GetRegistroCarro(ticket);
            TimeSpan tempo = DateTime.Now - carro.dtInicio;
            double valor = (tempo.TotalHours * tabelaPreco.vlAdicional) + dadosTicket.valor;
            return Ok(valor.ToString("0.00"));
        }

        [HttpPost("v1/pagar-ticket")]
        public IActionResult PagarTicket(string ticket, double valor){
            dao = new ParkingDao();
            return Ok(dao.SetPagarTicket(ticket, valor) ? "PAGO" : "NÃO PAGO" );
        }

        [HttpPost("v1/estacionar")]
        public IActionResult EstacionarCarro(string placa, string setor, string tabelaDePreco){
            dao = new ParkingDao();
            string ticket = Guid.NewGuid().ToString();
            double valor = dao.GetTabelaPreco(tabelaDePreco).vlMinimo;

            if(dao.SetTicket(ticket, valor, tabelaDePreco)){
                dao.SetEstacionar(placa,setor,ticket);
                return Ok(ticket);
            } else{
                return Unauthorized();
            }

        }

        [HttpPost("v1/obter-valor-periodo")]
        public IActionResult ObterValorPeriodo(DateTime dataInicio, DateTime dataFim){
            dao = new ParkingDao();

            return Ok(dao.GetValorPeriodo(dataInicio,dataFim));
        }
    }
}
