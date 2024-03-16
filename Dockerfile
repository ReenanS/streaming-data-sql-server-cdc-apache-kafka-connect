FROM confluentinc/cp-kafka-connect-base:5.5.0
RUN confluent-hub install --no-prompt debezium/debezium-connector-sqlserver:1.1.0