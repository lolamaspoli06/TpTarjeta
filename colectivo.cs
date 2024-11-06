using BoletoNamespace;
using ManejoDeTiempos;
using static TarjetaNamespace.Tarjeta;
using TarjetaNamespace;


namespace ColectivoNamespace
{
    // Colectivo.cs

    public class Colectivo
    {
        public string Linea { get; private set; }
        private readonly decimal tarifaBasica = 1200; // Tarifa para líneas urbanas
        private readonly decimal tarifaInterurbana = 2500; // Tarifa para líneas interurbanas
        private Tiempo tiempo; // Campo para el manejo de tiempo

        public decimal Tarifa { get; private set; }

        // Constructor actualizado para aceptar líneas interurbanas
        public Colectivo(string linea, Tiempo tiempo, bool esInterurbana = false)
        {
            this.Linea = linea;
            this.tiempo = tiempo;
            this.Tarifa = esInterurbana ? tarifaInterurbana : tarifaBasica;
        }

        public Boleto PagarCon(Tarjeta tarjeta)
        {
            decimal totalAbonado = tarjeta.CalcularTarifa(tarjeta);
            string descripcionExtra = "";

            // Verificar si es una franquicia y si estamos dentro del horario permitido
            if (EsFranquicia(tarjeta) && !EsHorarioPermitido())
            {
                throw new InvalidOperationException("Las franquicias solo pueden usarse de lunes a viernes de 6 a 22 horas.");
            }

            // Verificar descuentos y franquicias, aplicar la tarifa de la línea actual
            if (tarjeta is MedioBoleto)
            {
                if (tarjeta.ViajesHoy >= 4)
                {
                    totalAbonado = Tarifa;
                }
                if ((tiempo.Now() - tarjeta.UltimoUso).TotalMinutes < 1)
                {
                    totalAbonado = Tarifa;
                    tarjeta.ViajesHoy -= 1;
                }
            }

            if (tarjeta is BoletoGratuito && tarjeta.ViajesHoy > 2)
            {
                totalAbonado = Tarifa;
            }

            // Realizar el pago con el saldo de la tarjeta
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

        // Verifica si la tarjeta pertenece a una franquicia
        private bool EsFranquicia(Tarjeta tarjeta)
        {
            return tarjeta is MedioBoleto || tarjeta is BoletoGratuito;
        }

        // Verifica si el horario actual está dentro de la franja permitida
        private bool EsHorarioPermitido()
        {
            DateTime now = tiempo.Now();
            return now.DayOfWeek >= DayOfWeek.Monday && now.DayOfWeek <= DayOfWeek.Friday &&
                   now.Hour >= 6 && now.Hour < 22;
        }
    }

}
