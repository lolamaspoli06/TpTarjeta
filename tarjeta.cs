using System;

namespace TarjetaNamespace
{
    public class Tarjeta
    {
        private decimal saldo;
        private decimal saldoPendiente;
        private const decimal limiteSaldo = 36000;
        protected decimal tarifaBasica = 1200;
        private readonly decimal saldoNegativo = 480;
        public int Id { get; private set; }
        public DateTime UltimoUso { get; private set; }
        public int ViajesHoy { get; set; }
        public int ViajesEsteMes { get; private set; } // Añadido para contar los viajes del mes

        public Tarjeta(decimal saldoInicial)
        {
            saldo = saldoInicial > limiteSaldo ? limiteSaldo : saldoInicial;
            ViajesHoy = 1;
        }

        public decimal SaldoPendiente
        {
            get { return saldoPendiente; }
        }
        public decimal TarifaBasica => tarifaBasica; // Agrega esto en tu clase Tarjeta


        public decimal Saldo
        {
            get { return saldo; }
        }

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

        public decimal CalcularTarifa()
        {
            decimal tarifaCalculada = tarifaBasica;

            // Aplicación de descuentos para viajes frecuentes en tarjetas normales
            if (!(this is MedioBoleto) && !(this is BoletoGratuito))
            {
                if (ViajesEsteMes >= 30 && ViajesEsteMes < 78)
                {
                    Console.WriteLine($" ${ViajesEsteMes}");
                    tarifaCalculada *= 0.8m; // 20% de descuento
                }
                else if (ViajesEsteMes >= 78 && ViajesEsteMes <= 79)
                {
                    tarifaCalculada *= 0.75m; // 25% de descuento
                }
            }

            if (this is BoletoGratuito)
            {
                tarifaCalculada = 0; // Sin costo para Boleto Gratuito
            }
            else if (this is MedioBoleto)
            {
                tarifaCalculada /= 2; // La mitad de la tarifa normal para Medio Boleto
            }

            return tarifaCalculada;
        }


        public void ActualizarUltimoUso()
        {
            UltimoUso = DateTime.Now;

            // Reinicio de viajes mensuales si es el primer día del mes
            if (UltimoUso.Day == 1)
            {
                ViajesEsteMes = 0; // Reinicia el contador de viajes
            }

            // El incremento de ViajesEsteMes se maneja ahora en DescontarPasaje
        }

        public virtual bool DescontarPasaje(decimal monto)
        {
            // Actualizar el último uso antes de descontar
            ActualizarUltimoUso(); // Asegúrate de que esto se llame para actualizar ViajesEsteMes

            if (saldo >= monto)
            {
                saldo -= monto;
                AcreditarSaldoPendiente();
                Console.WriteLine($"Descuento exitoso. Saldo actual: ${saldo}");

                // Incrementa el contador de viajes después de un descuento exitoso
                ViajesEsteMes++; // Solo incrementa aquí
                return true;
            }
            else if (saldo + saldoNegativo >= monto)
            {
                saldo -= monto;
                AcreditarSaldoPendiente();
                Console.WriteLine($"Descuento exitoso con saldo negativo. Saldo actual: ${saldo}");

                // Incrementa el contador de viajes después de un descuento exitoso
                ViajesEsteMes++; // Solo incrementa aquí
                return true;
            }
            else
            {
                Console.WriteLine("No se puede descontar el pasaje.");
                return false;
            }
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
                    Console.WriteLine($"Descuento exitoso para Medio Boleto. Saldo actual: ${saldo}");
                    return true;
                }
                else if (saldo + saldoNegativo >= tarifaAplicada)
                {
                    saldo -= tarifaAplicada;
                    AcreditarSaldoPendiente();
                    Console.WriteLine($"Descuento exitoso con saldo negativo para Medio Boleto. Saldo actual: ${saldo}");
                    return true;
                }
                else
                {
                    Console.WriteLine("No se puede descontar el pasaje para Medio Boleto.");
                    return false;
                }
            }
        }

        public class BoletoGratuito : Tarjeta
        {
            public BoletoGratuito(decimal saldoInicial) : base(saldoInicial) { }

            public override bool DescontarPasaje(decimal monto)
            {
                decimal tarifaAplicada = monto == tarifaBasica ? tarifaBasica : 0;

                if (saldo >= tarifaAplicada)
                {
                    saldo -= tarifaAplicada;
                    AcreditarSaldoPendiente();
                    Console.WriteLine($"Descuento exitoso para Boleto Gratuito. Saldo actual: ${saldo}");
                    return true;
                }
                else if (saldo + saldoNegativo >= tarifaAplicada)
                {
                    saldo -= tarifaAplicada;
                    AcreditarSaldoPendiente();
                    Console.WriteLine($"Descuento exitoso con saldo negativo para Boleto Gratuito. Saldo actual: ${saldo}");
                    return true;
                }
                else
                {
                    Console.WriteLine("No se puede descontar el pasaje para Boleto Gratuito.");
                    return false;
                }
            }
        }
    }
}
    

