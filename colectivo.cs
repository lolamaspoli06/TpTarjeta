using BoletoNamespace;
using ManejoDeTiempos;
using static TarjetaNamespace.Tarjeta;
using TarjetaNamespace;

namespace ColectivoNamespace
{
    public class Colectivo
    {
        public string Linea { get; private set; }
        private readonly decimal tarifaBasica = 940;
        private Tiempo tiempo; 

        public decimal TarifaBasica => tarifaBasica;

        public Colectivo(string linea, Tiempo tiempo)
        {
            this.Linea = linea;
            this.tiempo = tiempo;
        }

        public Boleto PagarCon(Tarjeta tarjeta)
        {
            decimal totalAbonado = tarjeta.CalcularTarifa(tarjeta);
            string descripcionExtra = "";

            if (tarjeta.SaldoNegativo > 0)
            {
                descripcionExtra = $"Abona saldo {tarjeta.SaldoNegativo}";
            }

            if (tarjeta is MedioBoleto)
            {
                if (tarjeta.ViajesHoy >= 4)
                {
                   \\No se puede usar medio boleto más de 4 veces por día. Se cobra tarifa básica
                    totalAbonado = tarifaBasica;
                }

                if ((tiempo.Now() - tarjeta.UltimoUso).TotalMinutes < 5)
                {
                    \\No se puede usar la tarjeta de medio boleto antes de 5 minutos. Se cobra tarifa básica
                    totalAbonado = tarifaBasica;
                    tarjeta.ViajesHoy-=1;
                }
            }
            if (tarjeta is BoletoGratuito && tarjeta.ViajesHoy > 2)
            {
                totalAbonado = tarifaBasica;
                \\No se puede usar boleto gratuito más de 2 veces por día. Se cobra tarifa basica
            }

            if (tarjeta.DescontarPasaje(totalAbonado))
            {
                tarjeta.ActualizarUltimoUso();
                tarjeta.ViajesHoy++;
                return new Boleto(
                    tiempo.Now(),
                    tarjeta.GetType().Name,
                    this.Linea,
                    totalAbonado,
                    tarjeta.Saldo,
                    tarjeta.Id.ToString(),
                    descripcionExtra
                );
            }

            return null;
        }
    }

}
