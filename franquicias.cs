using System;

namespace Tp2AAT
{
    public class MedioBoleto : Tarjeta
    {
        public MedioBoleto(decimal saldoInicial) : base(saldoInicial) { }  
        public override bool DescontarPasaje(decimal monto)
        {
            decimal tarifaConDescuento = tarifaBasica / 2;  
            if (saldo >= tarifaConDescuento || (saldo + saldoNegativoPermitido >= tarifaConDescuento))
            {
                saldo -= tarifaConDescuento; // Resta la tarifa con descuento
                return true;
            }
            return false;
        }
    }
/*
    public class BoletoGratuito : Tarjeta
    {
        public BoletoGratuito(decimal saldoInicial) : base(saldoInicial) { }

        public override bool DescontarPasaje(decimal monto)
        {
            // No hay descuento, se permite el viaje sin costo
            return true;
        }
    }*/
}
