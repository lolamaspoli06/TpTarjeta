using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;
using BoletoNamespace;
using static TarjetaNamespace.Tarjeta;
using ManejoDeTiempos;

namespace Tests
{
    [TestFixture]
    public class TipoBoletos
    {
        private const decimal tarifa = 940m;
        private TiempoFalso tiempoFalso;
        private Colectivo colectivo;
        private Tarjeta.BoletoGratuito tarjetaGratuita;

        [SetUp]
        public void SetUp()
        {
            tiempoFalso = new TiempoFalso(); // Usamos TiempoFalso para simular el tiempo
            colectivo = new Colectivo("linea 120", tiempoFalso);
            tarjetaGratuita = new Tarjeta.BoletoGratuito(tarifa); // Inicializamos la tarjeta con saldo suficiente
            
        }


        [Test]
        public void Test_Normal()
        {
            var tarjeta = new Tarjeta(0);

            tarjeta.CargarSaldo(2000);

            colectivo.PagarCon(tarjeta);

            Assert.That(tarjeta.Saldo, Is.EqualTo(2000 - tarifa), "Es una tarjeta normal, el precio deberia ser completo (940)");
        }

        [Test]
        public void Test_MedioBoleto()
        {
            var tarjeta = new MedioBoleto(0);
            tarjeta.CargarSaldo(2000);

            colectivo.PagarCon(tarjeta);

            Assert.That(tarjeta.Saldo, Is.EqualTo(2000 - tarifa/2), "Es un medio boleto, el precio deberia ser la mitad (470)");
        }

        [Test]
        public void Test_FranquiciaCompleta()
        {
            var tarjeta = new BoletoGratuito(0);
            tarjeta.CargarSaldo(2000);

            colectivo.PagarCon(tarjeta);

            Assert.That(tarjeta.Saldo, Is.EqualTo(2000), "Es franquicia completa, el precio deberia ser 0");
        }

        [Test]
        public void NoPermitirMasDeDosViajesGratuitosPorDia()
        {
            // Act
            Boleto primerViaje = colectivo.PagarCon(tarjetaGratuita);
            Boleto segundoViaje = colectivo.PagarCon(tarjetaGratuita);
            Boleto tercerViaje = colectivo.PagarCon(tarjetaGratuita);

            // Assert
            Assert.That(primerViaje, Is.Not.Null, "El primer viaje gratuito deber�a estar permitido.");
            Assert.That(segundoViaje, Is.Not.Null, "El segundo viaje gratuito deber�a estar permitido.");
            Assert.That(tercerViaje, Is.Not.Null, "El tercer viaje deber�a estar permitido, pero a tarifa completa.");
            Assert.That(tercerViaje.TotalAbonado, Is.EqualTo(tarifa), "El tercer viaje en un mismo d�a no deber�a ser gratuito; deber�a cobrarse la tarifa completa.");

        }

        [Test]
        public void CobrarPrecioCompletoAPartirDelTercerViajeGratuito()
        {
            // Act
            Boleto primerViaje = colectivo.PagarCon(tarjetaGratuita);
            tiempoFalso.AgregarMinutos(1); //para que pase un tiempo entre viaje y viaje
            Boleto segundoViaje = colectivo.PagarCon(tarjetaGratuita);
            tiempoFalso.AgregarMinutos(1);
            Boleto tercerViaje = colectivo.PagarCon(tarjetaGratuita);

            // Assert
            Assert.That(primerViaje, Is.Not.Null, "El primer viaje deber�a estar permitido como gratuito.");
            Assert.That(segundoViaje, Is.Not.Null, "El segundo viaje deber�a estar permitido como gratuito.");
            Assert.That(tercerViaje, Is.Not.Null, "El tercer viaje deber�a permitirse, pero con tarifa completa.");
            Assert.That(tercerViaje.TotalAbonado, Is.EqualTo(tarifa), "El tercer viaje deber�a cobrarse a tarifa completa.");
            Assert.That(tarjetaGratuita.Saldo, Is.EqualTo(0), "El saldo restante deber�a reflejar la tarifa completa cobrada en el tercer viaje.");
        }
    }
    public class ColectivoTests
    {
        private Colectivo colectivo;
        private TiempoFalso tiempoFalso;
        private Tarjeta medioBoleto;
        [SetUp]
        public void Setup()
        {
            tiempoFalso = new TiempoFalso();  // Crear instancia de TiempoFalso
            colectivo = new Colectivo("L�nea 120", tiempoFalso);  // Pasar tiempoFalso al Colectivo
            medioBoleto = new Tarjeta.MedioBoleto(4000); // Saldo inicial
        }
        [Test]
        public void NoDeberiaPermitirViajeEnMenosDe1MinutoConMedioBoleto()
        {
            // Simulamos el primer viaje
            var primerViaje = colectivo.PagarCon(medioBoleto);
            Assert.IsNotNull(primerViaje, "El primer viaje deber�a ser permitido.");
            // Intentamos realizar un segundo viaje inmediatamente (sin esperar 1 minuto)
            var segundoViaje = colectivo.PagarCon(medioBoleto);
            Assert.IsNotNull(segundoViaje, "El segundo viaje deber�a ser permitido, pero con tarifa b�sica.");
            Assert.That(segundoViaje.TotalAbonado, Is.EqualTo(colectivo.TarifaBasica), "El monto del segundo viaje debe ser igual a la tarifa b�sica.");
            // Avanzar el tiempo en el objeto TiempoFalso para simular que ha pasado 1 minuto
            tiempoFalso.AgregarMinutos(5);
            // Ahora intentamos realizar un tercer viaje despu�s de 1 minuto
            var tercerViaje = colectivo.PagarCon(medioBoleto);
            Assert.IsNotNull(tercerViaje, "El tercer viaje deber�a ser permitido despu�s de esperar 5 minutos.");
        }
        [Test]
        public void ViajesMedioBoleto_DebenCumplirReglasDeTiempoYLimite()
        {
            // Arrange
            medioBoleto.CargarSaldo(5000); // Cargar saldo suficiente
            var primerViaje = colectivo.PagarCon(medioBoleto); // Primer viaje, debe ser exitoso
            Assert.IsNotNull(primerViaje, "El primer viaje deber�a ser permitido.");
            // Simular el paso del tiempo
            tiempoFalso.AgregarMinutos(5); // Simular que ha pasado 5 minutos
            // Act: Intentar realizar el segundo viaje despu�s de 5 minutos
            var segundoViaje = colectivo.PagarCon(medioBoleto);  // Segundo viaje, deber�a ser exitoso
            Assert.IsNotNull(segundoViaje, "El segundo viaje deber�a ser permitido despu�s de 5 minutos.");
            // Act: Acelerar el tiempo y realizar un tercer viaje
            tiempoFalso.AgregarMinutos(5); // Simular que han pasado otros 5 minutos
            var tercerViaje = colectivo.PagarCon(medioBoleto); // Tercer viaje, deber�a ser exitoso
            Assert.IsNotNull(tercerViaje, "El tercer viaje deber�a ser permitido despu�s de 5 minutos.");
            // Act: Intentar realizar un cuarto viaje
            tiempoFalso.AgregarMinutos(5); // Simular que han pasado otros 5 minutos
            var cuartoViaje = colectivo.PagarCon(medioBoleto); // Cuarto viaje, deber�a ser exitoso
            Assert.IsNotNull(cuartoViaje, "El cuarto viaje deber�a ser permitido despu�s de 5 minutos.");
            // Act: Intentar realizar un quinto viaje (deber�a cobrar tarifa normal)
            var quintoViaje = colectivo.PagarCon(medioBoleto); // Quinto viaje, deber�a ser permitido pero a tarifa normal
            Assert.IsNotNull(quintoViaje, "El quinto viaje deber�a ser permitido.");
            Assert.That(quintoViaje.TotalAbonado, Is.EqualTo(colectivo.TarifaBasica), "El quinto viaje deber�a ser a tarifa normal.");
            // Simular el paso del tiempo
            tiempoFalso.AgregarMinutos(5); // Simular que han pasado otros 5 minutos
            // Act: Intentar realizar un sexto viaje
            var sextoViaje = colectivo.PagarCon(medioBoleto); // Deber�a ser permitido, pero a tarifa normal
            Assert.IsNotNull(sextoViaje, "El sexto viaje deber�a ser permitido, pero a tarifa normal.");
            Assert.That(sextoViaje.TotalAbonado, Is.EqualTo(colectivo.TarifaBasica), "El sexto viaje deber�a ser a tarifa normal.");
        }
    }
}

    