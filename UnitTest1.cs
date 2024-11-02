using NUnit.Framework;
using ManejoDeTiempos;
using TarjetaNamespace;
using ColectivoNamespace;
using BoletoNamespace;
using static TarjetaNamespace.Tarjeta;

namespace Tests
{
    [TestFixture]
    public class FranquiciaHorarioTests
    {
        private TiempoFalso tiempo;
        private Colectivo colectivo;

        [SetUp]
        public void Setup()
        {
            tiempo = new TiempoFalso();
            colectivo = new Colectivo("Linea 120", tiempo);
        }
        //los primeros dos casos son para mostrar los casos que no deberia permitir las franquicias
        [Test]
        public void NoPermiteViajeConMedioBoletoFueraDeHorario()
        {
            Tarjeta medioBoleto = new MedioBoleto(500);

            // Cambia la hora a un sábado a las 5:00 (fuera de horario permitido)
            tiempo.AgregarDias(5); // Lunes + 5 días = Sábado
            tiempo.AgregarMinutos(300); // 5:00 am

            Boleto boleto = colectivo.PagarCon(medioBoleto);
            Assert.IsNull(boleto, "No se debería permitir un viaje con Medio Boleto fuera del horario permitido.");
        }

        [Test]
        public void NoPermiteViajeConBoletoGratuitoFueraDeHorario()
        {
            Tarjeta boletoGratuito = new BoletoGratuito(500);

            // Cambia la hora a un martes a las 23:00 (fuera de horario permitido)
            tiempo.AgregarDias(1); // Lunes + 1 día = Martes
            tiempo.AgregarMinutos(1380); // 23:00

            Boleto boleto = colectivo.PagarCon(boletoGratuito);
            Assert.IsNull(boleto, "No se debería permitir un viaje con Boleto Gratuito fuera del horario permitido.");
        }

        //los ultimos dos casos son para mostrar los casos que se deberian permitir las franquicias
        [Test]
        public void PermiteViajeConMedioBoletoEnHorarioPermitido()
        {
            Tarjeta medioBoleto = new MedioBoleto(500);

            // Cambia la hora a un miércoles a las 10:00 (dentro del horario permitido)
            tiempo.AgregarDias(2); // Lunes + 2 días = Miércoles
            tiempo.AgregarMinutos(600); // 10:00 am 

            Boleto boleto = colectivo.PagarCon(medioBoleto);
            Assert.IsNotNull(boleto, "Se debería permitir un viaje con Medio Boleto en el horario permitido.");
        }

        [Test]
        public void PermiteViajeConBoletoGratuitoEnHorarioPermitido()
        {
            Tarjeta boletoGratuito = new BoletoGratuito(500);

            // Cambia la hora a un jueves a las 15:00 (dentro del horario permitido)
            tiempo.AgregarDias(3); // Lunes + 3 días = Jueves
            tiempo.AgregarMinutos(900); // 15:00 pm

            Boleto boleto = colectivo.PagarCon(boletoGratuito);
            Assert.IsNotNull(boleto, "Se debería permitir un viaje con Boleto Gratuito en el horario permitido.");
        }
    }


    [TestFixture]
    public class TarjetaTests
    {

        [Test]
        public void TestTarifaNormalSinDescuento()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta(10000) { ViajesEsteMes = 10 };

            // Act
            decimal tarifa = tarjeta.CalcularTarifa();

            // Assert
            Assert.AreEqual(1200, tarifa);
        }

        [Test]
        public void TestTarifaConDescuento20()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta(10000) { ViajesEsteMes = 30 };

            // Act
            decimal tarifa = tarjeta.CalcularTarifa();

            // Assert
            Assert.AreEqual(960, tarifa); // 20% de descuento
        }

        [Test]
        public void TestTarifaConDescuento25()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta(10000) { ViajesEsteMes = 80 };

            // Act
            decimal tarifa = tarjeta.CalcularTarifa();

            // Assert
            Assert.AreEqual(900, tarifa); // 25% de descuento
        }

        [Test]
        public void TestTarifaNormalConMasDe80Viajes()
        {
            // Arrange
            Tarjeta tarjeta = new Tarjeta(10000) { ViajesEsteMes = 81 };

            // Act
            decimal tarifa = tarjeta.CalcularTarifa();

            // Assert
            Assert.AreEqual(1200, tarifa); // Debe ser la tarifa normal
        }


        [Test]
        public void MedioBoleto_NoAplicaDescuentoFrecuente()
        {
            var medioBoleto = new Tarjeta.MedioBoleto(10000);
            decimal tarifaEsperada = 1200 / 2; // La mitad de la tarifa normal

            for (int i = 1; i <= 80; i++)
            {
                // Calcula la tarifa usando el método sin argumentos
                decimal tarifa = medioBoleto.CalcularTarifa();
                Assert.That(tarifa, Is.EqualTo(tarifaEsperada).Within(0.01),
                            $"Viaje {i}: tarifa incorrecta para Medio Boleto");

                medioBoleto.DescontarPasaje(tarifa);
                medioBoleto.ActualizarUltimoUso(); // Incrementar viajes del mes
            }
        }

        [Test]
        public void BoletoGratuito_NoAplicaDescuentoFrecuente()
        {
            var boletoGratuito = new Tarjeta.BoletoGratuito(10000);

            for (int i = 1; i <= 80; i++)
            {
                // Calcula la tarifa usando el método sin argumentos
                decimal tarifa = boletoGratuito.CalcularTarifa();
                Assert.That(tarifa, Is.EqualTo(0), $"Viaje {i}: tarifa incorrecta para Boleto Gratuito");

                boletoGratuito.DescontarPasaje(0);
                boletoGratuito.ActualizarUltimoUso(); // Incrementar viajes del mes
            }
        }
    }
}