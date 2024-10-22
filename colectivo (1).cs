using System;

namespace Tp2AAT
{
    public class Colectivo
    {
        public string Linea { get; private set; }
        public Colectivo(string linea)
        {
            this.Linea = linea;
        }

        // Método para pagar el boleto con una tarjeta
        public Boleto PagarCon(Tarjeta tarjeta)
        {
            decimal totalAbonado = tarjeta.CalcularTarifa(tarjeta); // Calcular tarifa normal
            string descripcionExtra = "";

            // Verifica si se debe abonar saldo negativo
            if (tarjeta.SaldoNegativo > 0)
            {
                descripcionExtra = $"Abona saldo {tarjeta.SaldoNegativo}";
            }

            // Comprobar si es Medio Boleto o Boleto Gratuito
            if (tarjeta is MedioBoleto /* || tarjeta is BoletoGratuito*/)
            {
                // Comprobar si han pasado menos de 5 minutos desde el último uso
                if ((DateTime.Now - tarjeta.UltimoUso).TotalMinutes < 5)
                {
                    Console.WriteLine("No se puede usar la tarjeta de medio boleto antes de 5 minutos.");

                    if (!tarjeta.DescontarPasaje(totalAbonado))
                    {
                        Console.WriteLine("Saldo insuficiente para descontar la tarifa básica.");
                        return null;
                    }
                    totalAbonado = tarjeta.TarifaBasica;
                }
            }

            if (tarjeta.DescontarPasaje(totalAbonado))
            {
                tarjeta.ActualizarUltimoUso();
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
    }
}
