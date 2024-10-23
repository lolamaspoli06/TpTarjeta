using System;

namespace Tp2AAT
{
    class Program
    {
        public static void Main(string[] args) // Asegúrate de tener el parámetro args
        {
            Colectivo colectivo = new Colectivo("Línea 120"); 

            Tarjeta tarjeta = new Tarjeta(0, null); 
            bool salir = false;
            tarjeta = inicio(tarjeta);

            while (!salir)
            {
                Console.WriteLine("Elija una opción:");
                Console.WriteLine("1. Consultar saldo de tarjeta");
                Console.WriteLine("2. Cargar saldo en tarjeta");
                Console.WriteLine("3. Pagar boleto de colectivo");
                Console.WriteLine("4. Salir");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        Console.WriteLine($"Saldo actual de la tarjeta: ${tarjeta.Saldo}");
                        break;

                    case "2":
                        Console.WriteLine("Ingrese el monto a cargar: $2000, $3000, $4000, $5000, $6000, $7000, $8000, $9000");
                        string input = Console.ReadLine();
                        if (!string.IsNullOrEmpty(input) && decimal.TryParse(input, out decimal monto))
                        {
                             List<decimal> montosValidos = new List<decimal> { 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000 };
                             if (montosValidos.Contains(monto)){
                            tarjeta.CargarSaldo(monto);
                            Console.WriteLine("Carga realizada con éxito.");
                             } else {
                                Console.WriteLine ("Monto inválido");
                             }
                        }
                        else
                        {
                            Console.WriteLine("Monto inválido o límite de saldo alcanzado.");
                        }
                        break;

                    case "3":
                        Boleto boleto = colectivo.PagarCon(tarjeta);
                        if (boleto != null)
                        {
                            boleto.MostrarInformacion();
                        }
                        else
                        {
                            Console.WriteLine("No se pudo realizar el pago. Verifique el saldo de la tarjeta.");
                        }
                        break;

                    case "4":
                        salir = true;
                        Console.WriteLine("Saliendo del programa...");
                        break;

                    default:
                        Console.WriteLine("Opción inválida. Por favor, intente nuevamente.");
                        break;
                }
            }
        }

        // Debes mantener el método inicio aquí también
        static Tarjeta inicio(Tarjeta tarjeta)
        {
            Console.WriteLine("¿Tiene franquicia de medio boleto? (s/n)");
            string tieneFranquicia = Console.ReadLine();
            if (tieneFranquicia.ToLower() == "s")
            {
                tarjeta = new MedioBoleto(tarjeta.Saldo);
            }
            return tarjeta;
        }
    }
}
