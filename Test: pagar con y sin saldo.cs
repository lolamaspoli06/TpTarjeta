using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;
using BoletoNamespace;

namespace Tests
{
    public class PagarConSaldoTests
    {
        [Test]
        public void Test_PagarConSaldoSuficiente()
        {

            var tarjeta = new Tarjeta(0);
            var colectivo = new Colectivo();
            tarjeta.CargarSaldo(2000);  


            colectivo.PagarCon(tarjeta);


            Assert.AreEqual(1060, tarjeta.Saldo); 
        }

        [Test]
        public void Test_PagarConSaldoInsuficiente()
        {

            var tarjeta = new Tarjeta(0);
            var colectivo = new Colectivo();
            tarjeta.CargarSaldo(2000); 


            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);
            colectivo.PagarCon(tarjeta);


            Assert.AreEqual(120, tarjeta.Saldo); 
        }
    }
}
