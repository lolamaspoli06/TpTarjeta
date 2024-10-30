using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TarjetaNamespace;

public class Tarjeta
{
    private decimal saldo;
    private readonly decimal limiteSaldo = 9900;
    protected readonly decimal tarifaBasica = 940;
    private readonly decimal saldoNegativo = 480;
    public int Id { get; private set; }
    public DateTime UltimoUso { get; private set; }
    public int ViajesHoy { get; set; }

    public Tarjeta(decimal saldoInicial)
    {
        saldo = saldoInicial > limiteSaldo ? limiteSaldo : saldoInicial;
        ViajesHoy = 1;
    }

    public decimal Saldo
    {
        get { return saldo; }
    }

    public bool CargarSaldo(decimal monto)
    {
        if (monto <= limiteSaldo && (monto == 2000 || monto == 3000 || monto == 4000 || monto == 5000 || monto == 6000 || monto == 7000 || monto == 8000 || monto == 9000))
        {
            if (saldo < 0)
            {
                Console.WriteLine($"Saldo negativo ${saldo} acreditado por ${monto}");
            }
            decimal nuevoSaldo = saldo + monto;
            saldo = nuevoSaldo;
            Console.WriteLine($"Saldo actual ${saldo}");
            return true;
        }
        else
        {
            return false;
        }
    }

    public decimal SaldoNegativo
    {
        get
        {
            if (saldo < 0)
            {
                return saldo;
            }
            return 0;
        }
    }

    public decimal CalcularTarifa(Tarjeta tarjeta)
    {
        decimal tarifaCalculada = tarifaBasica;

        if (tarjeta is BoletoGratuito)
        {
            tarifaCalculada = 0;
        }
        else if (tarjeta is MedioBoleto)
        {
            tarifaCalculada /= 2;
        }

        return tarifaCalculada;
    }

    public void ActualizarUltimoUso()
    {
        UltimoUso = DateTime.Now;
    }

    public virtual bool DescontarPasaje(decimal monto)
    {
        if (saldo >= monto)
        {
            saldo -= monto;
            return true;
        }
        else if (saldo + saldoNegativo >= monto)
        {
            saldo -= monto;
            return true;
        }
        else
        {
            return false;
        }
    }

    public class MedioBoleto : Tarjeta
    {
        public MedioBoleto(decimal saldoInicial) : base(saldoInicial) { }

        public override bool DescontarPasaje(decimal monto)
        {
            // Usa tarifa básica si es solicitada, o la tarifa con descuento.
            decimal tarifaAplicada = monto == tarifaBasica ? tarifaBasica : tarifaBasica / 2;

            if (saldo >= tarifaAplicada)
            {
                saldo -= tarifaAplicada;
                return true;
            }
            else if (saldo + saldoNegativo >= tarifaAplicada)
            {
                saldo -= tarifaAplicada;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class BoletoGratuito : Tarjeta
    {
        public BoletoGratuito(decimal saldoInicial) : base(saldoInicial) { }

        public override bool DescontarPasaje(decimal monto)
        {
            // Usa tarifa básica si es solicitada, o viaje gratuito (tarifa = 0).
            decimal tarifaAplicada = monto == tarifaBasica ? tarifaBasica : 0;

            if (saldo >= tarifaAplicada)
            {
                saldo -= tarifaAplicada;
                return true;
            }
            else if (saldo + saldoNegativo >= tarifaAplicada)
            {
                saldo -= tarifaAplicada;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
