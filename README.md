# Shouko


## TODO
- Implement background service as guardrail against gemini api usage
- Remove ApiResponseSaveResult & DiscordInteractionSaveResult into base ServiceResult/SaveResult class
- Fix CandidateToken/PromptToken Count when saving in ApiResponse 
- Remove Worker.cs
- Move CreateApiText/ImageThread from DiscordInteractionsBusinessService to ApiServiceBusinessService to interface with ApiService 