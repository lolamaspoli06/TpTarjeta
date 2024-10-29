using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using BoletoNamespace;
using TarjetaNamespace;
using static TarjetaNamespace.Tarjeta;
namespace ColectivoNamespace;

public class Colectivo
{
    public string Linea { get; private set; }
    private readonly decimal tarifaBasica = 940;
    public Colectivo(string linea)
    {
        this.Linea = linea;
    }
    
    public Boleto PagarCon(Tarjeta tarjeta)
    {
        decimal totalAbonado = tarjeta.CalcularTarifa(tarjeta);
        string descripcionExtra = "";

        if (tarjeta.SaldoNegativo > 0)
        {
            descripcionExtra = $"Abona saldo {tarjeta.SaldoNegativo}";
        }

           // Para el caso del medio boleto, se pueden realizar hasta cuatro viajes por día.
        if (tarjeta is MedioBoleto && tarjeta.ViajesHoy >= 4)
        {
            totalAbonado = tarifaBasica;
            Console.WriteLine("No se puede usar medio boleto mas de 4 veces por día. Paga con tarifa básica.");
        }
       
       
        if (tarjeta is MedioBoleto && ((DateTime.Now - tarjeta.UltimoUso).TotalMinutes < 1))
        {
            totalAbonado = tarifaBasica;
            Console.WriteLine("No se puede usar medio boleto 2 veces durante 5 minutos. Paga con tarifa básica."); 
        }


         if (tarjeta is BoletoGratuito && tarjeta.ViajesHoy > 2)
         {
             totalAbonado = tarifaBasica;         
             Console.WriteLine("No se puede usar boleto gratuito mas de 2 veces por día. Paga con tarifa básica.");     
        }

            if (tarjeta.DescontarPasaje(totalAbonado))
                {     
                    tarjeta.ActualizarUltimoUso();
                    tarjeta.ViajesHoy++;
                    return new Boleto(
                        DateTime.Now,
                        tarjeta.GetType().Name,
                        this.Linea,
                        totalAbonado,
                        tarjeta.Saldo,
                        tarjeta.Id.ToString(),
                        descripcionExtra
                    );
                }
       

        return null;
    }

}