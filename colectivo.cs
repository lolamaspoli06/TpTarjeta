// Colectivo.cs

using BoletoNamespace;
using ManejoDeTiempos;
using static TarjetaNamespace.Tarjeta;
using TarjetaNamespace;

public class Colectivo
{
    public string Linea { get; private set; }
    private readonly decimal tarifaBasica = 1200;
    private Tiempo tiempo; // Nuevo campo

    public decimal TarifaBasica => tarifaBasica;

    // Constructor actualizado
    public Colectivo(string linea, Tiempo tiempo)
    {
        this.Linea = linea;
        this.tiempo = tiempo;
    }

    public Boleto PagarCon(Tarjeta tarjeta)
    {
        decimal totalAbonado = tarjeta.CalcularTarifa(tarjeta);
        string descripcionExtra = "";

        // Verificar si es una franquicia y si estamos dentro del horario permitido
        if (EsFranquicia(tarjeta) && !EsHorarioPermitido())
        {
            throw new InvalidOperationException("Las franquicias solo pueden usarse de lunes a viernes de 6 a 22 horas.");
        }

        if (tarjeta.SaldoNegativo > 0)
        {
            descripcionExtra = $"Abona saldo {tarjeta.SaldoNegativo}";
        }

        if (tarjeta is MedioBoleto)
        {
            if (tarjeta.ViajesHoy >= 4)
            {
                totalAbonado = tarifaBasica;
            }
            if ((tiempo.Now() - tarjeta.UltimoUso).TotalMinutes < 1)
            {
                totalAbonado = tarifaBasica;
                tarjeta.ViajesHoy -= 1;
            }
        }

        if (tarjeta is BoletoGratuito && tarjeta.ViajesHoy > 2)
        {
            totalAbonado = tarifaBasica;
        }

        if (tarjeta.DescontarPasaje(totalAbonado))
        {
            tarjeta.ActualizarUltimoUso();
            tarjeta.ViajesHoy++;
            return new Boleto(
                tiempo.Now(),
                tarjeta.GetType().Name,
                this.Linea,
                totalAbonado,
                tarjeta.Saldo,
                tarjeta.Id.ToString(),
                descripcionExtra
            );
        }
        return null;
    }

    // Verifica si la tarjeta pertenece a una franquicia
    private bool EsFranquicia(Tarjeta tarjeta)
    {
        return tarjeta is MedioBoleto || tarjeta is BoletoGratuito;
    }

    // Verifica si el horario actual está dentro de la franja permitida
    private bool EsHorarioPermitido()
    {
        DateTime now = tiempo.Now();
        return now.DayOfWeek >= DayOfWeek.Monday && now.DayOfWeek <= DayOfWeek.Friday &&
               now.Hour >= 6 && now.Hour < 22;
    }
}
