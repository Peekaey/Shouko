using Microsoft.Extensions.Logging;
using NetCord.Services.ApplicationCommands;

namespace Shouko.Helpers.Extensions;

public static class LoggerExtensions
{
        public static void LogActionTraceStart(this ILogger logger, SlashCommandContext context, string commandName)
    {
        logger.LogTrace($"Start of command {commandName} with interactionId {context.Interaction} initiated by: {context.User} with username: {context.User.GlobalName} " +
                        $"in guild: {context.Guild.Id} with guildname: {context.Guild.Name} " + $"at: {context.Interaction.CreatedAt} ");
    }
    
    public static void LogActionTraceFinish(this ILogger logger, SlashCommandContext context, string commandName)
    {
        logger.LogTrace($"End of command {commandName} with interactionId {context.Interaction.Id} initiated by: {context.User} with username: {context.User.GlobalName} " +
                        $"in guild: {context.Guild.Id} with guildname: {context.Guild.Name} " + $"at: {context.Interaction.CreatedAt} ");
    }

    public static void LogExceptionError(this ILogger logger, SlashCommandContext context, string commandName,
        Exception exception)
    {
        logger.LogCritical(exception, $"Exception error in command {commandName} with interactionId {context.Interaction.Id} initiated by: {context.User} with username: {context.User.GlobalName} " +
                                   $"in guild: {context.Guild.Id} with guildname: {context.Guild.Name} " + $"at: {context.Interaction.CreatedAt}. Reason: {exception.Message} StackTrace: {exception.StackTrace}");   
    }
    
    public static void LogStringError(this ILogger logger, SlashCommandContext context, string commandName, string error)
    {
        logger.LogError($"Error in command {commandName} with interactionId {context.Interaction.Id} initiated by: {context.User} with username: {context.User.GlobalName} " +
                                   $"in guild: {context.Guild.Id} with guildname: {context.Guild.Name} " + $"at: {context.Interaction.CreatedAt}. Reason: {error}");   
    }
    
    public static void LogUnhandledError(this ILogger logger, string commandName, Exception error)
    {
        logger.LogError($"Unhandled error in command {commandName}. Reason: {error.Message} StackTrace: {error.StackTrace}");
    }
}