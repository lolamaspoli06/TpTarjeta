using System;
using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;
using BoletoNamespace;

namespace Tests
{
    [TestFixture]
    public class BoletoGratuitoTests
    {
        private const decimal TarifaCompleta = 940m;

        [Test]
        public void NoPermitirMasDeDosViajesGratuitosPorDia()
        {
            // Arrange
            Colectivo colectivo = new Colectivo("linea 120");
            Tarjeta.BoletoGratuito tarjetaGratuita = new Tarjeta.BoletoGratuito(0);

            // Act
            Boleto primerViaje = colectivo.PagarCon(tarjetaGratuita);
            Boleto segundoViaje = colectivo.PagarCon(tarjetaGratuita);
            Boleto tercerViaje = colectivo.PagarCon(tarjetaGratuita);

            // Assert
            Assert.IsNotNull(primerViaje, "El primer viaje gratuito debería estar permitido.");
            Assert.IsNotNull(segundoViaje, "El segundo viaje gratuito debería estar permitido.");
            Assert.IsNull(tercerViaje, "El tercer viaje gratuito no debería estar permitido en el mismo día.");
        }

        [Test]
        public void CobrarPrecioCompletoAPartirDelTercerViajeGratuito()
        {
            // Arrange
            Colectivo colectivo = new Colectivo("linea 120");
            Tarjeta.BoletoGratuito tarjetaGratuita = new Tarjeta.BoletoGratuito(TarifaCompleta); // Inicializar con saldo suficiente

            // Act
            colectivo.PagarCon(tarjetaGratuita); // Primer viaje gratuito
            colectivo.PagarCon(tarjetaGratuita); // Segundo viaje gratuito
            Boleto tercerViaje = colectivo.PagarCon(tarjetaGratuita); // Tercer viaje, debería cobrar tarifa completa

            // Assert
            Assert.IsNotNull(tercerViaje, "El tercer viaje debería permitirse.");
            Assert.AreEqual(TarifaCompleta, tercerViaje.TotalAbonado, "El tercer viaje debería cobrarse a tarifa completa.");
            Assert.AreEqual(TarifaCompleta - TarifaCompleta, tarjetaGratuita.Saldo, "El saldo restante debería reflejar la tarifa completa cobrada en el tercer viaje.");
        }
    }
}

