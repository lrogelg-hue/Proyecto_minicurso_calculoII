using MinicursoCalculoII.Models;

namespace MinicursoCalculoII.Services;

/// <summary>
/// Repositorio central del contenido académico del minicurso.
/// Mantener los datos separados de la interfaz permite modificar textos,
/// fórmulas y ejercicios sin tocar los componentes .razor.
/// </summary>
public static class DatosMinicurso
{
    // ----------------------------------------------------------------------
    // Metadatos generales (sección A: carátula)
    // ----------------------------------------------------------------------
    public const string Titulo = "Sólidos en Revolución";
    public const string Subtitulo = "Volúmenes y áreas de superficies de revolución";
    public const string Institucion = "Universidad Mariano Gálvez de Guatemala";
    public const string Facultad = "Facultad de Ingeniería en Sistemas de Información";
    public const string Curso = "Cálculo II";
    public const string Docente = "Docente: ______________________";
    public const string Ciclo = "Ciclo 2026";

    // ----------------------------------------------------------------------
    // Sección B: objetivos de aprendizaje
    // ----------------------------------------------------------------------
    public static readonly IReadOnlyList<ObjetivoAprendizaje> Objetivos =
    [
        new(1, "Interpretar geométricamente el sólido de revolución que se genera al girar una región plana alrededor de un eje coordenado."),
        new(2, "Aplicar el método de los discos y de las arandelas para calcular volúmenes de sólidos de revolución mediante integrales definidas."),
        new(3, "Emplear el método de las capas o cascarones cilíndricos como alternativa cuando la integración respecto a una variable resulta más sencilla."),
        new(4, "Calcular el área de una superficie de revolución y verificar numéricamente los resultados obtenidos.")
    ];

    // ----------------------------------------------------------------------
    // Sección D: tres videos explicativos (máximo 1 minuto cada uno)
    // Reemplace los identificadores por los de sus propios videos.
    // ----------------------------------------------------------------------
    public static readonly IReadOnlyList<VideoExplicativo> Videos =
    [
        new(1, "Idea intuitiva del sólido de revolución",
            "Cómo una región plana genera un cuerpo en el espacio al girar alrededor de un eje.",
            "https://www.youtube.com/embed/VIDEO_ID_1"),
        new(2, "Método de los discos y arandelas",
            "Fórmulas, elección del radio y planteamiento de la integral en un minuto.",
            "https://www.youtube.com/embed/VIDEO_ID_2"),
        new(3, "Método de los cascarones cilíndricos",
            "Cuándo conviene usarlo y cómo se construye el diferencial de volumen.",
            "https://www.youtube.com/embed/VIDEO_ID_3")
    ];

    // ----------------------------------------------------------------------
    // Sección E: problemas resueltos paso a paso
    // ----------------------------------------------------------------------
    public static readonly IReadOnlyList<ProblemaResuelto> ProblemasResueltos =
    [
        new ProblemaResuelto(
            1,
            "Volumen por el método de los discos",
            """Calcule el volumen del sólido generado al girar la región limitada por \(y=\sqrt{x}\), el eje \(x\) y las rectas \(x=0\) y \(x=4\), alrededor del eje \(x\).""",
            "Básico",
            [
                new PasoResuelto(
                    "Se identifica el sólido y el eje de giro.",
                    """Al girar la región alrededor del eje \(x\), cada corte perpendicular al eje es un disco de radio \(R(x)=f(x)=\sqrt{x}\)."""),
                new PasoResuelto(
                    "Se aplica la fórmula del método de los discos.",
                    """\[V=\pi\int_{a}^{b}\left[f(x)\right]^{2}dx=\pi\int_{0}^{4}\left(\sqrt{x}\right)^{2}dx\]"""),
                new PasoResuelto(
                    "Se simplifica el integrando y se integra.",
                    """\[V=\pi\int_{0}^{4}x\,dx=\pi\left[\frac{x^{2}}{2}\right]_{0}^{4}=\pi\left(\frac{16}{2}\right)=8\pi\]"""),
                new PasoResuelto(
                    "Se expresa el resultado.",
                    """\[V=8\pi\ \text{unidades cúbicas}\approx 25.13\ \text{u}^{3}\]""")
            ],
            """Derivando la comprobación: el sólido es un cono con radio \(R=2\) y altura \(h=4\). Con la fórmula conocida \(V=\tfrac{1}{3}\pi R^{2}h=\tfrac{1}{3}\pi(4)(4)=\tfrac{16\pi}{3}\approx 16.76\). La diferencia se debe a que la generatriz \(y=\sqrt{x}\) NO es una recta: la superficie real es un paraboloide, por lo que el volumen \(8\pi\approx 25.13\) es mayor, consistente con la curvatura hacia fuera."""),

        new ProblemaResuelto(
            2,
            "Volumen por el método de las arandelas",
            """Halle el volumen del sólido obtenido al girar alrededor del eje \(x\) la región acotada por las curvas \(y=x\) y \(y=x^{2}\).""",
            "Intermedio",
            [
                new PasoResuelto(
                    "Se determinan los puntos de intersección.",
                    """Igualando \(x=x^{2}\) se obtiene \(x(x-1)=0\), es decir, \(x=0\) y \(x=1\). Estos son los límites de integración."""),
                new PasoResuelto(
                    "Se identifican los radios.",
                    """En \(0\le x\le 1\) se cumple \(x\ge x^{2}\), por lo que el radio externo es \(R(x)=x\) y el radio interno es \(r(x)=x^{2}\)."""),
                new PasoResuelto(
                    "Se plantea la integral de arandelas.",
                    """\[V=\pi\int_{0}^{1}\left(\left[R(x)\right]^{2}-\left[r(x)\right]^{2}\right)dx=\pi\int_{0}^{1}\left(x^{2}-x^{4}\right)dx\]"""),
                new PasoResuelto(
                    "Se integra y se evalúa.",
                    """\[V=\pi\left[\frac{x^{3}}{3}-\frac{x^{5}}{5}\right]_{0}^{1}=\pi\left(\frac{1}{3}-\frac{1}{5}\right)=\pi\left(\frac{5-3}{15}\right)=\frac{2\pi}{15}\]"""),
                new PasoResuelto(
                    "Se expresa el resultado final.",
                    """\[V=\frac{2\pi}{15}\approx 0.4189\ \text{u}^{3}\]""")
            ],
            """Verificación numérica: \(\pi\left(\tfrac{1}{3}-\tfrac{1}{5}\right)=\pi(0.3333-0.2)=0.1333\pi\approx 0.4189\). El resultado es positivo y menor que el volumen del cono externo \(\left(\tfrac{\pi}{3}\approx 1.047\right)\), lo cual es consistente porque se ha restado el hueco interior."""),

        new ProblemaResuelto(
            3,
            "Volumen por el método de los cascarones cilíndricos",
            """Calcule el volumen del sólido que se genera al girar alrededor del eje \(y\) la región limitada por \(y=x^{2}\), el eje \(x\) y la recta \(x=2\).""",
            "Avanzado",
            [
                new PasoResuelto(
                    "Se elige el método adecuado.",
                    """Como la región gira alrededor del eje \(y\) y la función está despejada como \(y=x^{2}\), es más directo integrar respecto a \(x\) usando cascarones cilíndricos."""),
                new PasoResuelto(
                    "Se describe el cascarón típico.",
                    """Un cascarón a distancia \(x\) del eje tiene radio \(x\) y altura \(h(x)=x^{2}\), con espesor \(dx\). Su volumen es \(dV=2\pi\,x\,h(x)\,dx\)."""),
                new PasoResuelto(
                    "Se plantea la integral.",
                    """\[V=2\pi\int_{0}^{2}x\cdot x^{2}\,dx=2\pi\int_{0}^{2}x^{3}\,dx\]"""),
                new PasoResuelto(
                    "Se integra y se evalúa.",
                    """\[V=2\pi\left[\frac{x^{4}}{4}\right]_{0}^{2}=2\pi\left(\frac{16}{4}\right)=2\pi(4)=8\pi\]"""),
                new PasoResuelto(
                    "Se expresa el resultado.",
                    """\[V=8\pi\approx 25.13\ \text{u}^{3}\]""")
            ],
            """Comprobación por el método de los discos respecto a \(y\): despejando \(x=\sqrt{y}\) para \(0\le y\le 4\), resulta \(V=\pi\int_{0}^{4}\left(\sqrt{y}\right)^{2}dy=\pi\int_{0}^{4}y\,dy=8\pi\). Ambos métodos coinciden, lo que confirma el resultado.""")
    ];

    // ----------------------------------------------------------------------
    // Sección F: problemas propuestos (organizados por dificultad)
    // La respuesta correcta y los pasos son validados y desplegados por C#.
    // ----------------------------------------------------------------------
    public static readonly IReadOnlyList<Ejercicio> Ejercicios =
    [
        new Ejercicio
        {
            Numero = 1,
            Dificultad = Dificultad.Basico,
            TipoRespuesta = TipoRespuesta.Numerica,
            Enunciado = """Gire la región limitada por \(y=2x\), el eje \(x\) y la recta \(x=3\) alrededor del eje \(x\). Calcule el volumen (use \(\pi\approx3.141592\) y redondee a dos decimales).""",
            RespuestaCorrecta = "113.10",
            Tolerancia = 0.1,
            Solucion =
            [
                """La región gira alrededor del eje \(x\); el radio es \(R(x)=2x\).""",
                """\[V=\pi\int_{0}^{3}(2x)^{2}dx=4\pi\int_{0}^{3}x^{2}dx=4\pi\left[\frac{x^{3}}{3}\right]_{0}^{3}\]""",
                """\[V=4\pi(9)=36\pi\approx 113.10\ \text{u}^{3}\]"""
            ],
            Comprobacion = """El sólido es un cono de radio \(R=6\) y altura \(h=3\): \(V=\tfrac{1}{3}\pi(36)(3)=36\pi\approx113.10\), lo que coincide con la integral."""
        },

        new Ejercicio
        {
            Numero = 2,
            Dificultad = Dificultad.Basico,
            TipoRespuesta = TipoRespuesta.Formula,
            Enunciado = """Determine el volumen del sólido generado al girar \(y=x^{2}\) alrededor del eje \(x\) en el intervalo \([0,3]\). Escriba el resultado exacto en función de \(\pi\) (por ejemplo: 5pi o 12pi/7).""",
            RespuestaCorrecta = "48.6pi|243pi/5",
            Solucion =
            [
                """Aplicando discos con \(R(x)=x^{2}\):""",
                """\[V=\pi\int_{0}^{3}x^{4}dx=\pi\left[\frac{x^{5}}{5}\right]_{0}^{3}=\frac{243\pi}{5}\]""",
                """\[\frac{243}{5}=48.6\quad\Rightarrow\quad V=48.6\pi\approx152.68\ \text{u}^{3}\]"""
            ],
            Comprobacion = """Verificación: \(\tfrac{243\pi}{5}=48.6\pi\approx152.68\). Se puede escribir también como \(48.6\pi\), \(243\pi/5\) o \(\tfrac{243}{5}\pi\); todas son equivalentes."""
        },

        new Ejercicio
        {
            Numero = 3,
            Dificultad = Dificultad.Intermedio,
            TipoRespuesta = TipoRespuesta.Numerica,
            Enunciado = """Halle el volumen del sólido que resulta al girar alrededor del eje \(x\) la región entre \(y=x\) y \(y=x^{2}\) (redondee a dos decimales).""",
            RespuestaCorrecta = "0.42",
            Tolerancia = 0.01,
            Solucion =
            [
                """Intersecciones: \(x=x^{2}\Rightarrow x=0,\ x=1\).""",
                """Método de arandelas con \(R(x)=x\) y \(r(x)=x^{2}\):""",
                """\[V=\pi\int_{0}^{1}(x^{2}-x^{4})dx=\pi\left[\frac{x^{3}}{3}-\frac{x^{5}}{5}\right]_{0}^{1}=\frac{2\pi}{15}\]""",
                """\[V=\frac{2\pi}{15}\approx0.42\ \text{u}^{3}\]"""
            ],
            Comprobacion = """El valor \(\tfrac{2\pi}{15}\approx0.4189\) redondeado a dos decimales es \(0.42\); es menor que el volumen del cono de radio 1 y altura 1 (\(\tfrac{\pi}{3}\approx1.05\)), como corresponde a un sólido con perforación interior."""
        },

        new Ejercicio
        {
            Numero = 4,
            Dificultad = Dificultad.Intermedio,
            TipoRespuesta = TipoRespuesta.Formula,
            Enunciado = """Usando cascarones cilíndricos, calcule el volumen al girar la región limitada por \(y=x^{2}\), el eje \(x\) y la recta \(x=2\) alrededor del eje \(y\). Escriba el resultado exacto con \(\pi\).""",
            RespuestaCorrecta = "8pi",
            Solucion =
            [
                """Radio del cascarón \(=x\), altura \(h(x)=x^{2}\).""",
                """\[V=2\pi\int_{0}^{2}x\cdot x^{2}dx=2\pi\int_{0}^{2}x^{3}dx\]""",
                """\[V=2\pi\left[\frac{x^{4}}{4}\right]_{0}^{2}=2\pi(4)=8\pi\]"""
            ],
            Comprobacion = """Por discos respecto a \(y\): \(x=\sqrt{y}\) con \(0\le y\le4\) da \(V=\pi\int_{0}^{4}y\,dy=8\pi\). Ambos métodos coinciden."""
        },

        new Ejercicio
        {
            Numero = 5,
            Dificultad = Dificultad.Avanzado,
            TipoRespuesta = TipoRespuesta.Numerica,
            Enunciado = """La región limitada por \(y=x^{2}\), el eje \(x\) y \(x=2\) gira alrededor de la recta \(y=-1\). Calcule el volumen (dos decimales).""",
            RespuestaCorrecta = "36.86",
            Tolerancia = 0.05,
            Solucion =
            [
                """Al girar alrededor de \(y=-1\), los radios se desplazan una unidad: \(R(x)=x^{2}+1\) y \(r(x)=1\).""",
                """\[V=\pi\int_{0}^{2}\left[(x^{2}+1)^{2}-1^{2}\right]dx=\pi\int_{0}^{2}\left(x^{4}+2x^{2}\right)dx\]""",
                """\[V=\pi\left[\frac{x^{5}}{5}+\frac{2x^{3}}{3}\right]_{0}^{2}=\pi\left(\frac{32}{5}+\frac{16}{3}\right)=\frac{176\pi}{15}\]""",
                """\[V=\frac{176\pi}{15}\approx36.86\ \text{u}^{3}\]"""
            ],
            Comprobacion = """\(\tfrac{176\pi}{15}=11.7333\pi\approx36.86\). Observe que es mayor que \(8\pi\) (giro sobre el eje \(x\)) porque el eje de giro se desplazó, aumentando los radios efectivos."""
        },

        new Ejercicio
        {
            Numero = 6,
            Dificultad = Dificultad.Avanzado,
            TipoRespuesta = TipoRespuesta.OpcionMultiple,
            Enunciado = """Al girar la curva \(y=f(x)\), con \(f'(x)\) continua en \([a,b]\), alrededor del eje \(x\), el área de la superficie generada es:""",
            RespuestaCorrecta = "A",
            Opciones =
            [
                @"\[S=\int_{a}^{b}2\pi f(x)\sqrt{1+[f'(x)]^{2}}\,dx\]",
                @"\[S=\int_{a}^{b}\pi[f(x)]^{2}\,dx\]",
                @"\[S=\int_{a}^{b}2\pi x f(x)\,dx\]",
                @"\[S=\int_{a}^{b}\pi\left(1+[f'(x)]^{2}\right)dx\]"
            ],
            Solucion =
            [
                """El elemento de arco es \(ds=\sqrt{1+[f'(x)]^{2}}\,dx\).""",
                """Al girar, ese elemento genera una banda de circunferencia \(2\pi f(x)\) y ancho \(ds\).""",
                """Por lo tanto \(S=\displaystyle\int_{a}^{b}2\pi f(x)\sqrt{1+[f'(x)]^{2}}\,dx\), que corresponde a la opción A."""
            ],
            Comprobacion = """Las opciones B y D corresponden a volúmenes (falta el factor \(2\pi\) y la raíz del arco), y la C es la fórmula de cascarones para volumen. Solo la opción A representa un área superficial."""
        }
    ];

    // ----------------------------------------------------------------------
    // Sección G: fuentes consultadas
    // ----------------------------------------------------------------------
    public static readonly IReadOnlyList<Fuente> Fuentes =
    [
        new("Stewart, J.", "2018", "Cálculo de una variable: trascendentes tempranas (8.ª ed.)", "Cengage Learning",
            "https://www.cengage.com", "Libro de texto"),
        new("Larson, R. y Edwards, B.", "2018", "Cálculo. Tomo 1 (10.ª ed.)", "Cengage Learning",
            "https://www.cengage.com", "Libro de texto"),
        new("Thomas, G. B.", "2015", "Cálculo: una variable (13.ª ed.)", "Pearson Educación",
            "https://www.pearson.com", "Libro de texto"),
        new("Universidad Mariano Gálvez de Guatemala", "2026", "Guía de minicurso: lineamientos de presentación y evaluación", "UMG",
            "https://www.umg.edu.gt", "Documento institucional"),
        new("MathJax Consortium", "2024", "MathJax: documentación de referencia para LaTeX en la web", "MathJax",
            "https://www.mathjax.org", "Recurso digital"),
        new("GeoGebra", "2024", "GeoGebra Clásico: visualización de sólidos de revolución", "GeoGebra GmbH",
            "https://www.geogebra.org", "Recurso digital")
    ];
}
