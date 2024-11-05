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

        public bool EsInterurbano { get; protected set; } = false;


        public Colectivo(string linea, Tiempo tiempo, bool esInterurbano = false)
        {
            this.Linea = linea;
            this.tiempo = tiempo;
            this.EsInterurbano = esInterurbano;
        }

        public Boleto PagarCon(Tarjeta tarjeta)
        {
            DateTime now = tiempo.Now();

            if (EsFranquicia(tarjeta) && !HorarioPermitido(now))
            {
                Console.WriteLine("No se puede usar esta franquicia fuera del horario permitido (Lunes a Viernes de 6 a 22).");
                return null;
            }


            decimal totalAbonado = tarjeta.CalcularTarifa();
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
                    totalAbonado = tarjeta.tarifaBasica;
                }


                if ((DateTime.Now - tarjeta.UltimoUso).TotalMinutes < 5)
                {
                    Console.WriteLine("No se puede usar la tarjeta de medio boleto antes de 5 minutos. Se cobra tarifa básica.");
                    totalAbonado = tarjeta.tarifaBasica;
                    tarjeta.ViajesHoy--;
                }
            }


            if (tarjeta is Tarjeta.BoletoGratuito && tarjeta.ViajesHoy > 2)
            {
                totalAbonado = tarjeta.tarifaBasica;
                Console.WriteLine("No se puede usar boleto gratuito más de 2 veces por día. Se cobra tarifa básica.");
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

    public class InterUrbano : Colectivo
    {
        public InterUrbano(string linea, Tiempo tiempo, decimal tarifaBasica) : base(linea, tiempo,  tarifaBasica) { }

    }

}