using System;
using System.Collections.Generic;

namespace Tp2AAT
{
    public class Tarjeta
    {
        private decimal saldo;
        private readonly decimal limiteSaldo = 9900;
        private readonly decimal tarifaBasica = 940;
    
        public Tarjeta(decimal saldoInicial)
        {
            saldo = saldoInicial > limiteSaldo ? limiteSaldo : saldoInicial;
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
            if (saldo >= tarifaBasica)
            {
                saldo -= tarifaBasica;
                return true; 
            }
            return false;
        }
    }
}