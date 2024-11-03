using NUnit.Framework;
using ColectivoNamespace;
using ManejoDeTiempos;
using TarjetaNamespace;

namespace ColectivoNamespace.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        private Tarjeta tarjeta;
        private Colectivo colectivo;
        private Tiempo tiempo;

        [SetUp]
        public void Setup()
        {
            tiempo = new Tiempo();
            colectivo = new Colectivo("linea 120", tiempo);
            tarjeta = new Tarjeta(2000);
        }
        [Test]
        public void TestTarifaInterurbana()
        {
            // Arrange
            Tiempo tiempo = new Tiempo(); // 
            Colectivo colectivoInterurbano = new Colectivo("Linea Interurbana", tiempo, true);


            decimal tarifa = tarjeta.CalcularTarifa( colectivoInterurbano);

            // Assert
            Assert.AreEqual(2500m, tarifa); // Verifica que la tarifa sea la esperada
        }

        [Test]
        public void TestTarifaUrbana()
        {
            // Arrange
            Tiempo tiempo = new Tiempo();
            Colectivo colectivoUrbano = new Colectivo("Linea Urbana", tiempo, false);

            // Act
            decimal tarifa = tarjeta.CalcularTarifa(colectivoUrbano);

            // Assert
            Assert.AreEqual(1200m, tarifa); // Verifica que la tarifa urbana sea la esperada
        }
    }
}
