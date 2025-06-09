
## About
Discord bot using [NetCord](https://netcord.dev/) that integrates with Google Gemini API for text and image generation

## Features
- Dedicated separate command for text & image responses
- Support for over 2000 character responses by splitting responses into multiple messages chained with a [Part] Identifier
- Logging of all discord interactions, prompts, as well as API responses and associated metadata to a postgres database
- Guardrail/API request limit by entering RPM and/or RPD limit (persists across application shutdown and cleared by a background worker)
- Docker Support


### Setup Instructions
1. Get Gemini [Api Key](https://ai.google.dev/gemini-api/docs/api-key)
2. Create a .env file in the root of the project and enter specified secrets (or rename .env.example to .env)
### Building & running through docker
1. Ensure you are in the root of the folder
2. ``docker build -t image_name .`` to build the image
3. ``docker run -d --name container_name -e DISCORD_TOKEN=yourtoken -e CONNECTION_STRING=yourstring -e DATABASE_NAME =databasename -e DATABASE_USERNAME=username -e DATABASE_PASSWORD -e DATABASE_PORT=port -e GEMINI_API_KEY=yourapikey -e GEMINI_TEXT_MODEL=textmodelname -e GEMINI_IMAGE_MODEL=imagemodelname -e GEMINI_TEXT_MODEL_MINUTE_LIMIT=number -e GEMINI_IMAGE_MODEL_MINUTE_LIMIT=number -e GEMINI_TEXT_MODEL_DAILY_LIMIT=number -e GEMINI_IMAGE_MODEL_DAILY_LIMIT=number image_name`` to run the container

