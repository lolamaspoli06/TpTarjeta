using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;
using BoletoNamespace;
using ManejoDeTiempos;

namespace TestsPagarConySinSaldo
{
    public class PagarConSaldoTests
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

        public void Test_PagarConSaldoSuficiente()
        {

            tarjeta = new Tarjeta(0);
            colectivo = new Colectivo("Línea 120", tiempoFalso);
            tarjeta.CargarSaldo(2000);  


            colectivo.PagarCon(tarjeta);


            Assert.AreEqual(1060, tarjeta.Saldo); 
        }

        [Test]
        public void Test_PagarConSaldoInsuficiente()
        {

            var tarjeta = new Tarjeta(0);
            var colectivo = new Colectivo("Línea 120", tiempoFalso);
            tarjeta.CargarSaldo(2000); 


            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);


            Assert.AreEqual(120, tarjeta.Saldo); 
        }
    }
}
