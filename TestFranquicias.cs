using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;
using BoletoNamespace;
using static TarjetaNamespace.Tarjeta;
using ManejoDeTiempos;

namespace TestsFranquicias
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


    }
}
