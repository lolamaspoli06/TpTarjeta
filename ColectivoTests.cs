using NUnit.Framework;
using ManejoDeTiempos;
using TarjetaNamespace;
using ColectivoNamespace;
using BoletoNamespace;

namespace ColectivoTest
{
    [TestFixture]
    public class ColectivoTests
    {
        private Tarjeta tarjeta;
        private Colectivo colectivo;
        private Tarjeta interurbana; 

        [SetUp]
        public void Setup()
        {
            var tiempo = new Tiempo();
            colectivo = new Colectivo("linea 120", tiempo);
            tarjeta = new Tarjeta(2000);
            interurbana = new Tarjeta(10000);
            decimal tarifaInterurbana = interurbana.tarifaInterurbana;
    }

        public class FranquiciaHorarioTests
        {
            private TiempoFalso tiempo;
            private Colectivo colectivo;
            private Tarjeta tarjeta;

            [SetUp]
            public void Setup()
            {
                tiempo = new TiempoFalso();
                colectivo = new Colectivo("Linea 120", tiempo);
                tarjeta = new Tarjeta(10000);
                decimal tarifaBasica = tarjeta.tarifaBasica;
            }

            [Test]
            public void NoPermiteViajeConMedioBoletoFueraDeHorario()
            {
                var medioBoleto = new Tarjeta.MedioBoleto(500);


                tiempo.AgregarDias(5);
                tiempo.AgregarMinutos(300);

                var boleto = colectivo.PagarCon(medioBoleto);
                Assert.IsNull(boleto, "No se debería permitir un viaje con Medio Boleto fuera del horario permitido.");
            }

            [Test]
            public void NoPermiteViajeConBoletoGratuitoFueraDeHorario()
            {
                var boletoGratuito = new Tarjeta.BoletoGratuito(500);


                tiempo.AgregarDias(1);
                tiempo.AgregarMinutos(1380);

                var boleto = colectivo.PagarCon(boletoGratuito);
                Assert.IsNull(boleto, "No se debería permitir un viaje con Boleto Gratuito fuera del horario permitido.");
            }

            [Test]
            public void PermiteViajeConMedioBoletoEnHorarioPermitido()
            {
                var medioBoleto = new Tarjeta.MedioBoleto(500);


                tiempo.AgregarDias(2);
                tiempo.AgregarMinutos(600);

                var boleto = colectivo.PagarCon(medioBoleto);
                Assert.IsNotNull(boleto, "Se debería permitir un viaje con Medio Boleto en el horario permitido.");
            }

            [Test]
            public void PermiteViajeConBoletoGratuitoEnHorarioPermitido()
            {
                var boletoGratuito = new Tarjeta.BoletoGratuito(500);


                tiempo.AgregarDias(3);
                tiempo.AgregarMinutos(900);

                var boleto = colectivo.PagarCon(boletoGratuito);
                Assert.IsNotNull(boleto, "Se debería permitir un viaje con Boleto Gratuito en el horario permitido.");
            }
        }

        //[Test]
        //public void TestTarifaInterurbana()
        //{
        //    var colectivoInterurbano = new Colectivo("Linea Interurbana", new Tiempo(), true);
        //    decimal tarifa = tarjeta.CalcularTarifa();
        //    Assert.AreEqual(interurbana.tarifaInterurbana, tarifa);
        //}

        //[Test]
        //public void TestTarifaUrbana()
        //{
        //    decimal tarifa = tarjeta.CalcularTarifa();
        //    Assert.AreEqual(interurbana.TarifaBasica, tarifa); 
        //}
    }
}
