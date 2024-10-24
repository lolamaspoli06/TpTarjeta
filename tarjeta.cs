using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TarjetaNamespace;
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
        if (monto <= limiteSaldo && (monto == 2000 || monto == 3000 || monto == 4000 || monto == 5000 || monto == 6000 || monto == 7000 || monto == 8000 || monto == 9000)) { 
            decimal nuevoSaldo = saldo + monto;
            saldo = nuevoSaldo;
            return true;

        } else
        {
            return false;
        }
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