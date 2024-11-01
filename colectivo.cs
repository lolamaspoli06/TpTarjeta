using BoletoNamespace;
using ManejoDeTiempos;
using static TarjetaNamespace.Tarjeta;
using TarjetaNamespace;

namespace ColectivoNamespace
{
    public class Colectivo
    {
        public string Linea { get; private set; }
        protected const decimal TarifaUrbana = 1200m;
        protected const decimal TarifaInterurbana = 2500m;
        private Tiempo tiempo;

        public bool EsInterurbano { get; protected set; } = false; // Indica si es un colectivo interurbano

        // Constructor para colectivos urbanos
        public Colectivo(string linea, Tiempo tiempo)
        {
            Linea = linea;
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

            // Calcular la tarifa a pagar según el tipo de colectivo
            decimal tarifa = tarjeta.CalcularTarifa(this);
            string descripcionExtra = "";

            // Comprobar saldo negativo
            if (tarjeta.SaldoNegativo > 0)
            {
                descripcionExtra = $"Abona saldo negativo: {tarjeta.SaldoNegativo}";
            }

            // Intentar descontar el pasaje
            if (tarjeta.DescontarPasaje(tarifa))
            {
                tarjeta.ActualizarUltimoUso();
                tarjeta.ViajesHoy++;
                return new Boleto(
                    DateTime.Now,
                    tarjeta.GetType().Name,
                    this.Linea,
                    tarifa,
                    tarjeta.Saldo,
                    tarjeta.Id.ToString(),
                    descripcionExtra
                );
            }
            return null;
        }

        private bool EsFranquicia(Tarjeta tarjeta)
        {
            return tarjeta is MedioBoleto || tarjeta is BoletoGratuito;
        }

        private bool HorarioPermitido(DateTime fecha)
        {
            DayOfWeek dia = fecha.DayOfWeek;
            int hora = fecha.Hour;

            return dia >= DayOfWeek.Monday && dia <= DayOfWeek.Friday && hora >= 6 && hora < 22;
        }
    }

    public class ColectivoInterurbano : Colectivo
    {
        public ColectivoInterurbano(string linea, Tiempo tiempo) : base(linea, tiempo)
        {
            EsInterurbano = true;
        }
    }
}
