#!/bin/bash -e

# Try and configure minimal settings or exit with error if there isn't enough information
if [[ -z "$KAFKA_ADVERTISED_HOST_NAME$KAFKA_LISTENERS" ]]; then
  if [[ -n "$KAFKA_ADVERTISED_LISTENERS" ]]; then
    echo "ERROR: Missing environment variable KAFKA_LISTENERS. Must be specified when using KAFKA_ADVERTISED_LISTENERS"
    exit 1
  elif [[ -z "$HOSTNAME_VALUE" ]]; then
    echo "ERROR: No listener or advertised hostname configuration provided in environment."
    echo "       Please define KAFKA_LISTENERS / (deprecated) KAFKA_ADVERTISED_HOST_NAME"
    exit 1
  fi

  # Maintain existing behaviour
  # If HOSTNAME_COMMAND is provided, set that to the advertised.host.name value if listeners are not defined.
  export KAFKA_ADVERTISED_HOST_NAME="$HOSTNAME_VALUE"
fi

#Issue newline to config file in case there is not one already
echo "" >>"$KAFKA_HOME/config/server.properties"

(
  function updateConfig() {
    key=$1
    value=$2
    file=$3

    # Omit $value here, in case there is sensitive information
    echo "[Configuring] '$key' in '$file'"

    # If config exists in file, replace it. Otherwise, append to file.
    if grep -E -q "^#?$key=" "$file"; then
      sed -r -i "s@^#?$key=.*@$key=$value@g" "$file" #note that no config values may contain an '@' char
    else
      echo "$key=$value" >>"$file"
    fi
  }

  # Fixes #312
  # KAFKA_VERSION + KAFKA_HOME + grep -rohe KAFKA[A-Z0-0_]* /opt/kafka/bin | sort | uniq | tr '\n' '|'
  EXCLUSIONS="|KAFKA_VERSION|KAFKA_HOME|KAFKA_DEBUG|KAFKA_GC_LOG_OPTS|KAFKA_HEAP_OPTS|KAFKA_JMX_OPTS|KAFKA_JVM_PERFORMANCE_OPTS|KAFKA_LOG|KAFKA_OPTS|"

  # Read in env as a new-line separated array. This handles the case of env variables have spaces and/or carriage returns. See #313
  IFS=$'\n'
  for VAR in $(env); do
    env_var=$(echo "$VAR" | cut -d= -f1)
    if [[ "$EXCLUSIONS" == *"|$env_var|"* ]]; then
      echo "Excluding $env_var from broker config"
      continue
    fi

    if [[ $env_var =~ ^KAFKA_ ]]; then
      kafka_name=$(echo "$env_var" | cut -d_ -f2- | tr '[:upper:]' '[:lower:]' | tr _ .)
      updateConfig "$kafka_name" "${!env_var}" "$KAFKA_HOME/config/server.properties"
    fi

    if [[ $env_var =~ ^LOG4J_ ]]; then
      log4j_name=$(echo "$env_var" | tr '[:upper:]' '[:lower:]' | tr _ .)
      updateConfig "$log4j_name" "${!env_var}" "$KAFKA_HOME/config/log4j.properties"
    fi
  done
)

if [[ -n "$CUSTOM_INIT_SCRIPT" ]]; then
  eval "$CUSTOM_INIT_SCRIPT"
fi

exec $KAFKA_HOME/bin/zookeeper-server-start.sh $KAFKA_HOME/config/zookeeper.properties