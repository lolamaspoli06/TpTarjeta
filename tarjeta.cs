using ColectivoNamespace;
using System;

namespace TarjetaNamespace
{
    public class Tarjeta
    {
        private decimal saldo;
        private decimal saldoPendiente;
        private const decimal limiteSaldo = 36000;
        protected decimal tarifaBasica = 1200;
        protected decimal tarifaInterurbana = 2500;
        private readonly decimal saldoNegativo = 480;

        public int Id { get; private set; }
        public DateTime UltimoUso { get; private set; }
        public int ViajesHoy { get; set; }
        public int ViajesEsteMes { get; private set; }

        public Tarjeta(decimal saldoInicial)
        {
            saldo = saldoInicial > limiteSaldo ? limiteSaldo : saldoInicial;
            ViajesHoy = 1;
        }

        public decimal SaldoPendiente => saldoPendiente;
        public decimal Saldo => saldo;

        public decimal SaldoNegativo => saldo < 0 ? saldo : 0; // Propiedad corregida

        public void ProcesarSaldoPendiente()
        {
            AcreditarSaldoPendiente();
        }

        public bool CargarSaldo(decimal monto)
        {
            if (monto == 2000 || monto == 3000 || monto == 4000 || monto == 5000 ||
                monto == 6000 || monto == 7000 || monto == 8000 || monto == 9000)
            {
                decimal espacioDisponible = limiteSaldo - saldo;

                if (monto <= espacioDisponible)
                {
                    saldo += monto;
                    saldoPendiente = 0;
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
                Console.WriteLine("Monto de carga no permitido.");
                return false;
            }
        }

        public decimal CalcularTarifa(Colectivo colectivo)
        {
            decimal tarifaAplicada = colectivo.EsInterurbano ? tarifaInterurbana : tarifaBasica;

            // Aplicación de descuentos para viajes frecuentes en tarjetas normales
            if (!(this is MedioBoleto) && !(this is BoletoGratuito))
            {
                if (ViajesEsteMes >= 30 && ViajesEsteMes < 78)
                {
                    tarifaAplicada *= 0.8m; // 20% de descuento
                }
                else if (ViajesEsteMes >= 78 && ViajesEsteMes <= 79)
                {
                    tarifaAplicada *= 0.75m; // 25% de descuento
                }
            }

            if (this is BoletoGratuito)
            {
                tarifaAplicada = 0; // Sin costo para BoletoGratuito
            }
            else if (this is MedioBoleto)
            {
                tarifaAplicada /= 2; // 50% de descuento para MedioBoleto
            }

            return tarifaAplicada;
        }

        public void ActualizarUltimoUso()
        {
            UltimoUso = DateTime.Now;

            if (UltimoUso.Day == 1)
            {
                ViajesEsteMes = 0; // Reiniciar viajes al comienzo de cada mes
            }
        }

        public virtual bool DescontarPasaje(decimal monto)
        {
            ActualizarUltimoUso();

            if (saldo >= monto)
            {
                saldo -= monto;
                AcreditarSaldoPendiente();
                ViajesEsteMes++;
                return true;
            }
            else if (saldo + saldoNegativo >= monto)
            {
                saldo -= monto;
                AcreditarSaldoPendiente();
                ViajesEsteMes++;
                return true;
            }
            return false; // No se pudo descontar el pasaje
        }

        private void AcreditarSaldoPendiente()
        {
            if (saldo < limiteSaldo && saldoPendiente > 0)
            {
                decimal espacioDisponible = limiteSaldo - saldo;
                decimal montoAcreditar = Math.Min(saldoPendiente, espacioDisponible);

                saldo += montoAcreditar;
                saldoPendiente -= montoAcreditar;
            }
        }

        public class MedioBoleto : Tarjeta
        {
            public MedioBoleto(decimal saldoInicial) : base(saldoInicial) { }

            public override bool DescontarPasaje(decimal monto)
            {
                decimal tarifaAplicada = monto == tarifaBasica ? tarifaBasica / 2 : monto;
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
                return false; // No se pudo descontar el pasaje
            }
        }

        public class BoletoGratuito : Tarjeta
        {
            public BoletoGratuito(decimal saldoInicial) : base(saldoInicial) { }

            public override bool DescontarPasaje(decimal monto)
            {
                // El BoletoGratuito no debería afectar el saldo, simplemente permite el viaje
                AcreditarSaldoPendiente();
                return true; // Permite el viaje
            }
        }
    }
}
