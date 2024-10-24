using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;
using BoletoNamespace;
using static TarjetaNamespace.Tarjeta;

namespace BoletoTests
{
    public class Tests
    {
        private Colectivo colectivo;
        private Tarjeta tarjetaNormal;
        private Tarjeta tarjetaMedioBoleto;
        private Tarjeta tarjetaBoletoGratuito;

        [SetUp]
        public void Setup()
        {
            colectivo = new Colectivo("Línea 120");
            tarjetaNormal = new Tarjeta(2000);
            tarjetaMedioBoleto = new MedioBoleto(2000);
            tarjetaBoletoGratuito = new BoletoGratuito(2000);
        }

        [Test]
        public void TestBoletoNormal()
        {
            var boleto = colectivo.PagarCon(tarjetaNormal);
            Assert.IsNotNull(boleto);
            Assert.AreEqual("Tarjeta", boleto.TipoTarjeta); 
            Assert.AreEqual(940, boleto.TotalAbonado); 
            Assert.AreEqual(1060, tarjetaNormal.Saldo); 
        }

        [Test]
        public void TestBoletoMedioBoleto()
        {
            var boleto = colectivo.PagarCon(tarjetaMedioBoleto);
            Assert.IsNotNull(boleto);
            Assert.AreEqual("MedioBoleto", boleto.TipoTarjeta); 
            Assert.AreEqual(470, boleto.TotalAbonado); 
            Assert.AreEqual(1530, tarjetaMedioBoleto.Saldo); 
        }

        [Test]
        public void TestBoletoGratuito()
        {
            var boleto = colectivo.PagarCon(tarjetaBoletoGratuito);
            Assert.IsNotNull(boleto);
            Assert.AreEqual("BoletoGratuito", boleto.TipoTarjeta); 
            Assert.AreEqual(0, boleto.TotalAbonado);
            Assert.AreEqual(2000, tarjetaBoletoGratuito.Saldo); 
        }
    }
}
