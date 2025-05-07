# Load database variables from .env file
if (Test-Path ../.env) {
    Get-Content ../.env | ForEach-Object {
        if ($_ -notmatch '^#') {
            $name, $value = $_ -split '='
            Set-Item -Path "env:$name" -Value $value
        }
    }
} else {
    Write-Host "Error: .env file not found."
    exit 1
}

# Check if the required environment variables are set
if (-not $env:DATABASE_NAME -or -not $env:DATABASE_USERNAME -or -not $env:DATABASE_PASSWORD -or -not $env:DATABASE_PORT) {
    Write-Host "Error: One or more required environment variables (DATABASE_NAME, DATABASE_USERNAME, DATABASE_PASSWORD, DATABASE_PORT) are not set."
    exit 1
}


Write-Host "Database Name: $env:DATABASE_NAME"
Write-Host "Database Username: $env:DATABASE_USERNAME"
Write-Host "Database Password: $env:DATABASE_PASSWORD"
Write-Host "Database Port: $env:DATABASE_PORT"


docker pull postgres:latest

# Run the PostgreSQL container
Write-Host "Starting PostgreSQL container..."
if (-not (docker run --name postgres -e POSTGRES_USER=$env:DATABASE_USERNAME -e POSTGRES_PASSWORD=$env:DATABASE_PASSWORD -e POSTGRES_DB=$env:DATABASE_NAME -p $env:DATABASE_PORT:$env:DATABASE_PORT -d postgres:latest)) {
    Write-Host "Error: Failed to start PostgreSQL container."
    exit 1
}

Write-Host "PostgreSQL container running successfully...."