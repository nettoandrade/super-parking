using System;

namespace Parking.Server.Models
{
    public class Ticket
    {
        
        public string ticket { get; set; }

        public double valor { get; set; }

        public bool pago { get; set; }
        public string tabela { get; set; }
        public DateTime? dataPagamento { get; set; }
    }
}