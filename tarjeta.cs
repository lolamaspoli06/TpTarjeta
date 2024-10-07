using System;

namespace Tp2AAT
{
    public class Tarjeta
    {
        private decimal saldo;
        private readonly decimal limiteSaldo = 9900;
        private readonly decimal saldoNegativoPermitido = 480;
        private readonly decimal tarifaBasica = 940;

        public Tarjeta(decimal saldoInicial)
        {
            saldo = saldoInicial;
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

        public bool DescontarPasaje()
        {
            // Verifica si el saldo actual o el saldo más el límite negativo permite cubrir la tarifa
            if (saldo >= tarifaBasica || (saldo + saldoNegativoPermitido >= tarifaBasica))
            {
                saldo -= tarifaBasica; // Descuenta la tarifa
                return true; // El pasaje se descontó con éxito
            }
            return false; // No se puede descontar
        }
    }
}
