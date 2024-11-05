using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;

namespace TestsSaldoAcreditacion
{

    [TestFixture]
    public class TarjetaTests
    {
        [Test]
        public void CargarSaldo_SuperaLimite_SaldoPendienteAlmacenado()
        {

            var tarjeta = new Tarjeta(35000);
            decimal montoACargar = 5000;

            bool resultadoCarga = tarjeta.CargarSaldo(montoACargar);

            Assert.That(resultadoCarga, Is.True, "La carga deberia ser exitosa.");
            Assert.That(tarjeta.Saldo, Is.EqualTo(36000), $"El saldo deberia estar en el máximo permitido, pero fue {tarjeta.Saldo}.");
            Assert.That(tarjeta.SaldoPendiente, Is.EqualTo(4000), $"El saldo pendiente deberia ser 4000, pero fue {tarjeta.SaldoPendiente}.");
        }
        [Test]
        public void RealizarViaje_AcreditaSaldoPendiente()
        {

            var tarjeta = new Tarjeta(36000);
            tarjeta.CargarSaldo(5000);

            var colectivo = new Colectivo("Línea 120");
            decimal saldoAntesDeViaje = tarjeta.Saldo;

            var boleto = colectivo.PagarCon(tarjeta); // Realiza el viaje


            // Assert
            Assert.That(boleto, Is.Not.Null, "El boleto debería ser creado después del viaje.");
            Assert.That(tarjeta.Saldo, Is.EqualTo(36000), "El saldo debería reacreditar hasta el máximo.");
            Assert.That(tarjeta.SaldoPendiente, Is.EqualTo(4060), "El saldo pendiente debería reflejar la diferencia después del viaje que es 4060.");
        }

    }
}

