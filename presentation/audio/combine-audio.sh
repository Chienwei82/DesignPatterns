#!/bin/bash
# combine-audio.sh — Monta audio/final-audio.mp3 con la narración COMPLETA de
# cada escena (sin recortes) y los mismos tiempos que el vídeo.
#
# La lógica vive en audio/build-audio.mjs y los tiempos en audio/timing.json
# (generado por `npm run timing`). Este envoltorio se mantiene por comodidad.
#
# Uso: bash audio/combine-audio.sh (desde la carpeta presentation/)
set -e
SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
cd "$SCRIPT_DIR/.."
node audio/build-audio.mjs
