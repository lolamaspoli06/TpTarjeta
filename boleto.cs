
using System;
using System.Collections.Generic;

namespace BoletoNamespace;

public class Boleto
{
    public DateTime Fecha { get; private set; }
    public string TipoTarjeta { get; private set; }
    public string LineaColectivo { get; private set; }
    public decimal TotalAbonado { get; private set; }
    public decimal SaldoRestante { get; private set; }
    public string IdTarjeta { get; private set; }
    public string DescripcionExtra { get; private set; }
  
    public Boleto(DateTime fecha, string tipoTarjeta, string lineaColectivo, decimal totalAbonado, decimal saldoRestante, string idTarjeta, string descripcionExtra)
    {
        Fecha = fecha;
        TipoTarjeta = tipoTarjeta;
        LineaColectivo = lineaColectivo;
        TotalAbonado = totalAbonado;
        SaldoRestante = saldoRestante;
        IdTarjeta = idTarjeta;
        DescripcionExtra = descripcionExtra;
    }
}
