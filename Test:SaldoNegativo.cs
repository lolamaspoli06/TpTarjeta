using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;
using BoletoNamespace;

namespace Tests
{
    public class PagarConSaldoTests
    {
        [Test]
        public void Test_TarjetaNoQuedaConSaldoNegativo()
        {
 
            var tarjeta = new Tarjeta(0);
            var colectivo = new Colectivo();
            tarjeta.CargarSaldo(500);  


            colectivo.PagarCon(tarjeta); 


            Assert.GreaterOrEqual(tarjeta.Saldo, 0, "La tarjeta no deberIa quedar con saldo negativo.");
        }


        [Test]
        public void Test_DescuentoCorrectoDelSaldo()
        {

            var tarjeta = new Tarjeta(0);
            var colectivo = new Colectivo();
            tarjeta.CargarSaldo(2000); 


            colectivo.PagarCon(tarjeta); 


            int saldoEsperado = 2000 - 940;  
            Assert.AreEqual(saldoEsperado, tarjeta.Saldo, "El saldo no se desconto correctamente.");
        }

    }
}
