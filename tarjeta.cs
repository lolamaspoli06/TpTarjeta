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

        public virtual bool DescontarPasaje()
        {
            if (saldo >= tarifaBasica - 480)
            {
                saldo -= tarifaBasica;
                return true;
            }
            return false;
        }

    public class MedioBoleto : Tarjeta
    {
        public MedioBoleto(decimal saldoInicial) : base(saldoInicial) { }

        public override bool DescontarPasaje()
        {
            decimal tarifaConDescuento = tarifaBasica / 2;
                saldo -= tarifaConDescuento;
                return true;
        }
    }

    public class BoletoGratuito : Tarjeta
    {
        public BoletoGratuito(decimal saldoInicial) : base(saldoInicial) { }

        public override bool DescontarPasaje()
        {
            return true;
        }
    }
}
