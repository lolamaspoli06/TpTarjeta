using BoletoNamespace;
using ColectivoNamespace;
using ManejoDeTiempos;
using TarjetaNamespace;

namespace ColectivoTest
{
    [TestFixture]
    public class ColectivoTests
    {
        private Tarjeta tarjeta;
        private Colectivo colectivo;
        private Tarjeta interurbana;
        private MedioBoleto medioBoleto;
        private BoletoGratuitoJubilados BoletoJubilados;
        private BoletoGratuito tarjetaGratuita;
        private const decimal TarifaCompleta = 940m;

        [SetUp]
        public void Setup()
        {
            var tiempo = new Tiempo();
            colectivo = new Colectivo("linea 120", tiempo);
            tarjeta = new Tarjeta(2000);
            medioBoleto = new MedioBoleto(4000);
            BoletoJubilados = new BoletoGratuitoJubilados(4000);
            tarjetaGratuita = new BoletoGratuito(TarifaCompleta);
            interurbana = new Tarjeta(10000);
            decimal tarifaInterurbana = interurbana.tarifaInterurbana;
        }

        //Test Iteracion 3: Limitacion Medio Boleto
        public void NoDeberiaPermitirViajeEnMenosDe5MinutosConMedioBoleto()
        {
            // Simulamos el primer viaje
            var primerViaje = colectivo.PagarCon(medioBoleto);
            Assert.IsNotNull(primerViaje, "El primer viaje debería ser permitido.");
            // Intentamos realizar un segundo viaje inmediatamente (sin esperar 1 minuto)
            var segundoViaje = colectivo.PagarCon(medioBoleto);
            Assert.IsNotNull(segundoViaje, "El segundo viaje debería ser permitido, pero con tarifa básica.");
            Assert.That(segundoViaje.TotalAbonado, Is.EqualTo(TarifaCompleta), "El monto del segundo viaje debe ser igual a la tarifa básica.");
            // Avanzar el tiempo en el objeto TiempoFalso para simular que ha pasado 1 minuto
            tiempoFalso.AgregarMinutos(5);
            // Ahora intentamos realizar un tercer viaje después de 1 minuto
            var tercerViaje = colectivo.PagarCon(medioBoleto);
            Assert.IsNotNull(tercerViaje, "El tercer viaje debería ser permitido después de esperar 5 minutos.");
        }
        [Test]

        //Test Iteracion 3: Limitacion Medio Boleto
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

        //Test Iteracion 3: Limitacion Boleto Gratuito
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

        //Test Iteracion 3: Limitacion Boleto Gratuito
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

        //Test Iteracion 3: Limitacion Boleto Gratuito Jubilados
        public void PermitirViajesEnBoletoJubilados()
        {
          
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

            //Test Iteracion 4 franja horaria
            [Test]
            public void NoPermiteViajeConMedioBoletoFueraDeHorario()
            {
                var medioBoleto = new MedioBoleto(500);


                tiempo.AgregarDias(5);
                tiempo.AgregarMinutos(300);

                var boleto = colectivo.PagarCon(medioBoleto);
                Assert.IsNull(boleto, "No se debería permitir un viaje con Medio Boleto fuera del horario permitido.");
            }

            //Test Iteracion 4 franja horaria
            [Test]
            public void NoPermiteViajeConBoletoGratuitoFueraDeHorario()
            {
                var boletoGratuito = new BoletoGratuito(500);


                tiempo.AgregarDias(1);
                tiempo.AgregarMinutos(1380);

                var boleto = colectivo.PagarCon(boletoGratuito);
                Assert.IsNull(boleto, "No se debería permitir un viaje con Boleto Gratuito fuera del horario permitido.");
            }

            //Test Iteracion 4 franja horaria
            [Test]
            public void PermiteViajeConMedioBoletoEnHorarioPermitido()
            {
                var medioBoleto = new MedioBoleto(500);


                tiempo.AgregarDias(2);
                tiempo.AgregarMinutos(600);

                var boleto = colectivo.PagarCon(medioBoleto);
                Assert.IsNotNull(boleto, "Se debería permitir un viaje con Medio Boleto en el horario permitido.");
            }

            //Test Iteracion 4 franja horaria
            [Test]
            public void PermiteViajeConBoletoGratuitoEnHorarioPermitido()
            {
                var boletoGratuito = new BoletoGratuito(500);


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
