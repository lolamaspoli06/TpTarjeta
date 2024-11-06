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

        // Función generalizada para pagar con tarjeta normal
        public Boleto PagarConTarjetaNormal(Tarjeta tarjeta)
        {
            decimal totalAbonado = tarjeta.CalcularTarifa();
            string descripcionExtra = "";

            if (tarjeta.SaldoNegativo > 0)
            {
                descripcionExtra = $"Abona saldo negativo: {tarjeta.SaldoNegativo}";
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

        // Función específica para pagar con medio boleto
        public Boleto PagarConMedioBoleto(MedioBoleto medioBoleto)
        {
            DateTime now = tiempo.Now();

            if (!HorarioPermitido(now))
            {
                Console.WriteLine("No se puede usar medio boleto fuera del horario permitido (Lunes a Viernes de 6 a 22).");
                return null;
            }

            decimal totalAbonado = medioBoleto.CalcularTarifa();
            string descripcionExtra = "";

            if (medioBoleto.SaldoNegativo > 0)
            {
                descripcionExtra = $"Abona saldo negativo: {medioBoleto.SaldoNegativo}";
            }

            if (medioBoleto.ViajesHoy >= 4)
            {
                Console.WriteLine("No se puede usar medio boleto más de 4 veces por día. Se cobra tarifa básica.");
                totalAbonado = medioBoleto.tarifaBasica;
            }

            if ((DateTime.Now - medioBoleto.UltimoUso).TotalMinutes < 5)
            {
                Console.WriteLine("No se puede usar la tarjeta de medio boleto antes de 5 minutos. Se cobra tarifa básica.");
                totalAbonado = medioBoleto.tarifaBasica;
                medioBoleto.ViajesHoy--;
            }

            if (medioBoleto.DescontarPasaje(totalAbonado))
            {
                medioBoleto.ActualizarUltimoUso();
                medioBoleto.ViajesHoy++;
                return new Boleto(
                    DateTime.Now,
                    medioBoleto.GetType().Name,
                    this.Linea,
                    totalAbonado,
                    medioBoleto.Saldo,
                    medioBoleto.Id.ToString(),
                    descripcionExtra
                );
            }
            return null;
        }

        // Función específica para pagar con medio boleto jubilado
        public Boleto PagarConMedioBoletoJubilado(MedioBoleto medioBoletoJubilado)
        {
            return PagarConMedioBoleto(medioBoletoJubilado); // Lógica similar a la de MedioBoleto
        }

        // Función específica para pagar con boleto gratuito
        public Boleto PagarConBoletoGratuito(BoletoGratuito boletoGratuito)
        {
            DateTime now = tiempo.Now();

            if (!HorarioPermitido(now))
            {
                Console.WriteLine("No se puede usar boleto gratuito fuera del horario permitido (Lunes a Viernes de 6 a 22).");
                return null;
            }

            decimal totalAbonado = boletoGratuito.CalcularTarifa();
            string descripcionExtra = "";

            if (boletoGratuito.SaldoNegativo > 0)
            {
                descripcionExtra = $"Abona saldo negativo: {boletoGratuito.SaldoNegativo}";
            }

            if (boletoGratuito.ViajesHoy > 2)
            {
                Console.WriteLine("No se puede usar boleto gratuito más de 2 veces por día. Se cobra tarifa básica.");
                totalAbonado = boletoGratuito.tarifaBasica;
            }

            if (boletoGratuito.DescontarPasaje(totalAbonado))
            {
                boletoGratuito.ActualizarUltimoUso();
                boletoGratuito.ViajesHoy++;
                return new Boleto(
                    DateTime.Now,
                    boletoGratuito.GetType().Name,
                    this.Linea,
                    totalAbonado,
                    boletoGratuito.Saldo,
                    boletoGratuito.Id.ToString(),
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
