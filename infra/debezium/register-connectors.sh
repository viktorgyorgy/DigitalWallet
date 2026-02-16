#!/bin/sh

# Install dependencies
apk add --no-cache curl gettext

echo "Waiting for Debezium at http://debezium:8083..."
until curl -s -o /dev/null -w "%{http_code}" http://debezium:8083/connectors | grep -q "200"; do
  sleep 2
done

echo "Debezium is UP. Processing templates..."

for f in /templates/*.json.template; do
  # Get filename without path and extension
  connector_name=$(basename "$f" .json.template)
  
  echo "--------------------------------------------------"
  echo "Processing: $connector_name"
  
  # Inject environment variables into the template
  envsubst < "$f" > "/tmp/config.json"
  
  # Register/Update the connector (PUT makes it idempotent)
  RESPONSE=$(curl -s -w "\n%{http_code}" -X PUT \
    -H "Content-Type: application/json" \
    --data @/tmp/config.json \
    "http://debezium:8083/connectors/$connector_name/config")

  HTTP_STATUS=$(echo "$RESPONSE" | tail -n1)
  
  if [ "$HTTP_STATUS" -eq 200 ] || [ "$HTTP_STATUS" -eq 201 ]; then
    echo "Successfully registered $connector_name (Status: $HTTP_STATUS)"
  else
    echo "Failed to register $connector_name (Status: $HTTP_STATUS)"
    echo "Response: $(echo "$RESPONSE" | head -n1)"
  fi
done

echo "--------------------------------------------------"
echo "Debezium configuration complete."