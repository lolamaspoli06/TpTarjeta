public class Tarjeta
{
    protected decimal tarifaBasica = 940;  // Asegúrate de usar nombres consistentes en minúsculas
    protected decimal saldoNegativoPermitido = 480;

    protected decimal saldo;
    protected decimal limiteSaldo = 9900;

    public Tarjeta(decimal saldoInicial)
    {
        saldo = saldoInicial;
    }

    public decimal Saldo
    {
        get { return saldo; }
    }

    public bool CargarSaldo(decimal monto)
    {
        decimal nuevoSaldo = saldo + monto;
        if (nuevoSaldo > limiteSaldo)
        {
            return false;
        }
        saldo = nuevoSaldo;
        return true;
    }

    public virtual bool DescontarPasaje()
    {
        if (saldo >= tarifaBasica || (saldo + saldoNegativoPermitido >= tarifaBasica))
        {
            saldo -= tarifaBasica;
            return true;
        }
        return false;
    }
}
