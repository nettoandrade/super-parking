namespace Parking.Server.Models
{
    public class Vaga
    {
        public Vaga(string cor, int numero, Sigla sigla, bool ocupado)
        {
            this.cor = cor;
            this.numero = numero;
            this.sigla = sigla;
            this.ocupado = ocupado;

        }
        public string cor { get; set; }
        public int numero { get; set; }
        public Sigla sigla { get; set; }
        public bool ocupado { get; set; }
    }
}