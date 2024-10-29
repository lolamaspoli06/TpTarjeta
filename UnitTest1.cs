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
    public void NoDeberiaPermitirMedioBoletoDentroDeCincoMinutosConMedioBoleto()
    {

        bool viaje1 = colectivo.PagarCon(tarjeta) != null;

        bool viaje2 = colectivo.PagarCon(tarjeta) != null;

        decimal tarifaNormal = tarjeta.CalcularTarifa(tarjeta);
        decimal saldoAntes = tarjeta.Saldo;

        Assert.That(viaje1, Is.True, "El primer viaje deberia ser permitido.");
        Assert.That(viaje2, Is.True, "El segundo viaje deberia ser permitido, pero con tarifa normal.");
        Assert.That(tarjeta.Saldo, Is.EqualTo(saldoAntes - tarifaNormal).Within(0.01m), "El saldo deberoa ajustarse al cobrar la tarifa normal del segundo viaje.");
    
        Thread.Sleep(60000*1);

        bool viaje3 = colectivo.PagarCon(tarjeta) != null;

        Assert.That(viaje3, Is.True, "El tercer viaje deberia ser permitido despues de esperar 5 minutos.");
    }


    [Test]
    public void QuintoViajeConMedioBoletoDeberiaSerTarifaNormal()
    {
        colectivo.PagarCon(tarjeta);
        Thread.Sleep(60000 *1);
        colectivo.PagarCon(tarjeta);
        Thread.Sleep(60000*1);
        colectivo.PagarCon(tarjeta);
        Thread.Sleep(60000*1);
        colectivo.PagarCon(tarjeta);
        Thread.Sleep(60000*1);


        decimal saldoAntes = tarjeta.Saldo;


        bool viaje5 = colectivo.PagarCon(tarjeta) != null;


        decimal tarifaNormal = tarjeta.CalcularTarifa(tarjeta);

        Assert.That(viaje5, Is.True, "El quinto viaje deberia ser permitido, pero con tarifa normal.");
        Assert.That(tarjeta.Saldo, Is.EqualTo(saldoAntes - tarifaNormal).Within(0.01m), "El saldo deberoa ajustarse al cobrar la tarifa normal del quinto viaje.");
    }


}