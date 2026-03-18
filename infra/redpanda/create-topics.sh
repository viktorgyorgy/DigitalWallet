#!/bin/bash
echo "Waiting for Redpanda to be healthy..."
rpk cluster health --watch --exit-when-healthy

SCHEMAS="users identity"

for SCHEMA in $SCHEMAS; do
  TOPIC_NAME="${ENV_NAME}.${SCHEMA}.events"
  echo "Ensuring topic exists: $TOPIC_NAME"
  
  rpk topic create "$TOPIC_NAME" \
    --brokers redpanda:29092 \
    --partitions 1 \
    --replicas 1
done

echo "All topics are ready."