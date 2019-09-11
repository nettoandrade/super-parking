using System;

namespace Parking.Server.Models
{
    public class Car
    {
        public Car(Cor cor, string modelo, string placa, Vaga localizacao)
        {
            this.cor = cor;
            this.modelo = modelo;
            this.placa = placa;
            this.localizacao = localizacao;
            this.entrada = DateTime.Now;

        }
        public Cor cor { get; set; }

        public string modelo { get; set; }

        public string placa { get; set; }

        public Vaga localizacao { get; set; }

        public DateTime? entrada { get; set; }

        public DateTime? saida { get; set; }

    }
}