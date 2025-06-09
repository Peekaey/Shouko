
## About
[Discord bot](https://discord.com/developers/docs/intro) using [NetCord](https://netcord.dev/) that integrates with [Google Gemini API](https://ai.google.dev/) for text and image generation

## Features
- Dedicated separate command for text & image responses
- Support for over 2000 character responses by splitting responses into multiple messages chained with a [Part] Identifier
- Logging of all discord interactions, prompts, as well as API responses and associated metadata to a postgres database
- Guardrail/API request limit by entering RPM and/or RPD limit (persists across application shutdown and cleared regularly by a background worker)
- Support for default prompt behaviour (customisable in ApplicationConfigurationSettings.cs)
- Docker Support

#### Bot Permission Requirements
- Message Content Intent
- Bot scope (Guild Install)

#### Bot Commands

| Command             | Paramater(Api Type) | Paramater(Input [Prompt]) | Response Out |
|---------------------|---------------------|---------------------------|--------------|
| getapitextresponse  | Gemini              | Text                      | Text         |
| getapiimageresponse | Gemini              | Text                      | Image        |
| ping                | None                | None                      | Text         |

### Setup Instructions
1. Get Gemini [Api Key](https://ai.google.dev/gemini-api/docs/api-key)
2. Create a .env file in the root of the project and enter required parameters (or rename .env.example to .env)
### Building & running through docker
1. Ensure you are in the root of the folder
2. ``docker build -t image_name .`` to build the image
3. ``docker run -d --name container_name -e DISCORD_TOKEN=yourtoken -e CONNECTION_STRING=yourstring -e DATABASE_NAME =databasename -e DATABASE_USERNAME=username -e DATABASE_PASSWORD -e DATABASE_PORT=port -e GEMINI_API_KEY=yourapikey -e GEMINI_TEXT_MODEL=textmodelname -e GEMINI_IMAGE_MODEL=imagemodelname -e GEMINI_TEXT_MODEL_MINUTE_LIMIT=number -e GEMINI_IMAGE_MODEL_MINUTE_LIMIT=number -e GEMINI_TEXT_MODEL_DAILY_LIMIT=number -e GEMINI_IMAGE_MODEL_DAILY_LIMIT=number image_name`` to run the container


#### Configuration Parameters

| Configuration                   | Purpose                                                       | Required | Example   |
|---------------------------------|---------------------------------------------------------------|---|-----------|
| DISCORD_TOKEN                   | Discord Bot Token                                             | Y |           |
| CONNECTION_STRING               | Discord Bot Token                                             | Y |           |
| DATABASE_NAME                   | Discord Bot Token                                             | Y |           |
| DATABASE_USERNAME               | Postgres connection string                                    | Y |           |
| DATABASE_PASSWORD               | Discord Bot Token                                             | Y |           |
| DATABASE_PORT                   | Discord Bot Token                                             | Y |           |
| DEBUG                           | Enable Debug Mode (Does nothing apart from log config params) | N | FALSE/TRUE |
| GEMINI_API_KEY                  | Gemini API Key                                                | Y |
| GEMINI_API_URL                  | Postgres SSL Mode (preferably 'Prefer')                      | Y |     |
| GEMINI_TEXT_MODEL               | Gemini Text Model                                             | Y |gemini-2.0-flash |
| GEMINI_IMAGE_MODEL              | Gemini Image Model                                            | Y | gemini-2.0-flash-preview-image-generation |
| GEMINI_TEXT_MODEL_MINUTE_LIMIT  | Request Per Minute Limit                                      | N | 10        |
| GEMINI_IMAGE_MODEL_MINUTE_LIMIT | Request Per Minute Limit                                      | N | 10        |
| GEMINI_TEXT_MODEL_DAILY_LIMIT   | Request Per Day Limit                                         | N | 1500      |
| GEMINI_IMAGE_MODEL_DAILY_LIMIT  | Request Per Day Limit                                         | N | 1500      |

#### Example Screenshots
![Image-Response](https://github.com/Peekaey/Shouko/blob/master/Images/Image-response.png)
![Text-Response](https://github.com/Peekaey/Shouko/blob/master/Images/Text-response.png)


