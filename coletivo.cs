using System;
using System.Collections.Generic;
using BoletoNamespace;
using ManejoDeTiempos;
using static TarjetaNamespace.Tarjeta;
using TarjetaNamespace;



namespace ColectivoNamespace
{
    public class Colectivo
    {
        public string Linea { get; private set; }
        private readonly decimal tarifaBasica = 940;
        private Tiempo tiempo;
      

        public decimal TarifaBasica => tarifaBasica;
        public bool EsInterurbano { get; protected set; } = false;

        public Colectivo(string linea, Tiempo tiempo, bool esInterurbano = false)
        {
            this.Linea = linea;
            this.tiempo = tiempo;
            this.EsInterurbano = esInterurbano;
        }

        public Boleto PagarCon(Tarjeta tarjeta)
        {
            DateTime now = tiempo.Now();

            if (EsFranquicia(tarjeta) && !HorarioPermitido(now))
            {
                return null;
            }

            decimal totalAbonado = tarjeta.CalcularTarifa(this);  // Usamos 'this' para referirnos al colectivo actual
            string descripcionExtra = "";

            if (tarjeta.SaldoNegativo > 0)
            {
                descripcionExtra = $"Abona saldo {tarjeta.SaldoNegativo}";
            }

            if (tarjeta is MedioBoleto)
            {
                if (tarjeta.ViajesHoy >= 4)
                {
                    totalAbonado = tarifaBasica;
                }

                if ((tiempo.Now() - tarjeta.UltimoUso).TotalMinutes < 1)
                {
                    totalAbonado = tarifaBasica;
                    tarjeta.ViajesHoy -= 1;
                }
            }

            if (tarjeta is BoletoGratuito && tarjeta.ViajesHoy > 2)
            {
                totalAbonado = tarifaBasica;
            }

            if (tarjeta.DescontarPasaje(totalAbonado))
            {
                tarjeta.ActualizarUltimoUso();
                tarjeta.ViajesHoy++;
                return new Boleto(
                    tiempo.Now(),
                    tarjeta.GetType().Name,
                    this.Linea,  // Usamos 'this' para referirnos al colectivo actual
                    totalAbonado,
                    tarjeta.Saldo,
                    tarjeta.Id.ToString(),
                    descripcionExtra
                );
            }

            return null;
        }


        private bool EsFranquicia(Tarjeta tarjeta)
        {
            return tarjeta is MedioBoleto || tarjeta is BoletoGratuito; // Agrega otras franquicias si tienes más
        }

        private bool HorarioPermitido(DateTime fecha)
        {
            DayOfWeek dia = fecha.DayOfWeek;
            int hora = fecha.Hour;

            return dia >= DayOfWeek.Monday && dia <= DayOfWeek.Friday && hora >= 6 && hora < 22;
        }

    }

}




