#!/bin/bash

# Wait for Keycloak to start
while ! curl -s http://localhost:8081/auth; do
  sleep 1
done

/opt/keycloak/bin/kc.sh import --dir /opt/keycloak/data/import --realm Stackoverflow-Lite