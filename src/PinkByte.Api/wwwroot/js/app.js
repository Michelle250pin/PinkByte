// =========================================================
// PINK BYTE · Lógica del frontend
// El front NO decide reglas del juego: solo pide cosas a la API y dibuja.
// =========================================================

const CLAVE_GUARDADO = "pinkbyte.protagonistaId";
const EMOJI_VILLANO = { BugFantasma: "👻", SpaghettiCode: "🍝", LaDeadline: "⏰", SindromeDelImpostor: "🖤" };

// Estado de la app en un solo objeto (fácil de razonar)
const estado = { protagonista: null, villanos: [], combate: null };

const $ = (selector) => document.querySelector(selector);

// ---------- Comunicación con la API ----------
async function api(ruta, { metodo = "GET", cuerpo } = {}) {
  const respuesta = await fetch(`/api/${ruta}`, {
    method: metodo,
    headers: cuerpo ? { "Content-Type": "application/json" } : {},
    body: cuerpo ? JSON.stringify(cuerpo) : undefined,
  });
  const datos = await respuesta.json().catch(() => null);
  if (!respuesta.ok) throw new Error(datos?.title ?? "Algo salió mal 💀");
  return datos;
}

// ---------- Utilidades ----------
function mostrarPantalla(id) {
  document.querySelectorAll(".pantalla").forEach((p) => (p.hidden = p.id !== id));
  window.scrollTo({ top: 0, behavior: "smooth" });
}

function avisar(mensaje) {
  const aviso = $("#aviso");
  aviso.textContent = mensaje;
  aviso.hidden = false;
  clearTimeout(avisar.timer);
  avisar.timer = setTimeout(() => (aviso.hidden = true), 3500);
}

// Escapa texto del usuario antes de meterlo en HTML (evita inyección de código)
function seguro(texto) {
  const div = document.createElement("div");
  div.textContent = texto;
  return div.innerHTML;
}

function avatarSvg(colorPelo, estilo) {
  const accesorio = {
    Gotica: `<path d="M38 22 l6 -10 l6 10 M70 22 l6 -10 l6 10" stroke="#1a0613" stroke-width="3" fill="none"/>`,
    Punk: `<path d="M48 10 l6 -10 l6 10 l6 -10 l6 10" fill="${colorPelo}"/>`,
    Kawaii: `<path d="M44 20 l-14 -8 v16 z M44 20 l14 -8 v16 z" fill="#ffb8da"/><circle cx="44" cy="20" r="4" fill="#ff4fa3"/>`,
  }[estilo] ?? "";

  return `<svg viewBox="0 0 120 120" role="img" aria-label="Avatar">
    <circle cx="60" cy="60" r="58" fill="#2a1424" stroke="#4a2240" stroke-width="2"/>
    <path d="M22 70 Q20 18 60 18 Q100 18 98 70 L98 104 L86 104 L84 60 L36 60 L34 104 L22 104 Z" fill="${colorPelo}"/>
    <ellipse cx="60" cy="64" rx="24" ry="27" fill="#f6d5c9"/>
    <path d="M36 52 Q60 28 84 52 Q72 40 60 44 Q48 40 36 52 Z" fill="${colorPelo}"/>
    <circle cx="51" cy="66" r="3.5" fill="#1a0613"/><circle cx="69" cy="66" r="3.5" fill="#1a0613"/>
    <path d="M47 60 l7 2 M73 60 l-7 2" stroke="#1a0613" stroke-width="2"/>
    <path d="M53 79 Q60 83 67 79" stroke="#c2185b" stroke-width="3" fill="none" stroke-linecap="round"/>
    <path d="M30 120 Q30 94 60 92 Q90 94 90 120 Z" fill="#ff4fa3"/>
    <text x="60" y="113" text-anchor="middle" font-size="10" font-family="monospace" fill="#1a0613">&lt;/&gt;</text>
    ${accesorio}
  </svg>`;
}

function barra(tipo, etiqueta, actual, maximo) {
  const porcentaje = Math.round((actual / maximo) * 100);
  return `<div class="barra ${tipo}">
      <span>${etiqueta}: ${actual}/${maximo}</span>
      <div class="barra-fondo" role="progressbar" aria-valuenow="${actual}" aria-valuemax="${maximo}">
        <div class="barra-relleno" style="width:${porcentaje}%"></div>
      </div>
    </div>`;
}

// ---------- PANTALLA 1: Creación ----------
const form = $("#form-creacion");

function actualizarPreview() {
  const datos = new FormData(form);
  $("#avatar-preview").innerHTML = avatarSvg(datos.get("colorPelo"), datos.get("estilo"));
}

form.addEventListener("input", actualizarPreview);

form.addEventListener("submit", async (evento) => {
  evento.preventDefault();
  const datos = new FormData(form);
  try {
    estado.protagonista = await api("protagonistas", {
      metodo: "POST",
      cuerpo: { nombre: datos.get("nombre"), estilo: datos.get("estilo"), colorPelo: datos.get("colorPelo") },
    });
    localStorage.setItem(CLAVE_GUARDADO, estado.protagonista.id);
    await abrirMapa();
  } catch (error) {
    avisar(error.message);
  }
});

// ---------- PANTALLA 2: Mapa ----------
async function abrirMapa() {
  if (estado.villanos.length === 0) estado.villanos = await api("villanos");
  dibujarFicha();
  dibujarVillanos();
  mostrarPantalla("pantalla-mapa");
}

function dibujarFicha() {
  const p = estado.protagonista;
  $("#ficha").innerHTML = `
    <div class="avatar">${avatarSvg(p.colorPelo, p.estilo)}</div>
    <h2>${seguro(p.nombre)}</h2>
    <p class="descripcion">Nivel ${p.nivel} · ${p.estilo}</p>
    ${barra("vida", "XP", p.experiencia, p.experienciaParaSubir)}
    <dl class="stats">
      <div><dt>Vida</dt><dd>${p.vidaMaxima}</dd></div>
      <div><dt>Ataque</dt><dd>${p.ataque}</dd></div>
      <div><dt>Defensa</dt><dd>${p.defensa}</dd></div>
      <div><dt>Café</dt><dd>${p.cafeMaximo}</dd></div>
    </dl>
    <ul class="chips">${p.lenguajes.map((l) => `<li>${l.icono} ${l.nombre}</li>`).join("")}</ul>
    <button class="boton" id="btn-nueva">Nueva partida</button>`;

  $("#btn-nueva").onclick = () => {
    localStorage.removeItem(CLAVE_GUARDADO);
    location.reload();
  };
}

function dibujarVillanos() {
  const sabe = (nombre) => estado.protagonista.lenguajes.some((l) => l.nombre === nombre);

  $("#lista-villanos").innerHTML = estado.villanos.map((v, i) => {
    // Un villano se desbloquea cuando ya aprendiste lo que enseña el anterior
    const anterior = estado.villanos[i - 1];
    const desbloqueado = !anterior || sabe(anterior.ensena?.nombre);
    const vencido = v.ensena && sabe(v.ensena.nombre);

    return `<li class="tarjeta villano-item ${desbloqueado ? "" : "bloqueado"} ${vencido ? "vencido" : ""}">
        <div>
          <h3>${EMOJI_VILLANO[v.tipo] ?? "😈"} ${v.nombre}</h3>
          <p>${v.descripcion}</p>
          <p class="frase">“${v.frase}”</p>
          <p class="recompensa">${v.ensena ? `Recompensa: ${v.ensena.icono} ${v.ensena.nombre}` : "Recompensa: la entrevista en la Gran Empresa 🏙️"} · ${v.experiencia} XP</p>
        </div>
        <button class="boton ${desbloqueado ? "boton-principal" : ""}" data-tipo="${v.tipo}" ${desbloqueado ? "" : "disabled"}>
          ${desbloqueado ? (vencido ? "Revancha" : "Pelear") : "🔒 Bloqueado"}
        </button>
      </li>`;
  }).join("");
}

// Delegación de eventos: UN listener para todos los botones de la lista
$("#lista-villanos").addEventListener("click", async (evento) => {
  const tipo = evento.target.closest("button")?.dataset.tipo;
  if (!tipo) return;
  try {
    estado.combate = await api("combates", {
      metodo: "POST",
      cuerpo: { protagonistaId: estado.protagonista.id, tipoVillano: tipo },
    });
    dibujarCombate();
    mostrarPantalla("pantalla-combate");
  } catch (error) {
    avisar(error.message);
  }
});

// ---------- PANTALLA 3: Combate ----------
function dibujarCombate() {
  const { protagonista: p, villano: v, estado: estadoCombate, registro } = estado.combate;
  const terminado = estadoCombate !== "EnCurso";

  $("#lado-protagonista").innerHTML = `
    <header>${avatarSvg(p.colorPelo, p.estilo)}<div><h3>${seguro(p.nombre)}</h3><small>Nivel ${p.nivel}</small></div></header>
    ${barra("vida", "Vida", p.vida, p.vidaMaxima)}
    ${barra("cafe", "Café", p.cafe, p.cafeMaximo)}`;

  $("#lado-villano").innerHTML = `
    <header><span class="emoji-villano">${EMOJI_VILLANO[v.tipo] ?? "😈"}</span><div><h3>${v.nombre}</h3><small>Ataque ${v.ataque} · Defensa ${v.defensa}</small></div></header>
    ${barra("vida", "Vida", v.vida, v.vidaMaxima)}`;

  $("#acciones").innerHTML = terminado
    ? `<button class="boton boton-principal" id="btn-volver">${estadoCombate === "Victoria" ? "🏆 Volver al mapa" : "💀 Volver a intentarlo"}</button>`
    : p.habilidades.map((h) => `
        <button class="boton accion" data-habilidad="${seguro(h.nombre)}" ${h.costoCafe > p.cafe ? "disabled" : ""}>
          ${seguro(h.nombre)} ${h.costoCafe ? `<small>☕ ${h.costoCafe}</small>` : ""}
          <small>${h.descripcion}</small>
        </button>`).join("");

  $("#registro").innerHTML = [...registro].reverse()
    .map((linea) => `<li class="${linea.startsWith("—") ? "separador" : ""}">${seguro(linea)}</li>`)
    .join("");

  if (terminado) {
    estado.protagonista = p;   // guardamos XP / lenguajes nuevos
    $("#btn-volver").onclick = abrirMapa;
  }
}

$("#acciones").addEventListener("click", async (evento) => {
  const boton = evento.target.closest("button[data-habilidad]");
  if (!boton) return;
  try {
    estado.combate = await api(`combates/${estado.combate.id}/turnos`, {
      metodo: "POST",
      cuerpo: { habilidad: boton.dataset.habilidad },
    });
    dibujarCombate();
  } catch (error) {
    avisar(error.message);
  }
});

// ---------- Arranque ----------
(async function iniciar() {
  actualizarPreview();
  const id = localStorage.getItem(CLAVE_GUARDADO);
  if (id) {
    try {
      estado.protagonista = await api(`protagonistas/${id}`);
      return await abrirMapa();
    } catch {
      localStorage.removeItem(CLAVE_GUARDADO);   // el servidor se reinició: empezamos de nuevo
    }
  }
  mostrarPantalla("pantalla-creacion");
})();
