# Minicurso Cálculo II — *Sólidos en Revolución*

Aplicación web educativa desarrollada con **C# / .NET 10** y **Blazor WebAssembly** para el
curso de **Cálculo II** de la **Universidad Mariano Gálvez de Guatemala (UMG)**.

Incluye todas las secciones obligatorias de la guía del minicurso, fórmulas tipografiadas con
**MathJax (LaTeX)**, diseño responsivo con **Bootstrap 5** y **problemas propuestos
interactivos** cuya validación se realiza íntegramente en C#.

---

## 1. Requisitos

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) o superior
- [Visual Studio Code](https://code.visualstudio.com/) con las extensiones:
  - **C# Dev Kit** (`ms-dotnettools.csdevkit`)
  - **Blazor WASM Debugging** (`ms-dotnettools.blazorwasm-companion`) — solo para depuración
  - Opcional: **Live Server** (no es necesario; el proyecto se sirve con `dotnet run`)

## 2. Cómo ejecutar el proyecto

```bash
# Restaurar dependencias
dotnet restore

# Ejecutar en modo desarrollo (abre en http://localhost:5101)
dotnet run

# Compilar
dotnet build

# Publicar la versión de producción en ./publish
dotnet publish -c Release -o publish
```

En VS Code también puede usar:

- **`Ctrl + Shift + B`** → tarea *build*
- **F5** → *Lanzar MinicursoCalculoII (Blazor WASM)*
- Paleta de comandos → *Tasks: Run Task* → **watch** (recompilación automática)

---

## 3. Estructura de carpetas

```
minicurso_calculoII/
│
├── MinicursoCalculoII.csproj        # Archivo de proyecto .NET (target net10.0)
├── Program.cs                       # Punto de entrada + inyección de dependencias
├── App.razor                        # Enrutador raíz de la aplicación
├── _Imports.razor                   # Usings globales de los componentes
│
├── Layout/                          # Estructura visual de la página
│   ├── MainLayout.razor             # Armazón: barra lateral, barra superior, contenido, pie
│   ├── MainLayout.razor.css         # Estilos del armazón (con ámbito)
│   ├── NavMenu.razor                # Menú lateral con las secciones A–G y anillo de avance
│   └── NavMenu.razor.css            # Estilos del menú (con ámbito)
│
├── Pages/                           # Una página por sección obligatoria
│   ├── Home.razor                   # A. Carátula / presentación (datos del estudiante)
│   ├── Introduccion.razor           # B. Introducción y objetivos de aprendizaje
│   ├── Conceptos.razor              # C. Conceptos, definiciones y propiedades
│   ├── Videos.razor                 # D. Tres videos explicativos (contenedores responsivos)
│   ├── ProblemasResueltos.razor     # E. Problemas resueltos paso a paso (3)
│   ├── ProblemasPropuestos.razor    # F. Problemas propuestos interactivos (6)
│   ├── Fuentes.razor                # G. Fuentes consultadas y créditos
│   └── NotFound.razor               # Página 404
│
├── Components/                      # Componentes reutilizables
│   ├── SectionTitle.razor           # Encabezado uniforme de sección (A, B, C…)
│   ├── BrandMark.razor              # Marca del minicurso (∫ en un disco)
│   ├── SolidIllustration.razor      # Ilustración SVG del sólido de la portada, calculada en C#
│   ├── MathFormula.razor            # Renderiza LaTeX con MathJax (interop C#/JS)
│   └── ExerciseCard.razor           # Tarjeta interactiva de un problema propuesto
│
├── Models/                          # Modelos de dominio y lógica en C#
│   ├── Ejercicio.cs                 # Enumeraciones + validación de respuestas
│   └── ContenidoCurso.cs            # Objetivo, ProblemaResuelto, Video, Fuente, Estudiante
│
├── Services/                        # Lógica de negocio y estado
│   ├── DatosMinicurso.cs            # TODO el contenido académico del minicurso
│   ├── CursoEstado.cs               # Estado compartido: estudiante + progreso
│   └── MathJaxInterop.cs            # Puente C# → MathJax (IJSRuntime)
│
├── Properties/
│   └── launchSettings.json          # Puertos y perfiles de ejecución
│
├── wwwroot/                         # Recursos estáticos servidos al navegador
│   ├── index.html                   # Página anfitriona, tipografías y configuración de MathJax
│   ├── css/
│   │   ├── app.css                  # Estados de la plantilla (validación, errores, carga)
│   │   └── minicurso.css            # Sistema de diseño completo del minicurso
│   ├── js/
│   │   └── mathjax-interop.js       # Funciones JS invocadas desde Blazor
│   ├── lib/bootstrap/               # Bootstrap 5 (local)
│   └── favicon.png / icon-192.png
│
├── .vscode/
│   ├── launch.json                  # Configuración de depuración (F5)
│   └── tasks.json                   # Tareas build / watch / publish
│
├── .gitignore
└── README.md
```

---

## 4. Arquitectura y decisiones de diseño

### 4.1 Separación de responsabilidades

| Capa | Carpeta | Responsabilidad |
|------|---------|-----------------|
| Presentación | `Pages/`, `Components/`, `Layout/` | Interfaz y experiencia de usuario |
| Dominio | `Models/` | Entidades y **reglas de negocio** (validación de respuestas) |
| Servicios | `Services/` | Datos del curso, estado compartido, interoperabilidad JS |

El contenido académico (fórmulas, enunciados, objetivos, bibliografía) está centralizado en
`Services/DatosMinicurso.cs`. **Para editar el minicurso no es necesario modificar los
componentes `.razor`.**

### 4.2 Interactividad con C#

- `Models/Ejercicio.cs` implementa `Validar()`, que resuelve tres tipos de respuesta:
  **numérica** (comparación con tolerancia), **simbólica** (normalización de `π`/`pi`,
  espacios y signos) y **opción múltiple**.
- `Services/CursoEstado.cs` registra el progreso del estudiante, cuenta intentos, calcula el
  porcentaje de avance y notifica a la interfaz mediante un evento `OnCambio`
  (patrón observador). Está registrado como **singleton**, por lo que el avance se comparte
  entre la carátula, el menú lateral y la página de ejercicios.
- `Components/ExerciseCard.razor` presenta cada ejercicio, verifica la respuesta, ofrece
  retroalimentación y **despliega la solución paso a paso**.

### 4.3 Renderizado de fórmulas matemáticas

1. `wwwroot/index.html` configura y carga **MathJax 3** desde CDN.
2. `wwwroot/js/mathjax-interop.js` expone `window.minicursoMath.typeset(...)`.
3. `Services/MathJaxInterop.cs` envuelve esa función para C#.
4. `Components/MathFormula.razor` la invoca en `OnAfterRenderAsync`, de modo que el LaTeX
   se vuelve a tipografiar tras cada actualización dinámica del DOM (por ejemplo, al
   revelar un paso de la solución).

> **Sin conexión a Internet:** descargue MathJax y Bootstrap, guárdelos en `wwwroot/lib/`
> y actualice las referencias de `index.html` para que el material funcione offline.

### 4.4 Diseño responsivo

- Rejilla de Bootstrap 5 (`row`/`col-*`) en todas las secciones.
- Los videos usan la utilidad `.ratio.ratio-16x9`, que mantiene la proporción en cualquier
  tamaño de pantalla.
- En pantallas menores a 992 px la barra lateral se convierte en un panel desplegable
  (menú hamburguesa) con velo de fondo; en escritorio queda fija y visible.
- Las fórmulas largas no desbordan el ancho en móviles
  (`mjx-container[display="true"] { overflow-x: auto; }`).

### 4.5 Sistema visual

La interfaz sigue una identidad de **plano técnico**: azul marino profundo para la estructura,
un único color de acento (cian de instrumento) y papel frío para la lectura matemática.

| Elemento | Decisión | Dónde se define |
|----------|----------|-----------------|
| Colores, sombras, radios y tipografía | Tokens en `:root` | `wwwroot/css/minicurso.css` (sección 1) |
| Titulares y cuerpo | *Source Serif 4* (titulares) + *Inter* (interfaz) | `wwwroot/index.html` + token `--fuente-*` |
| Barra lateral, barra superior y pie | Estilos del armazón | `Layout/MainLayout.razor.css` |
| Menú de secciones y anillo de avance | Estilos del menú | `Layout/NavMenu.razor.css` |
| Piezas compartidas (marca, fórmulas, ejercicios) | Estilos globales | `wwwroot/css/minicurso.css` (secciones 9–23) |
| Ilustración del sólido de la portada | SVG calculado en C# | `Components/SolidIllustration.razor` |

El anillo de avance de la barra lateral se dibuja a partir del porcentaje calculado en
`CursoEstado`, y la ilustración de la portada se genera recorriendo el perfil
`r(t) = R·√t`, que corresponde a la parábola `y = x²` girada alrededor del eje `y`.

> **Accesibilidad.** Hay enlace «Ir al contenido», foco visible en todos los controles,
> etiquetas ARIA en el progreso y en el menú, y las animaciones se desactivan con
> `prefers-reduced-motion`. Los estilos de impresión ocultan la interfaz de navegación para
> entregar el minicurso en PDF.


---

## 5. Cómo personalizar el contenido

| Quiero cambiar… | Archivo |
|-----------------|---------|
| Título, institución, curso, ciclo | `Services/DatosMinicurso.cs` (constantes iniciales) |
| Objetivos de aprendizaje | `DatosMinicurso.Objetivos` |
| Enlaces de los videos | `DatosMinicurso.Videos` (`VIDEO_ID_1`, `VIDEO_ID_2`, `VIDEO_ID_3`) |
| Problemas resueltos | `DatosMinicurso.ProblemasResueltos` |
| Problemas propuestos y respuestas | `DatosMinicurso.Ejercicios` |
| Bibliografía | `DatosMinicurso.Fuentes` |
| Colores, sombras, radios y tipografía | `wwwroot/css/minicurso.css` → bloque `:root` |
| Aspecto de la barra lateral | `Layout/NavMenu.razor.css` |
| Barra superior y pie de página | `Layout/MainLayout.razor.css` |
| Fuentes tipográficas | `wwwroot/index.html` (Google Fonts) |

---

## 6. Correspondencia con la guía oficial

| Sección | Requisito | Dónde se cumple |
|---------|-----------|-----------------|
| **A** | Carátula: título, tema, datos del estudiante, institución, curso, fecha | `Pages/Home.razor` |
| **B** | Introducción e importancia + 2 a 4 objetivos | `Pages/Introduccion.razor` (4 objetivos) |
| **C** | Conceptos, definiciones y propiedades con fórmulas | `Pages/Conceptos.razor` |
| **D** | Tres videos con contenedores adaptados | `Pages/Videos.razor` |
| **E** | Al menos 3 problemas resueltos paso a paso y comprobación | `Pages/ProblemasResueltos.razor` (3) |
| **F** | Al menos 5 problemas propuestos por dificultad e interactivos | `Pages/ProblemasPropuestos.razor` (6) |
| **G** | Fuentes consultadas y créditos | `Pages/Fuentes.razor` |

---

## 7. Licencia y uso

Material elaborado con fines **académicos y didácticos** para el curso de Cálculo II de la
Universidad Mariano Gálvez de Guatemala. Las bibliotecas de terceros (Bootstrap, MathJax)
conservan sus respectivas licencias.
