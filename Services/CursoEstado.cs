using MinicursoCalculoII.Models;

namespace MinicursoCalculoII.Services;

/// <summary>
/// Estado compartido del minicurso durante la sesión del estudiante:
/// datos de la carátula y progreso en los ejercicios propuestos.
/// Se registra como singleton, por lo que todos los componentes
/// comparten la misma información (gestión de estado con C#).
/// </summary>
public sealed class CursoEstado
{
    /// <summary>Datos capturados en la carátula.</summary>
    public DatosEstudiante Estudiante { get; } = new();

    private readonly Dictionary<int, ResultadoEjercicio> _resultados = new();

    /// <summary>Se dispara cuando cambia el progreso, para refrescar la interfaz.</summary>
    public event Action? OnCambio;

    /// <summary>Total de ejercicios propuestos del minicurso.</summary>
    public int TotalEjercicios { get; set; } = DatosMinicurso.Ejercicios.Count;

    public int EjerciciosResueltos => _resultados.Values.Count(r => r.Correcto);

    public int EjerciciosIntentados => _resultados.Count;

    public double PorcentajeAvance =>
        TotalEjercicios == 0 ? 0 : Math.Round(100.0 * EjerciciosResueltos / TotalEjercicios, 1);

    public ResultadoEjercicio? Obtener(int numero) =>
        _resultados.TryGetValue(numero, out var resultado) ? resultado : null;

    /// <summary>
    /// Valida una respuesta, guarda el resultado y notifica a los suscriptores.
    /// </summary>
    public ResultadoEjercicio Responder(Ejercicio ejercicio, string? respuesta)
    {
        var esCorrecta = ejercicio.Validar(respuesta);
        var resultado = new ResultadoEjercicio
        {
            Numero = ejercicio.Numero,
            Respuesta = respuesta ?? string.Empty,
            Correcto = esCorrecta,
            Intentos = (_resultados.TryGetValue(ejercicio.Numero, out var previo) ? previo.Intentos : 0) + 1,
            SolucionVisible = _resultados.TryGetValue(ejercicio.Numero, out var p) && p.SolucionVisible
        };

        _resultados[ejercicio.Numero] = resultado;
        OnCambio?.Invoke();
        return resultado;
    }

    /// <summary>Marca la solución de un ejercicio como visible.</summary>
    public void MostrarSolucion(int numeroEjercicio)
    {
        if (!_resultados.TryGetValue(numeroEjercicio, out var resultado))
        {
            resultado = new ResultadoEjercicio { Numero = numeroEjercicio };
            _resultados[numeroEjercicio] = resultado;
        }

        resultado.SolucionVisible = true;
        OnCambio?.Invoke();
    }

    /// <summary>Reinicia el progreso (nuevo intento del estudiante).</summary>
    public void Reiniciar()
    {
        _resultados.Clear();
        OnCambio?.Invoke();
    }
}

/// <summary>Resultado individual de un ejercicio propuesto.</summary>
public sealed class ResultadoEjercicio
{
    public int Numero { get; init; }
    public string Respuesta { get; set; } = string.Empty;
    public bool Correcto { get; set; }
    public int Intentos { get; set; }
    public bool SolucionVisible { get; set; }
}
