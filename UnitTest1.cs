using NUnit.Framework;
using TarjetaNamespace;
[TestFixture]
public class TarjetaTests
{
    [Test]
    public void CargarSaldo_SuperaLimite_SaldoPendienteAlmacenado()
    {
        
        var tarjeta = new Tarjeta(35000); 
        decimal montoACargar = 5000; 

        bool resultadoCarga = tarjeta.CargarSaldo(montoACargar);

        Assert.That(resultadoCarga, Is.True, "La carga deberia ser exitosa.");
        Assert.That(tarjeta.Saldo, Is.EqualTo(36000), $"El saldo deberia estar en el m�ximo permitido, pero fue {tarjeta.Saldo}.");
        Assert.That(tarjeta.SaldoPendiente, Is.EqualTo(4000), $"El saldo pendiente deberia ser 4000, pero fue {tarjeta.SaldoPendiente}.");
    }
    [Test]
    public void RealizarViaje_AcreditaSaldoPendiente()
    {
       
        var tarjeta = new Tarjeta(36000); 
        tarjeta.CargarSaldo(5000); 

        var colectivo = new ColectivoNamespace.Colectivo("L�nea 120");
        decimal saldoAntesDeViaje = tarjeta.Saldo;

        var boleto = colectivo.PagarCon(tarjeta); // Realiza el viaje


        // Assert
        Assert.That(boleto, Is.Not.Null, "El boleto deber�a ser creado despu�s del viaje.");
        Assert.That(tarjeta.Saldo, Is.EqualTo(36000), "El saldo deber�a reacreditar hasta el m�ximo.");
        Assert.That(tarjeta.SaldoPendiente, Is.EqualTo(4060), "El saldo pendiente deber�a reflejar la diferencia despu�s del viaje que es 4060.");
    }




}


