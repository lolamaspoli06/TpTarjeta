using NUnit.Framework;
using TarjetaNamespace;
using ColectivoNamespace;
using System.Threading;
using static TarjetaNamespace.Tarjeta;

[TestFixture]
public class ColectivoTests
{
    private Colectivo colectivo;
    private MedioBoleto tarjeta;

    [SetUp]
    public void Setup()
    {
        colectivo = new Colectivo("linea 120");
        tarjeta = new MedioBoleto(0);
        tarjeta.CargarSaldo(2000); 
    }

    [Test]
    public void NoDeberiaPermitirViajeDentroDeCincoMinutosConMedioBoleto()
    {

        bool viaje1 = colectivo.PagarCon(tarjeta) != null; 

        bool viaje2 = colectivo.PagarCon(tarjeta) != null; 

        Assert.That(viaje1, Is.True, "El primer viaje debería ser permitido.");
        Assert.That(viaje2, Is.False, "El segundo viaje no debería ser permitido, debe esperar 5 minutos.");

        Thread.Sleep(60000*5); 

        bool viaje3 = colectivo.PagarCon(tarjeta) != null; 

        Assert.That(viaje3, Is.True, "El tercer viaje debería ser permitido después de esperar 5 minutos.");
    }


    [Test]
    public void QuintoViajeConMedioBoletoDeberiaSerTarifaNormal()
    {
        colectivo.PagarCon(tarjeta); 
        Thread.Sleep(60000 *5);
        colectivo.PagarCon(tarjeta);
        Thread.Sleep(60000*5); 
        colectivo.PagarCon(tarjeta);
        Thread.Sleep(60000*5);
        colectivo.PagarCon(tarjeta);
        Thread.Sleep(60000*5);


        decimal saldoAntes = tarjeta.Saldo;


        bool viaje5 = colectivo.PagarCon(tarjeta) != null;


        decimal tarifaNormal = tarjeta.CalcularTarifa(tarjeta);

        Assert.That(viaje5, Is.True, "El quinto viaje debería ser permitido, pero con tarifa normal.");
        Assert.That(tarjeta.Saldo, Is.EqualTo(saldoAntes - tarifaNormal).Within(0.01m), "El saldo debería ajustarse al cobrar la tarifa normal del quinto viaje.");
    }


}
