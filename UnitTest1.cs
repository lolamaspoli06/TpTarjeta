using System;
using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;
using BoletoNamespace;
using ManejoDeTiempos;

namespace Tests
{
    [TestFixture]
    public class BoletoGratuitoTests
    {
        private const decimal TarifaCompleta = 940m;
        private TiempoFalso tiempoFalso;
        private Colectivo colectivo;
        private Tarjeta.BoletoGratuito tarjetaGratuita;

        [SetUp]
        public void SetUp()
        {
            tiempoFalso = new TiempoFalso(); // Usamos TiempoFalso para simular el tiempo
            colectivo = new Colectivo("linea 120", tiempoFalso);
            tarjetaGratuita = new Tarjeta.BoletoGratuito(TarifaCompleta); // Inicializamos la tarjeta con saldo suficiente
        }

        [Test]
        public void NoPermitirMasDeDosViajesGratuitosPorDia()
        {
            // Act
            Boleto primerViaje = colectivo.PagarCon(tarjetaGratuita);
            Boleto segundoViaje = colectivo.PagarCon(tarjetaGratuita);
            Boleto tercerViaje = colectivo.PagarCon(tarjetaGratuita);

            // Assert
            Assert.That(primerViaje, Is.Not.Null, "El primer viaje gratuito debería estar permitido.");
            Assert.That(segundoViaje, Is.Not.Null, "El segundo viaje gratuito debería estar permitido.");
            Assert.That(tercerViaje, Is.Not.Null, "El tercer viaje debería estar permitido, pero a tarifa completa.");
            Assert.That(tercerViaje.TotalAbonado, Is.EqualTo(TarifaCompleta), "El tercer viaje en un mismo día no debería ser gratuito; debería cobrarse la tarifa completa.");

        }

        [Test]
        public void CobrarPrecioCompletoAPartirDelTercerViajeGratuito()
        {
            // Act
            Boleto primerViaje = colectivo.PagarCon(tarjetaGratuita);
            tiempoFalso.AgregarMinutos(5); //para que pase un tiempo entre viaje y viaje
            Boleto segundoViaje = colectivo.PagarCon(tarjetaGratuita);
            tiempoFalso.AgregarMinutos(5);
            Boleto tercerViaje = colectivo.PagarCon(tarjetaGratuita);

            // Assert
            Assert.That(primerViaje, Is.Not.Null, "El primer viaje debería estar permitido como gratuito.");
            Assert.That(segundoViaje, Is.Not.Null, "El segundo viaje debería estar permitido como gratuito.");
            Assert.That(tercerViaje, Is.Not.Null, "El tercer viaje debería permitirse, pero con tarifa completa.");
            Assert.That(tercerViaje.TotalAbonado, Is.EqualTo(TarifaCompleta), "El tercer viaje debería cobrarse a tarifa completa.");
            Assert.That(tarjetaGratuita.Saldo, Is.EqualTo(0), "El saldo restante debería reflejar la tarifa completa cobrada en el tercer viaje.");
        }
    }
}
