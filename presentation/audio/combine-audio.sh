#!/bin/bash
# combine-audio.sh — Recorta cada MP3 de escena a 12 s y los concatena en un solo audio de 60 s.
# Uso: bash audio/combine-audio.sh (desde la carpeta presentation/)
set -e
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
cd "$PROJECT_DIR"

SCENE_DURATION=12
TMP_DIR="$PROJECT_DIR/.audio-tmp"
mkdir -p "$TMP_DIR"

echo "🎵 Recortando cada MP3 a ${SCENE_DURATION}s..."

ffmpeg -hide_banner -loglevel error -i "$PROJECT_DIR/audio/01-portada.mp3"   -t "$SCENE_DURATION" -c copy -y "$TMP_DIR/01-cut.mp3"
ffmpeg -hide_banner -loglevel error -i "$PROJECT_DIR/audio/02-creacionales.mp3" -t "$SCENE_DURATION" -c copy -y "$TMP_DIR/02-cut.mp3"
ffmpeg -hide_banner -loglevel error -i "$PROJECT_DIR/audio/03-estructurales.mp3" -t "$SCENE_DURATION" -c copy -y "$TMP_DIR/03-cut.mp3"
ffmpeg -hide_banner -loglevel error -i "$PROJECT_DIR/audio/04-comportamiento.mp3" -t "$SCENE_DURATION" -c copy -y "$TMP_DIR/04-cut.mp3"
ffmpeg -hide_banner -loglevel error -i "$PROJECT_DIR/audio/05-cierre.mp3"     -t "$SCENE_DURATION" -c copy -y "$TMP_DIR/05-cut.mp3"

echo "📝 Creando lista de concatenación..."
{
  echo "file '$TMP_DIR/01-cut.mp3'"
  echo "file '$TMP_DIR/02-cut.mp3'"
  echo "file '$TMP_DIR/03-cut.mp3'"
  echo "file '$TMP_DIR/04-cut.mp3'"
  echo "file '$TMP_DIR/05-cut.mp3'"
} > "$TMP_DIR/concat.txt"

echo "🔗 Concatenando en audio/final-audio.mp3..."
ffmpeg -hide_banner -loglevel error -f concat -safe 0 -i "$TMP_DIR/concat.txt" -c copy -y "$PROJECT_DIR/audio/final-audio.mp3"

rm -rf "$TMP_DIR"

DUR=$(ffprobe -v error -show_entries format=duration -of default=noprint_wrappers=1:nokey=1 "$PROJECT_DIR/audio/final-audio.mp3")
echo "✅ Audio final listo: audio/final-audio.mp3 (${DUR}s)"