using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;
using BoletoNamespace;
using static TarjetaNamespace.Tarjeta;
using ManejoDeTiempos;

namespace Tests
{
    [TestFixture]
    public class TipoBoletos
    {
        private const decimal tarifa = 940m;
        private TiempoFalso tiempoFalso;
        private Colectivo colectivo;
        private Tarjeta.BoletoGratuito tarjetaGratuita;

        [SetUp]
        public void SetUp()
        {
            tiempoFalso = new TiempoFalso(); // Usamos TiempoFalso para simular el tiempo
            colectivo = new Colectivo("linea 120", tiempoFalso);
            tarjetaGratuita = new Tarjeta.BoletoGratuito(tarifa); // Inicializamos la tarjeta con saldo suficiente
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

            Assert.That(tarjeta.Saldo, Is.EqualTo(2000 - tarifa/2), "Es un medio boleto, el precio deberia ser la mitad (470)");
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
