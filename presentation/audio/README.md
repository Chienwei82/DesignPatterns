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

| Archivo | Escena | Duración aproximada |
|---------|--------|---------------------|
| `audio/01-portada.mp3` | Portada | ~12 s |
| `audio/02-creacionales.mp3` | Creacionales | ~12 s |
| `audio/03-estructurales.mp3` | Estructurales | ~12 s |
| `audio/04-comportamiento.mp3` | Comportamiento | ~12 s |
| `audio/05-cierre.mp3` | Cierre | ~10 s |

## 3. Voces (`voices.json`)

Edita `audio/voices.json` y coloca el `reference_id` de cada voz que quieras usar (las encuentras en la Voice Library de fish.audio o clona tu propia voz). Si no se indica `reference_id`, se usa la voz por defecto del modelo.

## 4. Modelo y formato

- `model`: `s2.1-pro-free` (gratis, ideal para probar) o `s2-pro` / `s2.1-pro` (producción).
- `format`: `mp3`.
- Endpoint: `POST https://api.fish.audio/v1/tts`
- Cabecera de autenticación: `Authorization: Bearer $FISH_API_KEY`

## 5. Sincronizar con el vídeo

Los MP3 generados deben encajar en los tiempos de `data-start` / `data-duration` de `index.html` (guion: 60 s total). Si una narración se alarga, ajusta los tiempos de escena en `index.html` y vuelve a ejecutar `npm run check` y `npm run render`.