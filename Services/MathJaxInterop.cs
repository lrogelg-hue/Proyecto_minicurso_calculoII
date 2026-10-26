using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MinicursoCalculoII.Services;

/// <summary>
/// Servicio que expone la interoperabilidad con MathJax desde C#.
/// Los componentes lo invocan después de renderizar contenido dinámico
/// (por ejemplo, al desplegar la solución de un ejercicio) para que
/// las expresiones LaTeX vuelvan a procesarse.
/// </summary>
public sealed class MathJaxInterop(IJSRuntime js)
{
    private readonly IJSRuntime _js = js;

    /// <summary>Renderiza las fórmulas de un elemento específico del DOM.</summary>
    public async Task TypesetAsync(ElementReference element)
    {
        try
        {
            await _js.InvokeVoidAsync("minicursoMath.typeset", new object[] { element });
        }
        catch (JSDisconnectedException)
        {
            // El circuito de JS se cerró (navegación/recarga): se ignora.
        }
        catch (TaskCanceledException)
        {
            // Renderizado cancelado por una nueva actualización: se ignora.
        }
    }
}
