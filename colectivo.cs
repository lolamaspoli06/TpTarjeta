using System;
using System.Collections.Generic;
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

        if (tarjeta is MedioBoleto || tarjeta is BoletoGratuito)
        {
            if (tarjeta.ViajesHoy >= 4)
            {

                totalAbonado = tarifaBasica; 
            }

            if ((DateTime.Now - tarjeta.UltimoUso).TotalMinutes < 5)
            {
                Console.WriteLine("No se puede usar la tarjeta de medio boleto antes de 5 minutos.");
                return null; 
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
        }

        return null; 
    }

}