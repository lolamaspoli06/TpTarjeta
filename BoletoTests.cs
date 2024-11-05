using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;
using BoletoNamespace;
using static TarjetaNamespace.Tarjeta;
using ManejoDeTiempos;

namespace TestsTp
{
    [TestFixture]
    public class BoletoTest
    {
        private const decimal tarifa = 940m;
        private TiempoFalso tiempoFalso;
        private Colectivo colectivo;
        private Tarjeta.BoletoGratuito tarjetaGratuita;
        private Tarjeta tarjeta;

        [SetUp]
        public void SetUp()
        {
            tiempoFalso = new TiempoFalso(); // Usamos TiempoFalso para simular el tiempo
            colectivo = new Colectivo("linea 120", tiempoFalso);
            tarjetaGratuita = new Tarjeta.BoletoGratuito(tarifa); // Inicializamos la tarjeta con saldo suficiente
        }



        [Test]
        public void Test_FranquiciaCompletaSiemprePuedePagar()
        {

            tarjeta = new BoletoGratuito(0);
            colectivo = new Colectivo("Línea 120", tiempoFalso);
            tarjeta.CargarSaldo(0);


            colectivo.PagarCon(tarjeta);

            Assert.AreEqual(0, tarjeta.Saldo, "El saldo no deberia cambiar, ya que la tarjeta de Franquicia Completa no paga.");
        }



        [Test]
        public void Test_MedioBoletoPagaLaMitad()
        {

            var tarjeta = new MedioBoleto(0);
            var colectivo = new Colectivo("Línea 120", tiempoFalso);
            tarjeta.CargarSaldo(2000);


            colectivo.PagarCon(tarjeta);


            int saldoEsperado = 2000 - (940 / 2);
            Assert.AreEqual(saldoEsperado, tarjeta.Saldo, "El saldo deberia haberse descontado solo la mitad del boleto.");
        }





        [Test]
        public void Test_Normal()
        {
            var tarjeta = new Tarjeta(0);

            tarjeta.CargarSaldo(2000);

            colectivo.PagarCon(tarjeta);

            Assert.That(tarjeta.Saldo, Is.EqualTo(2000 - tarifa), "Es una tarjeta normal, el precio deberia ser completo (940)");
        }

        [Test]
        public void Test_MedioBoleto()
        {
            var tarjeta = new MedioBoleto(0);
            tarjeta.CargarSaldo(2000);

            colectivo.PagarCon(tarjeta);

            Assert.That(tarjeta.Saldo, Is.EqualTo(2000 - tarifa / 2), "Es un medio boleto, el precio deberia ser la mitad (470)");
        }

        [Test]
        public void Test_FranquiciaCompleta()
        {
            var tarjeta = new BoletoGratuito(0);
            tarjeta.CargarSaldo(2000);

            colectivo.PagarCon(tarjeta);

            Assert.That(tarjeta.Saldo, Is.EqualTo(2000), "Es franquicia completa, el precio deberia ser 0");
        }
    }
}