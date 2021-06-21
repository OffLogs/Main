binable/kafka/bin/windows/kafka-topics.sh --create --topic quickstart-events --bootstrap-server localhost:9092

kafka-topics.sh --create --topic=prod-logs --if-not-exists --partitions=5 --bootstrap-server localhost:9092
kafka-topics.sh --create --topic=prod-notifications --if-not-exists --partitions=5 --bootstrap-server localhost:9092