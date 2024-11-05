using NUnit.Framework;
using TarjetaNamespace;


namespace TestsIteracion1
{
    [TestFixture]
    public class TarjetaTests
    {
        [Test]
        [TestCase(2000)]
        [TestCase(3000)]
        [TestCase(4000)]
        [TestCase(5000)]
        [TestCase(6000)]
        [TestCase(7000)]
        [TestCase(8000)]
        [TestCase(9000)]
        public void CargarSaldo_ConMontosValidos_DeberiaCargarCorrectamente(int monto)
        {
            // Arrange
            var tarjeta = new Tarjeta(0);

            // Act
            tarjeta.CargarSaldo(monto);

            // Assert
            Assert.AreEqual(monto, tarjeta.Saldo, $"El saldo cargado debería ser {monto}, pero fue {tarjeta.Saldo}");
        }

        [Test]
        [TestCase(1000)]
        [TestCase(1500)]
        [TestCase(2500)]
        [TestCase(50000)]
        public void CargarSaldo_ConMontosInvalidos_NoDeberiaCargarSaldo(int monto)
        {
            // Arrange
            var tarjeta = new Tarjeta(0);

            // Act
            tarjeta.CargarSaldo(monto);

            // Assert
            Assert.AreEqual(0, tarjeta.Saldo, $"El saldo debería ser 0, pero fue {tarjeta.Saldo}");
        }
    }
}
