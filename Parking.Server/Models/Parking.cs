using System.Collections.Generic;

namespace Parking.Server.Models
{
    public class Parking
    {
        private List<Vaga> vagas1;

        public List<Vaga> vagas { get => vagas1; set => vagas1 = value; }

        public string endereco { get; set; }
        public int qtdeMaxVagas { get; set; }
        public int qtdeVagasOcupadas { get; set; }
    }
}