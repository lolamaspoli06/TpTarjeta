using System;
using BoletoNamespace;
using ColectivoNamespace;
using TarjetaNamespace;
using static TarjetaNamespace.Tarjeta;



class Program
{
  

    public static void Main()
    {
        Colectivo colectivo = new Colectivo("linea 120");
        Tarjeta tarjeta = new Tarjeta(0);
        bool salir = false;

        while (true)
        {
            Console.WriteLine("Tipo de tarjeta a usar: ");
            Console.WriteLine("1 Normal");
            Console.WriteLine("2 Medio Boleto");
            Console.WriteLine("3 Boleto Gratuito");
            string tipo_tarjeta = Console.ReadLine();

            switch (tipo_tarjeta)
            {
                case "1":
                    tarjeta = new Tarjeta(0);
                    break;
                case "2":
                    tarjeta = new MedioBoleto(0);
                    break;
                case "3":
                    tarjeta = new BoletoGratuito(0);
                    break;

                default:
                    Console.WriteLine("Opcion no valida");
                    break;
            }
            break;

        }
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
                    Console.WriteLine("Ingrese el monto a cargar (debe ser uno de los montos aceptados: \n$2000 $3000 \n$4000 $5000 \n$6000 $7000 \n$8000 $9000");
                    if (int.TryParse(Console.ReadLine(), out int monto) && tarjeta.CargarSaldo(monto))
                    {
                        Console.WriteLine("Carga realizada con éxito.");
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
                        if (tarjeta is BoletoGratuito)
                        {
                            Console.WriteLine($"Viaje pagado con boleto gratuito. Saldo restante: ${boleto.SaldoRestante}");
                        }
                        if (tarjeta is MedioBoleto)
                        {
                            Console.WriteLine($"Viaje pagado con medio boleto. Saldo restante: ${boleto.SaldoRestante}");
                        }
                        else
                        {
                            Console.WriteLine($"Viaje pagado. Saldo restante: ${boleto.SaldoRestante}");
                        }

                    }
                    else
                    {
                        Console.WriteLine("No se pudo realizar el boleto. Verifique el saldo de la tarjeta.");
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
}
