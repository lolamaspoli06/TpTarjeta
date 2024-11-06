using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;
using BoletoNamespace;
using static TarjetaNamespace.Tarjeta;
using ManejoDeTiempos;

namespace BoletoTest
{
    [TestFixture]
    public class BoletoTest
    {
        private const decimal tarifa = 940m;
        private TiempoFalso tiempoFalso;
        private Colectivo colectivo;
        private BoletoGratuito tarjetaGratuita;
        private MedioBoleto medioBoleto;
        private Tarjeta tarjeta;
        private Tiempo tiempo;

        [SetUp]
        public void SetUp()
        {
            tiempoFalso = new TiempoFalso(); // Usamos TiempoFalso para simular el tiempo
            colectivo = new Colectivo("linea 120", tiempoFalso);
            tarjetaGratuita = new BoletoGratuito(tarifa); // Inicializamos la tarjeta gratuita
        }

        [Test]
        public void Test_FranquiciaCompletaSiemprePuedePagar()
        {
            tarjeta = new BoletoGratuito(0);
            tarjeta.CargarSaldo(0); // Cargar saldo cero, para simular que no tiene saldo

            // Intentar pagar con boleto gratuito
            Boleto boleto = colectivo.PagarConBoletoGratuito((BoletoGratuito)tarjeta);

            // Verificar que el saldo no cambió
            Assert.AreEqual(0, tarjeta.Saldo, "El saldo no debería cambiar, ya que la tarjeta de Franquicia Completa no paga.");
        }
        
        [Test]
        public void Test_MedioBoletoPagaLaMitad()
        {
            var tarjeta = new MedioBoleto(0);
            tarjeta.CargarSaldo(2000);

            // Intentar pagar con medio boleto
            Boleto boleto = colectivo.PagarConMedioBoleto(tarjeta);

            decimal saldoEsperado = 2000 - (tarifa / 2); // tarifa es 940
            Assert.AreEqual(saldoEsperado, tarjeta.Saldo, "El saldo debería haberse descontado solo la mitad del boleto.");
        }

        [Test]
        public void Test_Normal()
        {
            var tarjeta = new Tarjeta(0);
            tarjeta.CargarSaldo(2000);

            // Intentar pagar con tarjeta normal
            Boleto boleto = colectivo.PagarConTarjetaNormal(tarjeta);

            Assert.That(tarjeta.Saldo, Is.EqualTo(2000 - tarifa), "Es una tarjeta normal, el precio debería ser completo (940)");
        }

        [Test]
        public void Test_FranquiciaCompleta()
        {
            var tarjeta = new BoletoGratuito(0);
            tarjeta.CargarSaldo(2000);


            // Intentar pagar con boleto gratuito
            Boleto boleto = colectivo.PagarConBoletoGratuito((BoletoGratuito)tarjeta);

            Assert.That(tarjeta.Saldo, Is.EqualTo(2000), "Es franquicia completa, el precio debería ser 0");
        }
        [Test]
        public void NoPermiteViajeConMedioBoletoFueraDeHorario()
        {
            // Crear una instancia de MedioBoleto
            var medioBoleto = new MedioBoleto(500);

            // Usar TiempoFalso para simular una fecha y hora específicas
            var tiempoFalso = new TiempoFalso();  // Pasar la fecha actual al constructor de TiempoFalso

            // Avanzar el tiempo para estar fuera del horario permitido (por ejemplo, 10 horas)
            tiempoFalso.AgregarMinutos(60 * 10);  // Agregar 10 horas

            // Pasar el objeto tiempoFalso al método de pago
            var boleto = colectivo.PagarConMedioBoleto(medioBoleto);

            // Verificar que no se permite el viaje fuera del horario permitido
            Assert.IsNull(boleto, "No se debería permitir un viaje con Medio Boleto fuera del horario permitido.");
        }


    }
}
