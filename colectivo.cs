using System;
using System.Collections.Generic;
using BoletoNamespace;
using TarjetaNamespace;
namespace ColectivoNamespace;

public class Colectivo
{

    public Boleto PagarCon(Tarjeta tarjeta)
    {
        if (tarjeta.DescontarPasaje())
        {
            return new Boleto(tarjeta.Saldo);
        }
        else
        {
            return null;
        }
    }
}
