using System;
using System.Collections.Generic;

namespace Tp2AAT
{
    public class Boleto
    {
        public decimal SaldoRestante { get; private set; }

        public Boleto(decimal saldoRestante)
        {
            this.SaldoRestante = saldoRestante;
        }
    }
    
}