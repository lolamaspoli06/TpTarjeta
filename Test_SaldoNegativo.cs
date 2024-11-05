using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;
using BoletoNamespace;
using ManejoDeTiempos;

namespace TestsSaldoNegativo
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

    }
}
