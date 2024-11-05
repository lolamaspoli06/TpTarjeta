using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;
using BoletoNamespace;
using ManejoDeTiempos;

namespace TarjetaTest
    
{
    [TestFixture]
    public class TarjetaTests
    {
        private Colectivo colectivo;
        private TiempoFalso tiempoFalso;
        private Tarjeta tarjeta;
        [SetUp]
        public void Setup()
        {
            tiempoFalso = new TiempoFalso();
            colectivo = new Colectivo("Línea 120", tiempoFalso);
        }


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
            var tarjeta = new Tarjeta(0);

            tarjeta.CargarSaldo(monto);

            Assert.AreEqual(0, tarjeta.Saldo, $"El saldo debería ser 0, pero fue {tarjeta.Saldo}");
        }


        [Test]
        public void Test_PagarConSaldoSuficiente()
        {

            tarjeta = new Tarjeta(0);
            colectivo = new Colectivo("Linea 120", tiempoFalso);
            tarjeta.CargarSaldo(2000);


            colectivo.PagarCon(tarjeta);


            Assert.AreEqual(1060, tarjeta.Saldo);
        }

        [Test]
        public void Test_PagarConSaldoInsuficiente()
        {

            var tarjeta = new Tarjeta(0);
            var colectivo = new Colectivo("Linea 120", tiempoFalso);
            tarjeta.CargarSaldo(2000);


            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);


            Assert.AreEqual(120, tarjeta.Saldo);
        }

        [Test]
        public void Test_TarjetaNoQuedaConSaldoNegativo()
        {

            tarjeta = new Tarjeta(0);
            colectivo = new Colectivo("Línea 120", tiempoFalso);
            tarjeta.CargarSaldo(500);


            colectivo.PagarCon(tarjeta);


            Assert.GreaterOrEqual(tarjeta.Saldo, 0, "La tarjeta no deberIa quedar con saldo negativo.");
        }


        [Test]
        public void Test_DescuentoCorrectoDelSaldo()
        {

            var tarjeta = new Tarjeta(0);
            var colectivo = new Colectivo("Línea 120", tiempoFalso);
            tarjeta.CargarSaldo(2000);


            colectivo.PagarCon(tarjeta);


            int saldoEsperado = 2000 - 940;
            Assert.AreEqual(saldoEsperado, tarjeta.Saldo, "El saldo no se desconto correctamente.");
        }



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

            var colectivo = new Colectivo("Línea 120", tiempoFalso);
            decimal saldoAntesDeViaje = tarjeta.Saldo;

            var boleto = colectivo.PagarCon(tarjeta); // Realiza el viaje


            // Assert
            Assert.That(boleto, Is.Not.Null, "El boleto debería ser creado después del viaje.");
            Assert.That(tarjeta.Saldo, Is.EqualTo(36000), "El saldo debería reacreditar hasta el máximo.");
            Assert.That(tarjeta.SaldoPendiente, Is.EqualTo(4060), "El saldo pendiente debería reflejar la diferencia después del viaje que es 4060.");
        }

    }
}

