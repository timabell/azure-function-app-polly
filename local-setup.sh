#!/bin/bash
set -e # stop on error
echo "Starting docker compose services..."
docker compose --profile local-dev up -d
