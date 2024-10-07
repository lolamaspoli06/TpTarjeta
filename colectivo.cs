using System;
using System.Collections.Generic;

namespace Tp2AAT
{
    public class Colectivo
    {
    
        public Boleto PagarCon(Tarjeta tarjeta)
        {
            {
                if (tarjeta.DescontarPasaje()){
                    return new Boleto(tarjeta.Saldo);
                }
                else{
                    return null;
                }
            }
            }
        }
    }

