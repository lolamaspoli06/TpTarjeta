using System;
using System.Collections.Generic;

namespace BoletoNamespace;

public class Boleto
{
    public string Linea { get; private set; }
    public decimal SaldoRestante { get; private set; }
    public DateTime FechaHora { get; private set; }

    public Boleto(decimal saldoRestante)
    {
        this.SaldoRestante = saldoRestante;
        this.FechaHora = DateTime.Now;
    }

    public override string ToString()
    {
        return $"Boleto - Saldo Restante: {SaldoRestante:C}, Fecha y Hora: {FechaHora}";
    }
}