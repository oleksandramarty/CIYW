#!/bin/bash

# Redis connection details
redis_host="localhost"
redis_port="6379"

# Key and value to insert
key=$1
value=$2

# Check if both key and value are provided
if [ -z "$key" ] || [ -z "$value" ]; then
  echo "Usage: $0 <key> <value>"
  exit 1
fi

# Insert the key and value into Redis
if redis-cli -h $redis_host -p $redis_port SET "$key" "$value"; then
  echo "Successfully inserted key: $key with value: $value"
else
  echo "Failed to insert key: $key with value: $value"
  exit 1
fi