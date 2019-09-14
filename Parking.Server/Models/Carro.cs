using System;

namespace Parking.Server.Models
{
    public class Carro
    {
        public string placa { get; set; }
        public DateTime dtInicio { get; set; }
        public DateTime? dtFim { get; set; }
        public string setor { get; set; }
        public string dsTicket { get; set; }
    }
}