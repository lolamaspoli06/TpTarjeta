using System;

namespace Tp2AAT
{

    public class MedioBoleto : Tarjeta
    {
        public MedioBoleto(decimal saldoInicial) : base(saldoInicial) { }

        public override bool DescontarPasaje()
        {
            decimal tarifaConDescuento = tarifaBasica / 2;  // Usa 'tarifaBasica' que está en la clase base
            if (saldo >= tarifaConDescuento || (saldo + saldoNegativoPermitido >= tarifaConDescuento)) // Usa 'saldoNegativoPermitido'
            {
                saldo -= tarifaConDescuento;
                return true;
            }
            return false;
        }
    }

 }