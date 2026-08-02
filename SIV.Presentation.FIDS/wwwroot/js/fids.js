(function () {
    "use strict";

    var intervaloRefresco = 20000;
    var intervaloPagina = 8000;
    var paginaActual = 0;
    var totalPaginas = 1;
    var filasPorPagina = 10;
    var contenedor = null;
    var temporizadorCiclo = null;

    function horaActual() {
        var ahora = new Date();
        var horas = String(ahora.getHours()).padStart(2, "0");
        var minutos = String(ahora.getMinutes()).padStart(2, "0");
        var segundos = String(ahora.getSeconds()).padStart(2, "0");
        return horas + ":" + minutos + ":" + segundos;
    }

    function actualizarReloj() {
        var reloj = document.getElementById("fidsClock");
        var fecha = document.getElementById("fidsDate");
        if (reloj) reloj.textContent = horaActual();
        if (fecha) fecha.textContent = new Date().toLocaleDateString("es-DO", { weekday: "long", day: "numeric", month: "long", year: "numeric" });
    }

    function alternarPantallaCompleta() {
        if (document.fullscreenElement) {
            document.exitFullscreen();
        } else {
            document.documentElement.requestFullscreen();
        }
    }

    function leerTotalPaginas() {
        var oculto = document.getElementById("fidsTotalPaginas");
        var total = oculto ? parseInt(oculto.value, 10) : 1;
        return isNaN(total) || total < 1 ? 1 : total;
    }

    function aplicarPaginacion() {
        var filas = document.querySelectorAll("#fidsTbody .fids-fila");
        if (!filas.length) return;
        totalPaginas = leerTotalPaginas();
        if (paginaActual >= totalPaginas) paginaActual = 0;
        var inicio = paginaActual * filasPorPagina;
        var fin = inicio + filasPorPagina;
        filas.forEach(function (fila, i) {
            if (i >= inicio && i < fin) {
                fila.classList.remove("pagina-oculta");
                fila.classList.add("pagina-visible");
            } else {
                fila.classList.add("pagina-oculta");
                fila.classList.remove("pagina-visible");
            }
        });
        if (totalPaginas > 1 && !temporizadorCiclo) {
            iniciarCiclo();
        }
    }

    function iniciarCiclo() {
        temporizadorCiclo = setInterval(function () {
            paginaActual = (paginaActual + 1) % totalPaginas;
            aplicarPaginacion();
        }, intervaloPagina);
    }

    function detenerCiclo() {
        if (temporizadorCiclo) {
            clearInterval(temporizadorCiclo);
            temporizadorCiclo = null;
        }
    }

    function refrescarContenido() {
        var url = window.FidsConfig ? window.FidsConfig.urlRefresco : null;
        if (!url) return;

        fetch(url, { headers: { "Accept": "text/html" } })
            .then(function (respuesta) {
                if (!respuesta.ok) throw new Error(respuesta.status);
                return respuesta.text();
            })
            .then(function (html) {
                var contenedorActual = document.getElementById("fidsContenido");
                if (!contenedorActual) return;
                detenerCiclo();
                paginaActual = 0;
                contenedorActual.innerHTML = html;
                aplicarPaginacion();
                var pie = document.getElementById("fidsActualizacion");
                if (pie) pie.textContent = "Última actualización: " + horaActual();
                if (!contenedorActual.querySelector(".fids-table")) {
                    console.log("FIDS: la API no devolvió vuelos para el tablero.");
                }
            })
            .catch(function (error) {
                console.log("FIDS: fallo al refrescar el contenido desde TablaPartial.", error);
                var estado = document.getElementById("fidsActualizacion");
                if (estado) estado.textContent = "Reconectando con el servidor de vuelos...";
            });
    }

    document.addEventListener("DOMContentLoaded", function () {
        actualizarReloj();
        setInterval(actualizarReloj, 1000);

        var btnFullscreen = document.getElementById("btnFullscreen");
        if (btnFullscreen) {
            btnFullscreen.addEventListener("click", alternarPantallaCompleta);
        }

        aplicarPaginacion();
        setInterval(refrescarContenido, intervaloRefresco);
    });
})();
