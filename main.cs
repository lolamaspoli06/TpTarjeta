using System;

namespace Tp2AAT
{
    class Program
    {
        public static void Main()
        {
            Colectivo colectivo = new Colectivo();
                    Tarjeta tarjeta = new Tarjeta(0);

                    bool salir = false;
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
                                    
                                    Console.WriteLine($"Viaje pago. Saldo restante: ${boleto.SaldoRestante}");
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
}