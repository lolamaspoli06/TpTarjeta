using NUnit.Framework;
using TarjetaNamespace;

[TestFixture]
public class TarjetaTests
{
    [Test]
    public void VerificarAcreditacionTrasViaje()
    {
        // Arrange: Crear una tarjeta con saldo inicial
        var tarjeta = new Tarjeta(32000); // Inicializando con saldo de 32,000
        tarjeta.CargarSaldo(10000); // Intentar cargar 10,000 (excediendo el límite)

        // Act: Simular un viaje que descuenta la tarifa básica
        bool viajeExitoso = tarjeta.DescontarPasaje(tarjeta.CalcularTarifa(tarjeta)); // Descontar tarifa básica

        // Verificación del estado de la tarjeta después del viaje
        Assert.IsTrue(viajeExitoso, "El viaje debería haber sido exitoso.");

        // Acreditar el saldo pendiente
        tarjeta.AcreditarSaldoPendiente();

        // Assert: Verificar que el saldo se ha acreditado correctamente
        Assert.AreEqual(36000, tarjeta.Saldo, "El saldo tras la acreditación debe ser 36,000.");
        Assert.AreEqual(4000, tarjeta.SaldoPendiente, "El saldo pendiente debe ser 4,000.");
    }
}
