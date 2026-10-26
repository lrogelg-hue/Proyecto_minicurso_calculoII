using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MinicursoCalculoII;
using MinicursoCalculoII.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Servicio de interoperabilidad con MathJax (renderizado de fórmulas LaTeX)
builder.Services.AddScoped<MathJaxInterop>();

// Estado compartido del minicurso: datos del estudiante y progreso
// en los ejercicios propuestos (singleton durante la sesión)
builder.Services.AddSingleton<CursoEstado>();

await builder.Build().RunAsync();
