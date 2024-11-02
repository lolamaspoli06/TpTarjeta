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
            decimal totalAbonado = tarjeta.CalcularTarifa();
            string descripcionExtra = "";

            // Comprobar saldo negativo
            if (tarjeta.SaldoNegativo > 0)
            {
                descripcionExtra = $"Abona saldo negativo: {tarjeta.SaldoNegativo}";
            }

            // Lógica para Medio Boleto
            if (tarjeta is Tarjeta.MedioBoleto medioBoleto)
            {
                if (tarjeta.ViajesHoy >= 4) 
                {
                    Console.WriteLine("No se puede usar medio boleto más de 4 veces por día. Se cobra tarifa básica.");
                    totalAbonado = tarjeta.TarifaBasica; // Cambia a tarifa básica
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

            // Retornar null si no se pudo descontar
            return null;
        }
    }
}
