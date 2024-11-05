using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaNamespace  
{
    public class BoletoGratuito : Tarjeta
    {
        public BoletoGratuito(decimal saldoInicial) : base(saldoInicial) { }

        public override bool DescontarPasaje(decimal monto)
        {
            decimal tarifaAplicada = monto == tarifaBasica ? tarifaBasica : 0;

            if (saldo >= tarifaAplicada)
            {
                saldo -= tarifaAplicada;
                AcreditarSaldoPendiente();
                return true;
            }
            else if (saldo + saldoNegativo >= tarifaAplicada)
            {
                saldo -= tarifaAplicada;
                AcreditarSaldoPendiente();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
