using System;

namespace Tp2AAT
{
    public class Tarjeta
    {   
         protected decimal saldo;
         protected decimal limiteSaldo = 9900;
         protected decimal saldoNegativoPermitido = 480;
         protected const decimal tarifaBasica = 940; 
         public int Id { get; private set; } 
         public DateTime UltimoUso { get; private set; }

        public decimal TarifaBasica
        {
            get { return tarifaBasica; }
        }

        public Tarjeta(decimal saldoInicial, int? idArbitrario = null)
        {
            saldo = saldoInicial;
            Id = idArbitrario ?? new Random().Next(100000, 999999);
            this.UltimoUso = DateTime.MinValue;
        }

        public decimal Saldo
        {
            get { return saldo; }
        }

        public bool CargarSaldo(decimal monto)
        {
            decimal nuevoSaldo = saldo + monto;
            if (nuevoSaldo > limiteSaldo)
            {
                return false;
            }
            saldo = nuevoSaldo;
            return true;
        }

        public decimal SaldoNegativo
        {
            get
            {
                if (saldo < 0)
                {
                    return saldo;
                }
                return 0;
            }
        }

        public decimal CalcularTarifa(Tarjeta tarjeta)
        {
            decimal tarifaCalculada = tarifaBasica;

           /* if (tarjeta is BoletoGratuito)
            {
                tarifaCalculada = 0; // Tarifa gratuita
            }
            else*/ if (tarjeta is MedioBoleto)
            {
                tarifaCalculada /= 2; 
            }

            return tarifaCalculada;
        }

        public void ActualizarUltimoUso()
        {
            UltimoUso = DateTime.Now;
        }

        public virtual bool DescontarPasaje(decimal monto)
        {

            if (saldo >= monto)
            {
                saldo -= monto; 
                Console.WriteLine($"Se ha descontado {monto}. Saldo restante: {saldo}");
                return true; 
            }
            else if ((saldo + saldoNegativoPermitido) >= monto)
            {
                saldo -= monto; 
                Console.WriteLine($"Se ha descontado {monto}. Saldo actual: {saldo}, incluyendo saldo negativo.");
                return true; 
            }
            else
            {
                Console.WriteLine("Saldo insuficiente para descontar el monto.");
                return false; 
            }
        }

    }
}
