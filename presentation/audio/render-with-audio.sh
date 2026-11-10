#!/bin/bash
# render-with-audio.sh — Renderiza video con hyperframes y le añade el audio generado.
# Uso: bash audio/render-with-audio.sh (desde la carpeta presentation/)
set -e
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
cd "$PROJECT_DIR"

echo "🎬 Renderizando video con hyperframes..."
npx --yes hyperframes@0.8.81 render --output renders/video.mp4

echo "🎵 Preparando audio final (60s)..."
bash "$PROJECT_DIR/audio/combine-audio.sh"

echo "🔗 Combinando video + audio..."
ffmpeg -hide_banner -loglevel error -i "$PROJECT_DIR/renders/video.mp4" -i "$PROJECT_DIR/audio/final-audio.mp3" \
  -c:v copy -c:a aac -shortest -y "$PROJECT_DIR/renders/video-con-audio.mp4"

mv "$PROJECT_DIR/renders/video-con-audio.mp4" "$PROJECT_DIR/renders/video.mp4"

DUR=$(ffprobe -v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 "$PROJECT_DIR/renders/video.mp4")
echo "✅ Video con audio listo: renders/video.mp4 (${DUR}s)"