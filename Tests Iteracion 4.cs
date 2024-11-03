using NUnit.Framework;
using ManejoDeTiempos;
using TarjetaNamespace;
using ColectivoNamespace;
using BoletoNamespace;

namespace Tests
{
    [TestFixture]
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
            decimal tarifaBasica = tarjeta.TarifaBasica;
        }

        [Test]
        public void NoPermiteViajeConMedioBoletoFueraDeHorario()
        {
            var medioBoleto = new Tarjeta.MedioBoleto(500);


            tiempo.AgregarDias(5);
            tiempo.AgregarMinutos(300);

            var boleto = colectivo.PagarCon(medioBoleto, colectivo);
            Assert.IsNull(boleto, "No se debería permitir un viaje con Medio Boleto fuera del horario permitido.");
        }

        [Test]
        public void NoPermiteViajeConBoletoGratuitoFueraDeHorario()
        {
            var boletoGratuito = new Tarjeta.BoletoGratuito(500);


            tiempo.AgregarDias(1);
            tiempo.AgregarMinutos(1380);

            var boleto = colectivo.PagarCon(boletoGratuito, colectivo);
            Assert.IsNull(boleto, "No se debería permitir un viaje con Boleto Gratuito fuera del horario permitido.");
        }

        [Test]
        public void PermiteViajeConMedioBoletoEnHorarioPermitido()
        {
            var medioBoleto = new Tarjeta.MedioBoleto(500);


            tiempo.AgregarDias(2);
            tiempo.AgregarMinutos(600);

            var boleto = colectivo.PagarCon(medioBoleto, colectivo);
            Assert.IsNotNull(boleto, "Se debería permitir un viaje con Medio Boleto en el horario permitido.");
        }

        [Test]
        public void PermiteViajeConBoletoGratuitoEnHorarioPermitido()
        {
            var boletoGratuito = new Tarjeta.BoletoGratuito(500);


            tiempo.AgregarDias(3);
            tiempo.AgregarMinutos(900);

            var boleto = colectivo.PagarCon(boletoGratuito, colectivo);
            Assert.IsNotNull(boleto, "Se debería permitir un viaje con Boleto Gratuito en el horario permitido.");
        }
    }

    [TestFixture]
    public class TarjetaTests
    {
        private Tarjeta tarjeta;
        private Colectivo colectivo;

        [SetUp]
        public void Setup()
        {
            var tiempo = new Tiempo();
            colectivo = new Colectivo("linea 120", tiempo);
            tarjeta = new Tarjeta(5000);
            decimal tarifaBasica = tarjeta.TarifaBasica;
        }

        [Test]
        public void TestTarifaNormalSinDescuento()
        {
            tarjeta = new Tarjeta(10000) { ViajesEsteMes = 10 };
            decimal tarifa = tarjeta.CalcularTarifa(colectivo);
            Assert.AreEqual(tarjeta.tarifaBasica, tarifa);
        }

        [Test]
        public void TestTarifaConDescuento20()
        {
            tarjeta = new Tarjeta(10000) { ViajesEsteMes = 30 };
            decimal tarifa = tarjeta.CalcularTarifa(colectivo);
            Assert.AreEqual(tarjeta.tarifaBasica * 0.8m, tarifa);
        }

        [Test]
        public void TestTarifaConDescuento25()
        {
            tarjeta = new Tarjeta(10000) { ViajesEsteMes = 80 };
            decimal tarifa = tarjeta.CalcularTarifa(colectivo);
            Assert.AreEqual(tarjeta.tarifaBasica * 0.75m, tarifa);
        }

        [Test]
        public void TestTarifaNormalConMasDe80Viajes()
        {
            tarjeta = new Tarjeta(10000) { ViajesEsteMes = 81 };
            decimal tarifa = tarjeta.CalcularTarifa(colectivo);
            Assert.AreEqual(tarjeta.tarifaBasica, tarifa); 
        }

        [Test]
        public void MedioBoleto_NoAplicaDescuentoFrecuente()
        {
            var medioBoleto = new Tarjeta.MedioBoleto(10000);
            decimal tarifaEsperada = tarjeta.tarifaBasica / 2;

            for (int i = 1; i <= 80; i++)
            {
                decimal tarifa = medioBoleto.CalcularTarifa(colectivo);
                Assert.That(tarifa, Is.EqualTo(tarifaEsperada).Within(0.01), $"Viaje {i}: tarifa incorrecta para Medio Boleto");

                medioBoleto.DescontarPasaje(tarifa);
                medioBoleto.ActualizarUltimoUso();
            }
        }

        [Test]
        public void BoletoGratuito_NoAplicaDescuentoFrecuente()
        {
            var boletoGratuito = new Tarjeta.BoletoGratuito(10000);

            for (int i = 1; i <= 80; i++)
            {
                decimal tarifa = boletoGratuito.CalcularTarifa(colectivo);
                Assert.That(tarifa, Is.EqualTo(0), $"Viaje {i}: tarifa incorrecta para Boleto Gratuito");

                boletoGratuito.DescontarPasaje(0);
                boletoGratuito.ActualizarUltimoUso();
            }
        }
    }

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

        [Test]
        public void TestTarifaInterurbana()
        {
            var colectivoInterurbano = new Colectivo("Linea Interurbana", new Tiempo(), true);
            decimal tarifa = tarjeta.CalcularTarifa(colectivoInterurbano);
            Assert.AreEqual(interurbana.tarifaInterurbana, tarifa);
        }

        [Test]
        public void TestTarifaUrbana()
        {
            decimal tarifa = tarjeta.CalcularTarifa(colectivo);
            Assert.AreEqual(interurbana.TarifaBasica, tarifa); 
        }
    }
}
