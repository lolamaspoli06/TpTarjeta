using BoletoNamespace;
using ManejoDeTiempos;
using static TarjetaNamespace.Tarjeta;
using TarjetaNamespace;


namespace ColectivoNamespace
{
    public class Colectivo
    {
        public string Linea { get; private set; }
        protected const decimal TarifaBasica = 1200m;
        protected const decimal TarifaInterurbana = 2500m;
        private Tiempo tiempo;

        public bool EsInterurbano { get; protected set; } = false; // Indica si es un colectivo interurbano

        // Constructor para colectivos urbanos
        public Colectivo(string linea, Tiempo tiempo, bool esInterurbano = false)
        {
            this.Linea = linea;
            this.tiempo = tiempo;
            this.EsInterurbano = esInterurbano;
        }


        public Boleto PagarCon(Tarjeta tarjeta, Colectivo colectivo)
        {
            DateTime now = tiempo.Now();

            if (EsFranquicia(tarjeta) && !HorarioPermitido(now))
            {
                Console.WriteLine("No se puede usar esta franquicia fuera del horario permitido (Lunes a Viernes de 6 a 22).");
                return null;
            }

            decimal totalAbonado = tarjeta.CalcularTarifa(colectivo);
            string descripcionExtra = "";

            if (tarjeta.SaldoNegativo > 0)
            {
                descripcionExtra = $"Abona saldo negativo: {tarjeta.SaldoNegativo}";
            }

            if (tarjeta is Tarjeta.MedioBoleto medioBoleto)
            {
                if (tarjeta.ViajesHoy >= 4)
                {
                    Console.WriteLine("No se puede usar medio boleto más de 4 veces por día. Se cobra tarifa básica.");
                    totalAbonado = tarjeta.TarifaBasica;
                }

                // Comprobar tiempo desde el último uso
                if ((DateTime.Now - tarjeta.UltimoUso).TotalMinutes < 5) // Cambiado a 5 minutos
                {
                    Console.WriteLine("No se puede usar la tarjeta de medio boleto antes de 5 minutos. Se cobra tarifa básica.");
                    totalAbonado = tarjeta.TarifaBasica;
                    tarjeta.ViajesHoy--; // Reduce el viaje de hoy si se aplica la tarifa básica
                }
            }

            // Lógica para Boleto Gratuito
            if (tarjeta is Tarjeta.BoletoGratuito && tarjeta.ViajesHoy > 2)
            {
                totalAbonado = tarjeta.TarifaBasica; // Se cobra tarifa básica
                Console.WriteLine("No se puede usar boleto gratuito más de 2 veces por día. Se cobra tarifa básica.");
            }

            // Intentar descontar el pasaje
            if (tarjeta.DescontarPasaje(totalAbonado))
            {
                tarjeta.ActualizarUltimoUso(); // Actualizar la fecha del último uso
                tarjeta.ViajesHoy++; // Incrementar el conteo de viajes del día
                // Crear y retornar el boleto
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

        public class ColectivoInterurbano : Colectivo
        {
            public ColectivoInterurbano(string linea, Tiempo tiempo) : base(linea, tiempo)
            {
                EsInterurbano = true;
            }
        }
    }
}
