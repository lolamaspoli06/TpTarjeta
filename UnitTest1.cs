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

        //los primeros dos casos son para mostrar los casos que se deberian permitir las franquicias
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
}
