using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaNamespace
{
    public class MedioBoleto : Tarjeta
    {
        public MedioBoleto(decimal saldoInicial) : base(saldoInicial) { }

        public override bool DescontarPasaje(decimal monto)
        {
            decimal tarifaAplicada = monto == tarifaBasica ? tarifaBasica : tarifaBasica / 2;

            if (saldo >= tarifaAplicada)
            {
                saldo -= tarifaAplicada;
                AcreditarSaldoPendiente();
                return true;
            }
            else if (saldo + saldoNegativo >= tarifaAplicada)
            {
                saldo -= tarifaAplicada;
                AcreditarSaldoPendiente(); ;
                return true;
            }
            else
            {

                return false;
            }
        }
    }
}
