using BoletoNamespace;
using ManejoDeTiempos;
using static TarjetaNamespace.Tarjeta;
using TarjetaNamespace;

namespace ColectivoNamespace
{
    public class Colectivo
    {
        public string Linea { get; private set; }
        private readonly decimal tarifaBasica = 1200;
        private Tiempo tiempo;

        public decimal TarifaBasica => tarifaBasica;


        public Colectivo(string linea, Tiempo tiempo)
        {
            this.Linea = linea;
            this.tiempo = tiempo;
        }

        public Boleto PagarCon(Tarjeta tarjeta)
        {
            DateTime now = tiempo.Now();

            // Verifica si la tarjeta es una franquicia y si está en horario permitido
            if (EsFranquicia(tarjeta) && !HorarioPermitido(now))
            {
                Console.WriteLine("No se puede usar esta franquicia fuera del horario permitido (Lunes a Viernes de 6 a 22).");
                return null;
            }

            // Calcular la tarifa a pagar
            decimal totalAbonado = tarjeta.CalcularTarifa();
            string descripcionExtra = "";

            // Comprobar saldo negativo
            if (tarjeta.SaldoNegativo > 0)
            {
                descripcionExtra = $"Abona saldo negativo: {tarjeta.SaldoNegativo}";
            }

            // Aplicar la lógica de restricciones de Medio Boleto y Boleto Gratuito como en tu código...

            // Intentar descontar el pasaje
            if (tarjeta.DescontarPasaje(totalAbonado))
            {
                tarjeta.ActualizarUltimoUso();
                tarjeta.ViajesHoy++;
                return new Boleto(
                    DateTime.Now,
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

        private bool EsFranquicia(Tarjeta tarjeta)
        {
            return tarjeta is MedioBoleto || tarjeta is BoletoGratuito; // Agrega otras franquicias si tienes más
        }

        private bool HorarioPermitido(DateTime fecha)
        {
            DayOfWeek dia = fecha.DayOfWeek;
            int hora = fecha.Hour;

            return dia >= DayOfWeek.Monday && dia <= DayOfWeek.Friday && hora >= 6 && hora < 22;
        }






    }
}
