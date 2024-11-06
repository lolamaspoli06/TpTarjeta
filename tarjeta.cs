using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TarjetaNamespace
{
    public class Tarjeta
    {
        private decimal saldo;
        private decimal saldoPendiente;
        private const decimal limiteSaldo = 36000;
        private readonly decimal tarifaBasica = 940;
        private readonly decimal saldoNegativo = 480;
        public int Id { get; private set; }
        public DateTime UltimoUso { get; private set; }
        public int ViajesHoy { get; set; }
        public Tarjeta(decimal saldoInicial)
        {
            saldo = saldoInicial > limiteSaldo ? limiteSaldo : saldoInicial;
            ViajesHoy = 1;
        }
        public decimal SaldoPendiente
        {
            get { return saldoPendiente; }
        }
        public decimal Saldo
        {
            get { return saldo; }
        }
        public void ProcesarSaldoPendiente()
        {
            AcreditarSaldoPendiente();
        }
        public bool CargarSaldo(decimal monto)
        {            // Validar que el monto es permitido
            if (monto == 2000 || monto == 3000 || monto == 4000 || monto == 5000 ||
                monto == 6000 || monto == 7000 || monto == 8000 || monto == 9000)
            {
                decimal espacioDisponible = limiteSaldo - saldo;
                if (monto <= espacioDisponible)
                {
                    saldo += monto;
                    saldoPendiente = 0; // No hay saldo pendiente si la carga se realiza completamente.
                    Console.WriteLine($"Saldo cargado exitosamente. Saldo actual: ${saldo}");
                }
                else
                {
                    saldo += espacioDisponible;
                    saldoPendiente += (monto - espacioDisponible);
                    Console.WriteLine($"Carga parcial realizada. Saldo actual: ${saldo}. Saldo pendiente de acreditación: ${saldoPendiente}");
                }
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
        private void AcreditarSaldoPendiente()
        {
            if (saldo < limiteSaldo && saldoPendiente > 0)
            {
                decimal espacioDisponible = limiteSaldo - saldo;
                decimal montoAcreditar = Math.Min(saldoPendiente, espacioDisponible);
                saldo += montoAcreditar;
                saldoPendiente -= montoAcreditar;
                Console.WriteLine($"Se acreditaron ${montoAcreditar} del saldo pendiente. Saldo actual: ${saldo}. Saldo pendiente restante: ${saldoPendiente}");
            }
        }
        public virtual bool DescontarPasaje(decimal monto)
        {
            if (saldo >= monto)
            {
                saldo -= monto;
                AcreditarSaldoPendiente();
                return true;
            }
            else if (saldo + saldoNegativo >= monto)
            {
                saldo -= monto;
                AcreditarSaldoPendiente();
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
            {                // Usa tarifa básica si es solicitada, o la tarifa con descuento.
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
            {                // Usa tarifa básica si es solicitada, o viaje gratuito (tarifa = 0).
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
}