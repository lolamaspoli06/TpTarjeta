using System;

namespace Tp2AAT
{

    public class MedioBoleto : Tarjeta
    {
        public MedioBoleto(decimal saldoInicial) : base(saldoInicial) { }

        public override bool DescontarPasaje()
        {
            decimal tarifaConDescuento = tarifaBasica / 2;  
            if (saldo >= tarifaConDescuento || (saldo + saldoNegativoPermitido >= tarifaConDescuento))
            {
                saldo -= tarifaConDescuento;
                return true;
            }
            return false;
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