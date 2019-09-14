namespace Parking.Server.Models
{
    public class Vaga
    {

        public Vaga(string sigla, int vagaTotal, int vagaDisponivel)
        {
            this.vagaTotal = vagaTotal;
            this.sigla = sigla;
            this.vagaDisponivel = vagaDisponivel;

        }
        public int vagaDisponivel { get; set; }
        public string sigla { get; set; }
        public int vagaTotal { get; set; }
    }
}