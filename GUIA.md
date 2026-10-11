# 💗 Pink Byte: guía de aprendizaje

Juego web para aprender **C#, .NET, Programación Orientada a Objetos (POO) y diseño UX responsivo**.

## ▶️ Cómo correrlo

```bash
cd src/PinkByte.Api
dotnet run
```
Abre **http://localhost:5095** para jugar y **http://localhost:5095/swagger** para probar la API a mano.

---

## 📖 La historia

> En **Ciudad Obsidiana**, una metrópoli gótica iluminada con neón rosa, la **Gran Empresa** solo
> contrata a quien cruce el distrito de los bugs. Tú eres una programadora ruda, de rosa de pies
> a cabeza, con un teclado mecánico como arma y café de fresa como combustible.
> Cada villano que vences te enseña un lenguaje nuevo. Al final te espera el enemigo más difícil:
> el **Síndrome del Impostor**, que solo puedes vencer si de verdad aprendiste.

| Nivel | Villano | Cómo pelea (POO) | Te enseña |
|---|---|---|---|
| 1 | 👻 Bug Fantasma | `override RecibirDanio`: esquiva el 30% | JavaScript ⚡ |
| 2 | 🍝 Señor Spaghetti Code | golpea de 1 a 3 veces | C# 💜 |
| 3 | ⏰ La Deadline | se hace más fuerte cada turno | SQL 🗄️ |
| Jefe final | 🖤 Síndrome del Impostor | pega el doble si sabes menos de 4 lenguajes | ¡La entrevista! |

---

## 🗂️ Orden de carpetas (y por qué)

```
ProyectoPoo/
├── PinkByte.sln                  ← la "solución": agrupa proyectos
└── src/PinkByte.Api/
    ├── Domain/                   ← 🧠 EL CORAZÓN: las reglas del juego (POO pura, sin HTTP)
    │   ├── Comun/                   IEntidad (interfaz) + excepciones propias
    │   ├── Personajes/              Personaje (abstracta) → Protagonista
    │   ├── Villanos/                Villano (abstracta) → 4 villanos + Fábrica
    │   ├── Habilidades/             IHabilidad + habilidades + Lenguaje
    │   └── Combates/                Combate: controla turnos, victoria, derrota
    ├── Repositorios/             ← 💾 dónde se guardan los datos (por ahora en memoria)
    ├── Servicios/                ← 🎼 orquesta: busca, crea, guarda
    ├── DTOs/                     ← 📦 la forma del JSON que entra y sale
    ├── Controllers/              ← 🚪 las puertas HTTP (endpoints)
    ├── Middleware/               ← 🛡️ convierte errores en respuestas HTTP
    ├── wwwroot/                  ← 🎨 frontend: HTML + CSS + JS
    └── Program.cs                ← ⚙️ arranque y configuración
```

**Regla de oro:** las flechas de dependencia van hacia adentro.
`Controllers → Servicios → Domain`. El **Domain no conoce** a nadie de afuera.
Así puedes cambiar la web, la base de datos o el front sin tocar las reglas del juego.

---

## 🏛️ Los 4 pilares de la POO, en este proyecto

| Pilar | Qué significa | Dónde verlo |
|---|---|---|
| **Abstracción** | Modelar la idea general y ocultar detalles | `Personaje` y `Villano` son `abstract` |
| **Encapsulamiento** | Proteger los datos; solo se cambian con reglas | `_vida` privada + `Math.Clamp`; `GastarCafe()` valida |
| **Herencia** | Una clase "es un" tipo de otra y reutiliza su código | `Protagonista : Personaje`, `BugFantasma : Villano` |
| **Polimorfismo** | Mismo mensaje, comportamiento distinto | `Villano.RealizarTurno()` e `IHabilidad.Ejecutar()` |

Extras "nivel pro" que también aparecen: **interfaces**, **genéricos** (`IRepositorio<T>`),
**patrón Factory** (`FabricaDeVillanos`), **inyección de dependencias**, **métodos de extensión**
(`Mapeos.ADto()`), **records**, **pattern matching**.

---

## ✍️ Orden recomendado para transcribir

Escríbelo tú, en este orden, y compila (`dotnet build`) después de cada paso:

1. `Domain/Comun/IEntidad.cs` y `Excepciones.cs`
2. `Domain/Personajes/Personaje.cs` ← **léelo dos veces, es la clase más importante**
3. `Domain/Personajes/Estilo.cs` y `Protagonista.cs`
4. `Domain/Habilidades/IHabilidad.cs` → `Habilidades.cs` → `Lenguaje.cs`
5. `Domain/Villanos/Villano.cs` → `Villanos.cs` → `FabricaDeVillanos.cs`
6. `Domain/Combates/Combate.cs`
7. `Repositorios/` → `Servicios/` → `DTOs/` → `Controllers/` → `Middleware/` → `Program.cs`
8. `wwwroot/` (HTML → CSS → JS)

---

## 🎨 Lecciones de UX/UI que ya trae

- **Design tokens**: todos los colores en variables CSS (`:root`) → cambia uno y cambia todo el juego.
- **Mobile-first**: el CSS base es para celular; `@media (min-width: 720px)` solo *agrega* cambios.
- **Áreas táctiles de 44px** mínimo en botones.
- **Feedback inmediato**: barras animadas, botones deshabilitados si no te alcanza el café, avisos de error.
- **Accesibilidad**: `aria-live` en el registro, `role="progressbar"`, foco visible, `prefers-reduced-motion`.
- **Revelación progresiva**: villanos bloqueados hasta que aprendes lo necesario.

---

## 🗺️ Próximas fases

- **Fase 2 · Inventario y outfits**: ropa rosa con bonos (composición en vez de herencia).
- **Fase 3 · Base de datos**: cambiar `RepositorioEnMemoria` por Entity Framework + SQLite.
- **Fase 4 · Pruebas unitarias** con xUnit (¿la vida nunca baja de 0? ¡pruébalo!).
- **Fase 5 · Jefe final especial**: entrevista técnica con preguntas reales de C#.
- **Fase 6 · Estados alterados** (patrón State/Strategy): "Burnout", "Modo Flow", "Café frío".

## 🧩 Retos para practicar tú sola

1. Crea un villano nuevo `ElReclutadorFantasma` que robe café. Regístralo en la fábrica.
2. Agrega una habilidad `Git Revert` que cure la mitad del daño recibido en el último turno.
3. Haz que el servidor (no solo el front) impida pelear con un villano bloqueado.
4. Cambia la paleta de colores del juego editando solo las variables de `:root`.
