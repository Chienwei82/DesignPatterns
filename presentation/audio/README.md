# Audio · Narración con fish.audio

Genera la narración MP3 del vídeo (`presentation/guion.md`) usando la API de **https://fish.audio** (Text to Speech).

## 1. Consigue tu API key
1. Entra en https://fish.audio e inicia sesión.
2. Ve a **Get Started / API Keys** y crea una nueva clave.
3. Copiala y exportala como variable de entorno:

```bash
export FISH_API_KEY=tu_clave_aquí
```

> Nunca subas `FISH_API_KEY` al repositorio. Está en `.gitignore` (o debe estarlo).

## 2. Ejecuta la generación

Desde la carpeta `presentation/`:

```bash
# Con Node (sin instalar dependencias, usa fetch nativo)
npm run audio

# O con bash + curl
bash audio/generate-audio.sh
```

Esto genera un MP3 por escena en `audio/`:

| Archivo | Escena | Duración real (medida) |
|---------|--------|------------------------|
| `audio/01-portada.mp3` | Portada | ~30,9 s |
| `audio/02-creacionales.mp3` | Creacionales | ~28,2 s |
| `audio/03-estructurales.mp3` | Estructurales | ~35,0 s |
| `audio/04-comportamiento.mp3` | Comportamiento | ~46,2 s |
| `audio/05-cierre.mp3` | Cierre | ~25,1 s |

## 3. Voces (`voices.json`)

Edita `audio/voices.json` y coloca el `reference_id` de cada voz que quieras usar (las encuentras en la Voice Library de fish.audio o clona tu propia voz). Si no se indica `reference_id`, se usa la voz por defecto del modelo.

## 4. Modelo y formato

- `model`: `s2.1-pro-free` (gratis, ideal para probar) o `s2-pro` / `s2.1-pro` (producción).
- `format`: `mp3`.
- Endpoint: `POST https://api.fish.audio/v1/tts`
- Cabecera de autenticación: `Authorization: Bearer $FISH_API_KEY`

## 5. Sincronizar con el vídeo

No hay que ajustar nada a mano. Los textos viven en `audio/narracion.json`, y este comando:

```bash
npm run timing
```

1. mide cada MP3 (duración y pausas con `ffmpeg silencedetect`),
2. calcula el inicio de cada escena, los crossfades y la duración total,
3. sitúa cada elemento con `data-mencion="..."` de `index.html` en el instante en que se
   nombra (así se resalta en pantalla),
4. reescribe `audio/timing.json`, el bloque `GUION` y los `data-start`/`data-duration`
   de `index.html`.

Después, `npm run audio:compose` monta `audio/final-audio.mp3` con cada narración **entera**
(1,2 s de silencio entre escenas y relleno hasta la duración del vídeo). `npm run render`
hace ambos pasos y renderiza el vídeo con el audio ya sincronizado.

Ajustes opcionales: `HF_PAD` (silencio entre escenas), `HF_FUNDIDO` (crossfade),
`HF_COLA` (cola final) y `HF_HOLD` (duración máxima de un resalte).
