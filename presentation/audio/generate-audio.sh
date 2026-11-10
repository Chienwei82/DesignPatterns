#!/usr/bin/env bash
# Genera la narración MP3 por escena con la API de fish.audio (https://fish.audio).
# Requiere: curl, jq (opcional), y la variable de entorno FISH_API_KEY.
#
# Uso:
#   export FISH_API_KEY=tu_clave
#   bash audio/generate-audio.sh

set -euo pipefail

API_BASE="https://api.fish.audio/v1/tts"
MODEL="${FISH_MODEL:-s2.1-pro-free}"
FORMAT="${FISH_FORMAT:-mp3}"
OUT_DIR="$(cd "$(dirname "$0")" && pwd)"

if [[ -z "${FISH_API_KEY:-}" ]]; then
  echo "❌ Falta la variable de entorno FISH_API_KEY."
  echo "   Consíguela en https://fish.audio (Get Started / API Keys) y ejecuta:"
  echo "   export FISH_API_KEY=tu_clave"
  exit 1
fi

if ! command -v curl >/dev/null 2>&1; then
  echo "❌ curl no está instalado."
  exit 1
fi

# Una entrada por escena: "archivo|texto"
# Los textos vienen de guion.md (resumidos para narración).
mapfile -t ESCENAS <<'EOF'
01-portada.mp3|Los patrones de diseño son soluciones probadas y reutilizables a problemas comunes en el software orientado a objetos. Fueron catalogados por la Gang of Four en 1994 y se dividen en tres familias: creacionales, que resuelven cómo se crean los objetos; estructurales, que resuelven cómo se componen las clases; y de comportamiento, que resuelven cómo se comunican entre sí. En este repositorio encontrarás veinte de ellos implementados en C# y .NET 10, siempre en español.
02-creacionales.mp3|Los patrones creacionales controlan cómo nacen los objetos. Singleton garantiza una única instancia global, ideal para loggers o configuración. Factory Method deja que las subclases decidan qué clase crear. Abstract Factory crea familias de objetos que deben combinarse entre sí. Builder separa la construcción compleja en pasos encadenables, evitando constructores gigantes. Y Prototype crea objetos copiando un prototipo existente en vez de partir desde cero.
03-estructurales.mp3|Los patrones estructurales organizan cómo se componen las clases y objetos. Adapter hace compatible una interfaz ajena, como un adaptador de enchufe. Decorator añade responsabilidades nuevas sin modificar la clase original. Facade ofrece una interfaz simplificada sobre un subsistema complejo. Proxy controla el acceso a un objeto, por ejemplo con caché o seguridad. Composite trata objetos simples y compuestos de forma uniforme. Bridge desacopla una abstracción de su implementación. Y Flyweight comparte estado intrínseco para ahorrar memoria cuando hay muchos objetos similares.
04-comportamiento.mp3|Los patrones de comportamiento gestionan la comunicación y las responsabilidades entre objetos. Strategy permite intercambiar algoritmos en tiempo de ejecución. Observer avisa a varios interesados cuando algo cambia, ideal para eventos de UI. Command encapsula una solicitud como objeto para deshacerla o encolarla. Template Method define el esqueleto de un algoritmo y deja que las subclases rellenen pasos. State hace que un objeto cambie de comportamiento según su estado interno. Mediator centraliza la comunicación mediante un intermediario. Memento captura y restaura estado sin romper el encapsulamiento. Y Chain of Responsibility pasa una solicitud por una cadena de manejadores hasta que alguien la resuelva.
05-cierre.mp3|Este proyecto incluye veinte de los veintitrés patrones GoF. Faltan a propósito Interpreter, Iterator y Visitor: Iterator ya lo cubre C# con IEnumerable y LINQ, Visitor es muy especializado e Interpreter es raro en la práctica. Lo más poderoso es que los patrones se combinan: un Abstract Factory puede crear Strategies, coordinadas por un Mediator que notifica cambios vía Observer. Pruébalo tú mismo con dotnet run en el repositorio.
EOF

echo "🎙️  Generando audio con fish.audio (modelo: $MODEL, formato: $FORMAT)..."

for entrada in "${ESCENAS[@]}"; do
  archivo="${entrada%%|*}"
  texto="${entrada#*|}"
  salida="$OUT_DIR/$archivo"

  echo "▶ $archivo"

  curl --silent --show-error --fail \
    --request POST "$API_BASE" \
    --header "Authorization: Bearer $FISH_API_KEY" \
    --header "Content-Type: application/json" \
    --header "model: $MODEL" \
    --data "{ \"text\": \"$texto\", \"format\": \"$FORMAT\" }" \
    --output "$salida"

  if [[ -s "$salida" ]]; then
    echo "   ✅ Generado: $salida"
  else
    echo "   ❌ Falló la generación de $archivo"
    exit 1
  fi
done

echo "🎧  Todo el audio generado en $OUT_DIR"
echo "   Revisa los tiempos frente a guion.md y ajusta index.html si es necesario."