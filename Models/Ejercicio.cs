namespace MinicursoCalculoII.Models;

/// <summary>Nivel de dificultad de un ejercicio propuesto.</summary>
public enum Dificultad
{
    Basico,
    Intermedio,
    Avanzado
}

/// <summary>Tipo de respuesta que se espera del estudiante.</summary>
public enum TipoRespuesta
{
    /// <summary>Valor numérico comparado con tolerancia (ej. 28.27).</summary>
    Numerica,

    /// <summary>Expresión simbólica normalizada (ej. "9pi", "9π", "9*3.1416").</summary>
    Formula,

    /// <summary>Selección de una opción (A, B, C o D).</summary>
    OpcionMultiple
}

/// <summary>
/// Ejercicio propuesto del minicurso. Concentra tanto los datos de presentación
/// (enunciado en LaTeX, dificultad) como la lógica de validación de respuestas,
/// demostrando el uso de C# para la interactividad educativa.
/// </summary>
public sealed class Ejercicio
{
    public required int Numero { get; init; }
    public required string Enunciado { get; init; }
    public Dificultad Dificultad { get; init; } = Dificultad.Basico;
    public TipoRespuesta TipoRespuesta { get; init; } = TipoRespuesta.Numerica;

    /// <summary>Respuesta correcta en formato numérico o simbólico.</summary>
    public required string RespuestaCorrecta { get; init; }

    /// <summary>Tolerancia absoluta para respuestas numéricas (por defecto 0.01).</summary>
    public double Tolerancia { get; init; } = 0.01;

    /// <summary>Opciones para preguntas de opción múltiple, en orden A, B, C, D.</summary>
    public IReadOnlyList<string>? Opciones { get; init; }

    /// <summary>Pasos de la solución, en orden de aparición.</summary>
    public required IReadOnlyList<string> Solucion { get; init; }

    /// <summary>Comprobación final del resultado (verificación del problema).</summary>
    public required string Comprobacion { get; init; }

    /// <summary>
    /// Valida la respuesta del estudiante según el tipo de ejercicio.
    /// </summary>
    /// <param name="respuestaUsuario">Texto ingresado o seleccionado.</param>
    /// <returns>true si la respuesta es correcta dentro de la tolerancia admitida.</returns>
    public bool Validar(string? respuestaUsuario)
    {
        if (string.IsNullOrWhiteSpace(respuestaUsuario))
            return false;

        return TipoRespuesta switch
        {
            TipoRespuesta.Numerica => ValidarNumerica(respuestaUsuario),
            TipoRespuesta.Formula => ValidarFormula(respuestaUsuario),
            TipoRespuesta.OpcionMultiple => ValidarOpcionMultiple(respuestaUsuario),
            _ => false
        };
    }

    /// <summary>Devuelve un mensaje de orientación (no la solución) si la respuesta es incorrecta.</summary>
    public string ObtenerRetroalimentacion(string? respuestaUsuario)
    {
        if (string.IsNullOrWhiteSpace(respuestaUsuario))
            return "Ingresa una respuesta antes de verificar.";

        return TipoRespuesta switch
        {
            TipoRespuesta.Numerica =>
                "Revisa los límites de integración y la fórmula aplicada. Recuerda que π ≈ 3.141592.",
            TipoRespuesta.Formula =>
                "Verifica la expresión. Puedes escribir 'pi' o 'π' para el número π. Simplifica tanto como sea posible.",
            TipoRespuesta.OpcionMultiple =>
                "Revisa tu selección. Analiza de nuevo las condiciones del problema.",
            _ => "Intenta nuevamente."
        };
    }

    private bool ValidarNumerica(string entrada)
    {
        var texto = entrada.Replace(',', '.').Trim();
        if (!double.TryParse(texto,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out var valor))
            return false;

        return Math.Abs(valor - double.Parse(RespuestaCorrecta,
            System.Globalization.CultureInfo.InvariantCulture)) <= Tolerancia;
    }

    private bool ValidarFormula(string entrada)
    {
        var normalizada = Normalizar(entrada);
        var esperada = Normalizar(RespuestaCorrecta);

        // Se admiten varias formas equivalentes de escribir la misma respuesta
        return RespuestaCorrecta.Split('|', StringSplitOptions.RemoveEmptyEntries)
                .Select(Normalizar)
                .Contains(normalizada)
            || normalizada == esperada;
    }

    private bool ValidarOpcionMultiple(string entrada)
    {
        var limpia = entrada.Trim().TrimEnd('.').ToUpperInvariant();
        return limpia == RespuestaCorrecta.Trim().ToUpperInvariant();
    }

    /// <summary>Normaliza una expresión: minúsculas, sin espacios y con π como 'pi'.</summary>
    private static string Normalizar(string texto) =>
        texto.Trim().ToLowerInvariant()
             .Replace(" ", "")
             .Replace("π", "pi")
             .Replace("*", "")
             .Replace("·", "");
}
