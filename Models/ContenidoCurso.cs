namespace MinicursoCalculoII.Models;

/// <summary>Objetivo de aprendizaje del minicurso (sección B).</summary>
public sealed record ObjetivoAprendizaje(int Codigo, string Descripcion);

/// <summary>
/// Problema resuelto por escrito (sección E): enunciado, pasos con justificación
/// matemática y comprobación final.
/// </summary>
public sealed record ProblemaResuelto(
    int Numero,
    string Titulo,
    string Enunciado,
    string DificultadTexto,
    IReadOnlyList<PasoResuelto> Pasos,
    string Comprobacion);

/// <summary>Paso individual de un problema resuelto.</summary>
public sealed record PasoResuelto(string Justificacion, string Desarrollo);

/// <summary>Video explicativo breve (sección D, máximo 1 minuto).</summary>
public sealed record VideoExplicativo(
    int Numero,
    string Titulo,
    string Descripcion,
    string UrlEmbebido);

/// <summary>Entrada de bibliografía o recurso consultado (sección G).</summary>
public sealed record Fuente(
    string Autores,
    string Anio,
    string Titulo,
    string Editorial,
    string ReferenciaUrl,
    string? Tipo = "Libro");

/// <summary>Datos del estudiante capturados en la carátula (sección A).</summary>
public sealed class DatosEstudiante
{
    public string Nombre { get; set; } = string.Empty;
    public string Carnet { get; set; } = string.Empty;
    public string Carrera { get; set; } = string.Empty;
    public string Ciclo { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.Now;

    public bool EstaCompleto =>
        !string.IsNullOrWhiteSpace(Nombre) &&
        !string.IsNullOrWhiteSpace(Carnet);

    public string FechaFormateada => Fecha.ToString("dd 'de' MMMM 'de' yyyy",
        new System.Globalization.CultureInfo("es-GT"));
}
