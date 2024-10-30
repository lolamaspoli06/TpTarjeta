using System;
using BoletoNamespace;
using ColectivoNamespace;
using TarjetaNamespace;
using ManejoDeTiempos;
using NUnit.Framework;

namespace Tests
{
    public class ColectivoTests
    {
        private Colectivo colectivo;
        private TiempoFalso tiempoFalso;
        private Tarjeta medioBoleto;

        [SetUp]
        public void Setup()
        {
            tiempoFalso = new TiempoFalso();  // Crear instancia de TiempoFalso
            colectivo = new Colectivo("Línea 120", tiempoFalso);  // Pasar tiempoFalso al Colectivo
            medioBoleto = new Tarjeta.MedioBoleto(4000); // Saldo inicial
        }

        [Test]
        public void NoDeberiaPermitirViajeEnMenosDe1MinutoConMedioBoleto()
        {
            // Simulamos el primer viaje
            var primerViaje = colectivo.PagarCon(medioBoleto);
            Assert.IsNotNull(primerViaje, "El primer viaje debería ser permitido.");

            // Intentamos realizar un segundo viaje inmediatamente (sin esperar 1 minuto)
            var segundoViaje = colectivo.PagarCon(medioBoleto);
            Assert.IsNotNull(segundoViaje, "El segundo viaje debería ser permitido, pero con tarifa básica.");
            Assert.That(segundoViaje.TotalAbonado, Is.EqualTo(colectivo.TarifaBasica), "El monto del segundo viaje debe ser igual a la tarifa básica.");

            // Avanzar el tiempo en el objeto TiempoFalso para simular que ha pasado 1 minuto
            tiempoFalso.AgregarMinutos(5);

            // Ahora intentamos realizar un tercer viaje después de 1 minuto
            var tercerViaje = colectivo.PagarCon(medioBoleto);
            Assert.IsNotNull(tercerViaje, "El tercer viaje debería ser permitido después de esperar 5 minutos.");
        }








        [Test]
        public void ViajesMedioBoleto_DebenCumplirReglasDeTiempoYLimite()
        {
            // Arrange
            medioBoleto.CargarSaldo(5000); // Cargar saldo suficiente
            var primerViaje = colectivo.PagarCon(medioBoleto); // Primer viaje, debe ser exitoso
            Assert.IsNotNull(primerViaje, "El primer viaje debería ser permitido.");

            // Simular el paso del tiempo
            tiempoFalso.AgregarMinutos(5); // Simular que ha pasado 5 minutos

            // Act: Intentar realizar el segundo viaje después de 5 minutos
            var segundoViaje = colectivo.PagarCon(medioBoleto);  // Segundo viaje, debería ser exitoso
            Assert.IsNotNull(segundoViaje, "El segundo viaje debería ser permitido después de 5 minutos.");

            // Act: Acelerar el tiempo y realizar un tercer viaje
            tiempoFalso.AgregarMinutos(5); // Simular que han pasado otros 5 minutos
            var tercerViaje = colectivo.PagarCon(medioBoleto); // Tercer viaje, debería ser exitoso
            Assert.IsNotNull(tercerViaje, "El tercer viaje debería ser permitido después de 5 minutos.");

            // Act: Intentar realizar un cuarto viaje
            tiempoFalso.AgregarMinutos(5); // Simular que han pasado otros 5 minutos
            var cuartoViaje = colectivo.PagarCon(medioBoleto); // Cuarto viaje, debería ser exitoso
            Assert.IsNotNull(cuartoViaje, "El cuarto viaje debería ser permitido después de 5 minutos.");

            // Act: Intentar realizar un quinto viaje (debería cobrar tarifa normal)
            var quintoViaje = colectivo.PagarCon(medioBoleto); // Quinto viaje, debería ser permitido pero a tarifa normal
            Assert.IsNotNull(quintoViaje, "El quinto viaje debería ser permitido.");
            Assert.That(quintoViaje.TotalAbonado, Is.EqualTo(colectivo.TarifaBasica), "El quinto viaje debería ser a tarifa normal.");

            // Simular el paso del tiempo
            tiempoFalso.AgregarMinutos(5); // Simular que han pasado otros 5 minutos

            // Act: Intentar realizar un sexto viaje
            var sextoViaje = colectivo.PagarCon(medioBoleto); // Debería ser permitido, pero a tarifa normal
            Assert.IsNotNull(sextoViaje, "El sexto viaje debería ser permitido, pero a tarifa normal.");
            Assert.That(sextoViaje.TotalAbonado, Is.EqualTo(colectivo.TarifaBasica), "El sexto viaje debería ser a tarifa normal.");
        }


    }
}

/*using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;
using System.Threading;
using static TarjetaNamespace.Tarjeta;

[TestFixture]
public class ColectivoTests
{
    private Colectivo colectivo;
    private MedioBoleto tarjeta;

    [SetUp]
    public void Setup()
    {
        colectivo = new Colectivo("linea 120");
        tarjeta = new MedioBoleto(0);
        tarjeta.CargarSaldo(2000);
    }
    
    [Test]
    public void NoDeberiaPermitirViajeDentroDeCincoMinutosConMedioBoleto()
    {

        bool viaje1 = colectivo.PagarCon(tarjeta) != null;

        bool viaje2 = colectivo.PagarCon(tarjeta) != null;

        Assert.That(viaje1, Is.True, "El primer viaje debería ser permitido.");
        Assert.That(viaje2, Is.False, "El segundo viaje no debería ser permitido, debe esperar 5 minutos.");

        Thread.Sleep(60000*1);

        bool viaje3 = colectivo.PagarCon(tarjeta) != null;

        Assert.That(viaje3, Is.True, "El tercer viaje debería ser permitido después de esperar 5 minutos.");
    }

    
    [Test]
    public void QuintoViajeConMedioBoletoDeberiaSerTarifaNormal()
    {
        colectivo.PagarCon(tarjeta);
        Thread.Sleep(60000 *5);
        colectivo.PagarCon(tarjeta);
        Thread.Sleep(60000*5);
        colectivo.PagarCon(tarjeta);
        Thread.Sleep(60000*5);
        colectivo.PagarCon(tarjeta);
        Thread.Sleep(60000*5);


        decimal saldoAntes = tarjeta.Saldo;


        bool viaje5 = colectivo.PagarCon(tarjeta) != null;


        decimal tarifaNormal = tarjeta.CalcularTarifa(tarjeta);

        Assert.That(viaje5, Is.True, "El quinto viaje debería ser permitido, pero con tarifa normal.");
        Assert.That(tarjeta.Saldo, Is.EqualTo(saldoAntes - tarifaNormal).Within(0.01m), "El saldo debería ajustarse al cobrar la tarifa normal del quinto viaje.");
    }
   

}*/