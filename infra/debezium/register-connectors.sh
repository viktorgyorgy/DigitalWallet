#!/bin/sh
apk add --no-cache curl gettext

echo "Waiting for Redpanda Proxy..."
until curl -s http://wallet_redpanda:8082/topics > /dev/null; do sleep 2; done

echo "Waiting for Debezium..."
until curl -s http://wallet_debezium:8083/connectors > /dev/null; do sleep 2; done

SCHEMAS="users identity"

for SCHEMA in $SCHEMAS; do
  export SCHEMA_NAME=$SCHEMA
  connector_name="${SCHEMA}-outbox-connector"
  TOPIC_NAME="${ENV_NAME}.${SCHEMA_NAME}.events"

  # 1. Create topic via Redpanda HTTP Proxy (Port 8082)
  # The Proxy uses the Kafka-style API which is more stable for this
  echo "Creating topic: $TOPIC_NAME"
  curl -s -X POST "http://wallet_redpanda:8082/topics" \
       -H "Content-Type: application/vnd.kafka.v2+json" \
       -d "{\"topic_name\": \"$TOPIC_NAME\", \"partitions_count\": 1, \"replication_factor\": 1}"

  # 2. Register the Connector
  echo "Registering connector: $connector_name"
  envsubst < "/templates/outbox-connector.json.template" > "/tmp/config.json"

  curl -s -X PUT \
    -H "Content-Type: application/json" \
    --data @/tmp/config.json \
    "http://wallet_debezium:8083/connectors/$connector_name/config"
  
  echo "Done: $connector_name"
done

echo "Infrastructure fully prepared."