#!/bin/sh
apk add --no-cache curl gettext

echo "Waiting for Debezium..."
until curl -s http://debezium:8083/connectors > /dev/null; do sleep 2; done

# Define which schemas need an outbox connector
SCHEMAS="users"

for SCHEMA in $SCHEMAS; do
  export SCHEMA_NAME=$SCHEMA
  connector_name="${SCHEMA}-outbox-connector"

  echo "Registering connector for schema: $SCHEMA_NAME"

  envsubst < "/templates/outbox-connector.json.template" > "/tmp/config.json"

  curl -s -X PUT \
    -H "Content-Type: application/json" \
    --data @/tmp/config.json \
    "http://debezium:8083/connectors/$connector_name/config" \
    -o /dev/null

  echo "Done: $connector_name"
done