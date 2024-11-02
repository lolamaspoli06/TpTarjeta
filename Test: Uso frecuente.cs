using NUnit.Framework;
using System;
using TarjetaNamespace;

namespace TarjetaNamespace.Tests
{
    [TestFixture]
    public class TarjetaTests
    {

        [Test]
        public void TestTarifaNormalSinDescuento()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta(10000) { ViajesEsteMes = 10 };

            // Act
            decimal tarifa = tarjeta.CalcularTarifa();

            // Assert
            Assert.AreEqual(1200, tarifa);
        }

        [Test]
        public void TestTarifaConDescuento20()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta(10000) { ViajesEsteMes = 30 };

            // Act
            decimal tarifa = tarjeta.CalcularTarifa();

            // Assert
            Assert.AreEqual(960, tarifa); // 20% de descuento
        }

        [Test]
        public void TestTarifaConDescuento25()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta(10000) { ViajesEsteMes = 80 };

            // Act
            decimal tarifa = tarjeta.CalcularTarifa();

            // Assert
            Assert.AreEqual(900, tarifa); // 25% de descuento
        }

        [Test]
        public void TestTarifaNormalConMasDe80Viajes()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta(10000) { ViajesEsteMes = 81 };

            // Act
            decimal tarifa = tarjeta.CalcularTarifa();

            // Assert
            Assert.AreEqual(1200, tarifa); // Debe ser la tarifa normal
        }


        [Test]
        public void MedioBoleto_NoAplicaDescuentoFrecuente()
        {
            var medioBoleto = new Tarjeta.MedioBoleto(10000);
            decimal tarifaEsperada = 1200 / 2; // La mitad de la tarifa normal

            for (int i = 1; i <= 80; i++)
            {
                // Calcula la tarifa usando el método sin argumentos
                decimal tarifa = medioBoleto.CalcularTarifa();
                Assert.That(tarifa, Is.EqualTo(tarifaEsperada).Within(0.01),
                            $"Viaje {i}: tarifa incorrecta para Medio Boleto");

                medioBoleto.DescontarPasaje(tarifa);
                medioBoleto.ActualizarUltimoUso(); // Incrementar viajes del mes
            }
        }

        [Test]
        public void BoletoGratuito_NoAplicaDescuentoFrecuente()
        {
            var boletoGratuito = new Tarjeta.BoletoGratuito(10000);

            for (int i = 1; i <= 80; i++)
            {
                // Calcula la tarifa usando el método sin argumentos
                decimal tarifa = boletoGratuito.CalcularTarifa();
                Assert.That(tarifa, Is.EqualTo(0), $"Viaje {i}: tarifa incorrecta para Boleto Gratuito");

                boletoGratuito.DescontarPasaje(0);
                boletoGratuito.ActualizarUltimoUso(); // Incrementar viajes del mes
            }
        }
    }
}
