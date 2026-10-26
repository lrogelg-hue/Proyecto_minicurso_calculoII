// Interoperabilidad entre Blazor (C#) y MathJax (JavaScript)
// Permite a los componentes C# solicitar el renderizado de fórmulas LaTeX
// en elementos específicos del DOM después de actualizaciones dinámicas.
window.minicursoMath = {

    // Renderiza (o vuelve a renderizar) las fórmulas dentro de los elementos dados.
    // Cada llamada limpia el estado previo para evitar duplicados.
    typeset: function (elements) {
        const targets = (elements && elements.length)
            ? Array.from(elements).filter(el => el)
            : [document.body];

        if (!targets.length) {
            return Promise.resolve();
        }

        if (!window.MathJax || !window.MathJax.typesetPromise) {
            // MathJax aún no terminó de cargar; se reintenta en 300 ms
            return new Promise(resolve => setTimeout(resolve, 300))
                .then(() => window.minicursoMath.typeset(elements));
        }

        try {
            if (window.MathJax.typesetClear) {
                window.MathJax.typesetClear(targets);
            }
        } catch (e) {
            // typesetClear puede fallar si los elementos no fueron procesados antes
        }

        return window.MathJax.typesetPromise(targets)
            .catch(err => console.warn('MathJax: error al renderizar fórmulas.', err));
    },

    // Libera los recursos de MathJax de elementos que están siendo eliminados.
    clear: function (elements) {
        if (window.MathJax && window.MathJax.typesetClear && elements && elements.length) {
            try {
                window.MathJax.typesetClear(Array.from(elements).filter(el => el));
            } catch (e) { /* sin efecto si no había fórmulas renderizadas */ }
        }
    }
};
