using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;
using ManejoDeTiempos;
using BoletoNamespace;

namespace Tests
{
    public class ColectivoTests
    {
        private const decimal TarifaCompleta = 940m;
        private Colectivo colectivo;
        private TiempoFalso tiempoFalso;
        private Tarjeta medioBoleto;
        private Tarjeta.BoletoGratuito tarjetaGratuita;

        [SetUp]
        public void Setup()
        {
            tiempoFalso = new TiempoFalso();
            colectivo = new Colectivo("Línea 120", tiempoFalso);
            medioBoleto = new Tarjeta.MedioBoleto(4000);
            tarjetaGratuita = new Tarjeta.BoletoGratuito(TarifaCompleta);
        }
        [Test]
        public void NoDeberiaPermitirViajeEnMenosDe5MinutosConMedioBoleto()
        {
            // Simulamos el primer viaje
            var primerViaje = colectivo.PagarCon(medioBoleto);
            Assert.IsNotNull(primerViaje, "El primer viaje debería ser permitido.");
            // Intentamos realizar un segundo viaje inmediatamente (sin esperar 1 minuto)
            var segundoViaje = colectivo.PagarCon(medioBoleto);
            Assert.IsNotNull(segundoViaje, "El segundo viaje debería ser permitido, pero con tarifa básica.");
            Assert.That(segundoViaje.TotalAbonado, Is.EqualTo(colectivo.TarifaBasica), "El monto del segundo viaje debe ser igual a la tarifa básica.");
            // Avanzar el tiempo en el objeto TiempoFalso para simular que ha pasado 1 minuto
            tiempoFalso.AgregarMinutos(5);
            // Ahora intentamos realizar un tercer viaje después de 1 minuto
            var tercerViaje = colectivo.PagarCon(medioBoleto);
            Assert.IsNotNull(tercerViaje, "El tercer viaje debería ser permitido después de esperar 5 minutos.");
        }
        [Test]
        public void ViajesMedioBoleto_DebenCumplirReglasDeTiempoYLimite()
        {
            medioBoleto.CargarSaldo(5000);
            var primerViaje = colectivo.PagarCon(medioBoleto); // Primer viaje, debe ser exitoso
            Assert.IsNotNull(primerViaje, "El primer viaje debería ser permitido.");
            // Simular el paso del tiempo
            tiempoFalso.AgregarMinutos(5); // Simular que ha pasado 5 minutos
            //  Intentar realizar el segundo viaje después de 5 minutos
            var segundoViaje = colectivo.PagarCon(medioBoleto);  // Segundo viaje, debería ser exitoso
            Assert.IsNotNull(segundoViaje, "El segundo viaje debería ser permitido después de 5 minutos.");
            //Acelerar el tiempo y realizar un tercer viaje
            tiempoFalso.AgregarMinutos(5); // Simular que han pasado otros 5 minutos
            var tercerViaje = colectivo.PagarCon(medioBoleto); // Tercer viaje, debería ser exitoso
            Assert.IsNotNull(tercerViaje, "El tercer viaje debería ser permitido después de 5 minutos.");
            // Intentar realizar un cuarto viaje
            tiempoFalso.AgregarMinutos(5); // Simular que han pasado otros 5 minutos
            var cuartoViaje = colectivo.PagarCon(medioBoleto); // Cuarto viaje, debería ser exitoso
            Assert.IsNotNull(cuartoViaje, "El cuarto viaje debería ser permitido después de 5 minutos.");
            // Intentar realizar un quinto viaje (debería cobrar tarifa normal)
            var quintoViaje = colectivo.PagarCon(medioBoleto); // Quinto viaje, debería ser permitido pero a tarifa normal
            Assert.IsNotNull(quintoViaje, "El quinto viaje debería ser permitido.");
            Assert.That(quintoViaje.TotalAbonado, Is.EqualTo(colectivo.TarifaBasica), "El quinto viaje debería ser a tarifa normal.");
            // Simular el paso del tiempo
            tiempoFalso.AgregarMinutos(5); // Simular que han pasado otros 5 minutos
            // Intentar realizar un sexto viaje
            var sextoViaje = colectivo.PagarCon(medioBoleto); // Debería ser permitido, pero a tarifa normal
            Assert.IsNotNull(sextoViaje, "El sexto viaje debería ser permitido, pero a tarifa normal.");
            Assert.That(sextoViaje.TotalAbonado, Is.EqualTo(colectivo.TarifaBasica), "El sexto viaje debería ser a tarifa normal.");
        }

        [Test]
        public void NoPermitirMasDeDosViajesGratuitosPorDia()
        {
            // Act
            Boleto primerViaje = colectivo.PagarCon(tarjetaGratuita);
            Boleto segundoViaje = colectivo.PagarCon(tarjetaGratuita);
            Boleto tercerViaje = colectivo.PagarCon(tarjetaGratuita);

            // Assert
            Assert.That(primerViaje, Is.Not.Null, "El primer viaje gratuito debería estar permitido.");
            Assert.That(segundoViaje, Is.Not.Null, "El segundo viaje gratuito debería estar permitido.");
            Assert.That(tercerViaje, Is.Not.Null, "El tercer viaje debería estar permitido, pero a tarifa completa.");
            Assert.That(tercerViaje.TotalAbonado, Is.EqualTo(TarifaCompleta), "El tercer viaje en un mismo día no debería ser gratuito; debería cobrarse la tarifa completa.");

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
            Assert.That(primerViaje, Is.Not.Null, "El primer viaje debería estar permitido como gratuito.");
            Assert.That(segundoViaje, Is.Not.Null, "El segundo viaje debería estar permitido como gratuito.");
            Assert.That(tercerViaje, Is.Not.Null, "El tercer viaje debería permitirse, pero con tarifa completa.");
            Assert.That(tercerViaje.TotalAbonado, Is.EqualTo(TarifaCompleta), "El tercer viaje debería cobrarse a tarifa completa.");
            Assert.That(tarjetaGratuita.Saldo, Is.EqualTo(0), "El saldo restante debería reflejar la tarifa completa cobrada en el tercer viaje.");
        }

    }
}

