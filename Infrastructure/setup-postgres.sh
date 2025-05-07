#!/bin/sh

# Load database variables from .env file
if [ -f ../.env ]; then
  export $(cat ../.env | grep -v '^#' | xargs)
else
  echo "Error: .env file not found."
  exit 1
fi

# Check if the required environment variables are set
if [ -z "$DATABASE_NAME" ] || [ -z "$DATABASE_USERNAME" ] || [ -z "$DATABASE_PASSWORD" ] || [ -z "$DATABASE_PORT" ]; then
  echo "Error: One or more required environment variables (DATABASE_NAME, DATABASE_USERNAME, DATABASE_PASSWORD, DATABASE_PORT) are not set."
  exit 1
fi

# Use the environment variables
echo "Database Name: $DATABASE_NAME"
echo "Database Username: $DATABASE_USERNAME"
echo "Database Password: $DATABASE_PASSWORD"

# Pull the latest PostgreSQL Docker image
docker pull postgres:latest

# Run the PostgreSQL container
echo "Starting PostgreSQL container..."
if ! docker run --name postgres \
  -e POSTGRES_USER=$DATABASE_USERNAME \
  -e POSTGRES_PASSWORD=$DATABASE_PASSWORD \
  -e POSTGRES_DB=$DATABASE_NAME \
  -p $DATABASE_PORT:$DATABASE_PORT \
  -d postgres:latest; then
  echo "Error: Failed to start PostgreSQL container."
  exit 1
fi


echo "PostgreSQL container running successfully...."