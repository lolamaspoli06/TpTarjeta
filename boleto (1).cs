using System;

namespace Tp2AAT
{
    public class Boleto
    {
        // Propiedades
        public DateTime Fecha { get; private set; }
        public string TipoTarjeta { get; private set; }
        public string LineaColectivo { get; private set; }
        public decimal TotalAbonado { get; private set; }
        public decimal SaldoRestante { get; private set; }
        public string IdTarjeta { get; private set; }
        public string DescripcionExtra { get; private set; }

        // Constructor
        public Boleto(DateTime fecha, string tipoTarjeta, string lineaColectivo, decimal totalAbonado, decimal saldoRestante, string idTarjeta, string descripcionExtra)
        {
            Fecha = fecha;
            TipoTarjeta = tipoTarjeta;
            LineaColectivo = lineaColectivo;
            TotalAbonado = totalAbonado;
            SaldoRestante = saldoRestante;
            IdTarjeta = idTarjeta;
            DescripcionExtra = descripcionExtra;
        }

        // Método para mostrar la información del boleto
        public void MostrarInformacion()
        {
            Console.WriteLine($"Fecha: {Fecha}");
            Console.WriteLine($"Tipo de Tarjeta: {TipoTarjeta}");
            Console.WriteLine($"Línea de Colectivo: {LineaColectivo}");
            Console.WriteLine($"Total Abonado: {TotalAbonado}");
            Console.WriteLine($"Saldo Restante: {SaldoRestante}");
            Console.WriteLine($"ID de Tarjeta: {IdTarjeta}");
        }
    }
}

