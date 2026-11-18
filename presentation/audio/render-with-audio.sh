#!/bin/bash
# render-with-audio.sh — Renderiza el vídeo y le añade la narración completa.
#
#   1. Recalcula el guion desde los MP3 (audio/timing.json + index.html).
#   2. Monta audio/final-audio.mp3 con cada narración entera + silencios.
#   3. Renderiza el vídeo con hyperframes.
#   4. Mezcla vídeo + audio; el audio se rellena con silencio hasta el final
#      del vídeo, de modo que nunca se corta una frase.
#
# Uso: bash audio/render-with-audio.sh  (o: npm run render)
set -e
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
cd "$PROJECT_DIR"

echo "🧭 Recalculando guion desde el audio..."
node audio/build-timeline.mjs

echo "🎵 Montando la narración completa..."
node audio/build-audio.mjs

echo "🎬 Renderizando video con hyperframes..."
npx --yes hyperframes@0.8.81 render --output renders/video.mp4

echo "🔗 Combinando video + audio..."
ffmpeg -hide_banner -loglevel error \
  -i "$PROJECT_DIR/renders/video.mp4" \
  -i "$PROJECT_DIR/audio/final-audio.mp3" \
  -map 0:v:0 -map 1:a:0 \
  -c:v copy -c:a aac -b:a 192k -ar 44100 \
  -af apad -shortest -y "$PROJECT_DIR/renders/video-con-audio.mp4"

mv "$PROJECT_DIR/renders/video-con-audio.mp4" "$PROJECT_DIR/renders/video.mp4"

DUR=$(ffprobe -v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 "$PROJECT_DIR/renders/video.mp4")
echo "✅ Video con audio listo: renders/video.mp4 (${DUR}s)"
