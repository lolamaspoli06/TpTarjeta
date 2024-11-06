using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManejoDeTiempos
{
    public class TiempoFalso : Tiempo
    {
        private DateTime tiempo;

        public TiempoFalso()
        {
            tiempo = new DateTime(2024, 10, 14);
        }

        public override DateTime Now()
        {
            return tiempo;
        }

        public void AgregarDias(int cantidad)
        {
            tiempo = tiempo.AddDays(cantidad);
        }

        public void AgregarMinutos(int cantidad)
        {
            tiempo = tiempo.AddMinutes(cantidad);
        }
    }
}