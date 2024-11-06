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
        private Tiempo tiempo; // Nuevo campo
        public decimal TarifaBasica => tarifaBasica;
        // Constructor actualizado
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
                // Usamos tiempo.Now() en lugar de DateTime.Now
                if (tarjeta.ViajesHoy >= 4)
                {
                    totalAbonado = tarifaBasica;
                }
                if ((tiempo.Now() - tarjeta.UltimoUso).TotalMinutes < 1)
                {

                    totalAbonado = tarifaBasica;
                    tarjeta.ViajesHoy -= 1;
                }
            }
            // Restricción en boleto gratuito.
            if (tarjeta is BoletoGratuito && tarjeta.ViajesHoy > 2)
            {
                totalAbonado = tarifaBasica;
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