using System;
using BoletoNamespace;

namespace TarjetaNamespace
{

    public class Tarjeta
    {
        public int saldo;
        public int limite = 9900;
        public int ID = 123;
        public DateTime ultimaUso;

        public bool cargarSaldo(int monto)
        {
            if (monto <= limite && (monto == 2000 || monto == 3000 || monto == 4000 || monto == 5000 || monto == 6000 || monto == 7000 || monto == 8000 || monto == 9000))
            {
                saldo += monto;
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual int precioBoleto(int precio)
        {
            return precio;
        }


        public bool TarjetaUsos(Tarjeta t)
        {

            TimeSpan tiempoDesdeUltimoUso = DateTime.Now - ultimaUso;
            if (t is MedioBoleto)
            {
                if (tiempoDesdeUltimoUso.TotalMinutes >= 5)
                {

                    ultimaUso = DateTime.Now;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            ultimaUso = DateTime.Now;
            return true;
        }

    }
  
  
          public virtual bool DescontarPasaje()
        {
            if (saldo >= tarifaBasica - 480)
            {
                saldo -= tarifaBasica;
                return true;
            }
            return false;
        }

    public class MedioBoleto : Tarjeta
    {
        public override int precioBoleto(int precio)
        {
            return precio / 2;
        }
    }

    public class FranquiciaCompleta : Tarjeta
    {
        public override int precioBoleto(int precio)
        {
            return 0;
        }
    }
}